#This script will download the files from WSO SFTP. 


#input params
param ($fileDownloadList, $fileDownloadPath, $ftpRemotePath, $totalAttempts, $attemptTimeInterval, $logFile)

# Use below block for testing
#$fileDownloadList = 'TradeDateCashCLO120210624.csv,TradeDateCashCLO220210624.csv'
#$fileDownloadPath = "C:\KLIM\*"
<<<<<<< HEAD
#$ftpRemotePath = "/"
=======
#$ftpRemotePath = "/mackay/"
>>>>>>> origin/Dev
#$totalAttempts = 5
#$attemptTimeInterval = 2		# in seconds
#$logFile = "C:\Users\rpatkar\Desktop\KLIM\SFTP_Download_20210624.txt"

#Function to be used for logging
Function LogWrite
{
	Param ([string]$logstring)
	$DateStr = Get-Date -Format "MM/dd/yyyy HH:mm:ss "
	Add-content $logFile -value "$DateStr - $logstring"
}

LogWrite("----------------- SFTP Download processing START -----------------") 
LogWrite("Input param list:") 
LogWrite("fileDownloadList: $fileDownloadList") 
LogWrite("fileDownloadPath: $fileDownloadPath") 
LogWrite("ftpRemotePath: $ftpRemotePath") 
LogWrite("totalAttempts: $totalAttempts") 
LogWrite("attemptTimeInterval: $attemptTimeInterval") 
LogWrite("logFile: $logFile") 
LogWrite("")

#Default variables
$currentDateTime = Get-Date -Format "yyyyMMddhhmss";
$attemptCount = 0
$checkAndDownloadFilesFlag = 1
$ftpDownloadedFiles = ""
$ftpMissingFiles = ""
<<<<<<< HEAD
$fileArchivePath = "\\KL02WSODB\Backups\WSOFiles_Archive\"
=======
$fileArchivePath = "\\KL02WSODB\Backups\WSOFiles-UAT_Archive\"
>>>>>>> origin/Dev

#WSO SFTP details
$hostName = "sftp.wsoweb.com"
$userName = "kennedylewis"
$password = "GdRePSUC9aAA"
$SshHostKeyFingerprint = "ssh-rsa 1024 /BddYPYZKLNxfOHV3L+RSkaxrU7ofzTSI62+ErYoht0="

#Loop after the defined interval to check and download files if exists
do {
	$attemptCount += 1
	
	LogWrite("") 
	LogWrite("--- File Download Attempt #$attemptCount ---") 
	
	if ($ftpMissingFiles -ne "")
	{
		$fileDownloadList = $ftpMissingFiles;
	}
	
	$ftpMissingFiles = "";
	$ftpDownloadedFiles = "";
		
	try
	{
		# Load WinSCP .NET assembly
		Add-Type -Path "C:\Program Files (x86)\WinSCP\WinSCPnet.dll"
		
		# Set up session options
		$sessionOptions = New-Object WinSCP.SessionOptions -Property @{
			Protocol = [WinSCP.Protocol]::Sftp
			HostName = $hostName
			UserName = $userName
			Password = $password 
			SshHostKeyFingerprint = $SshHostKeyFingerprint
		}

		$session = New-Object WinSCP.Session
		
		try
		{
			# Connect
			$session.Open($sessionOptions)

			$files = $fileDownloadList.split(",");
			foreach($file in $files)
			{ 
				$fileName = $file.Trim();
				$getfile = $ftpRemotePath + $fileName.Trim();
				if ($session.FileExists($getfile))
				{
					# Transfer files
					$session.GetFiles("$getfile", "$fileDownloadPath").Check()
					
					Copy-Item "$fileDownloadPath$fileName" -Destination "$fileArchivePath"
					
					#Set list of downloaded files in this attempt
					$ftpDownloadedFiles = $ftpDownloadedFiles + $file + ",";
				}
				else
				{
					#Set list of missing files in this attempt
					$ftpMissingFiles = $ftpMissingFiles + $file + ",";
				}
			}
		}
		finally
		{
			$session.Dispose()
		}
	}
	catch
	{
		$errorMessage = "Error in file download - Exception Message: $($_.Exception.Message)";
		LogWrite($errorMessage)
		return $errorMessage
	}
		
	if ($ftpMissingFiles -eq "")
	{
		$checkAndDownloadFilesFlag = 0;
		
		$ftpDownloadedFiles = $ftpDownloadedFiles.Substring(0,$ftpDownloadedFiles.Length-1)
		LogWrite("Downloaded files: $ftpDownloadedFiles")
		LogWrite("Missing files: $ftpMissingFiles")
	}
	else
	{
		$ftpMissingFiles = $ftpMissingFiles.Substring(0,$ftpMissingFiles.Length-1)
		
		LogWrite("Downloaded files: $ftpDownloadedFiles")
		LogWrite("Missing files: $ftpMissingFiles")
		
		if ($attemptCount -lt $totalAttempts)
		{
			LogWrite("Waiting for $attemptTimeInterval seconds for next attempt")
			Start-Sleep -Seconds $attemptTimeInterval
		}
	}
		
} while(($attemptCount -lt $totalAttempts) -and ($checkAndDownloadFilesFlag -eq 1))

if ($ftpMissingFiles -ne "")
{
	LogWrite("")
	LogWrite("Process completed and given files are missing, Please co-ordinate with the support team for same. ")
	LogWrite("Missing files: $ftpMissingFiles")
	LogWrite("")
	
	LogWrite("----------------- SFTP Download processing END -----------------") 
	LogWrite("")

	$errorMessage = "Process completed and given files are missing, Please co-ordinate with the support team for same. 
	Missing files: $ftpMissingFiles";
	return $errorMessage
}
else
{
	LogWrite("")
	LogWrite("Process completed and all files are downloaded")
	LogWrite("")
}

LogWrite("----------------- SFTP Download processing END -----------------") 
LogWrite("")


#$param1=$args[0]
#$param2=$args[1]
#$param1
#$param2

#input params
$totalAttempts = 5;
$timeInterval = 1; # in seconds
$filelist = 'Positions20210621.CSV,Positions20210606.CSV';	# pipe separated file list to download
$remotePath = "/mackay/";
$downloadPath = "C:\KLIM\WSOFiles\*";


#Default variables
$CurrentDate = Get-Date -Format "yyyyMMddhhmss";
$LogfilePath = "C:\KLIM\Logs\"; 
$counter = 0;
$CheckAndDownloadFiles = 1;
$DownloadedFiles = "";
$MissingFiles = "";
$DownloadFileList = $filelist;

#Function to be used for logging
$Logfile = "$LogfilePath" + "FTP_Download_$CurrentDate.txt"; 
New-Item $Logfile
Function LogWrite
{
	Param ([string]$logstring)
	$DateStr = Get-Date -Format "MM/dd/yyyy HH:mm:ss "
	Add-content $Logfile -value "$DateStr - $logstring"
}

LogWrite("----------------- PROCESSING START -----------------") 
LogWrite("Download file list: $filelist") 

#Loop after the defined interval to check if file exists
do {
	$counter += 1
	LogWrite("") 
	LogWrite("--- File Download Attempt #$counter ---") 
	
	if ($MissingFiles -ne "")
	{
		$DownloadFileList = $MissingFiles;
	}
	
	$MissingFiles = "";
	$DownloadedFiles = "";
		
	try
	{
		# Load WinSCP .NET assembly
		Add-Type -Path "C:\Program Files (x86)\WinSCP\WinSCPnet.dll"
		
		# Set up session options
		$sessionOptions = New-Object WinSCP.SessionOptions -Property @{
			Protocol = [WinSCP.Protocol]::Sftp
			HostName = "sftp.wsoweb.com"
			UserName = "Mackay"
			Password = "z7aoJBwu"
			SshHostKeyFingerprint = "ssh-rsa 1024 /BddYPYZKLNxfOHV3L+RSkaxrU7ofzTSI62+ErYoht0="
		}

		$session = New-Object WinSCP.Session
		
		LogWrite("Processing files: $DownloadFileList")
				
		try
		{
			# Connect
			$session.Open($sessionOptions)

			$files = $DownloadFileList.split(",");
			foreach($file in $files)
			{ 
				$getfile = "$remote$file";
				if ($session.FileExists($getfile))
				{
					#LogWrite("File Exists") 
					# Transfer files
					$session.GetFiles("$getfile", "$downloadPath").Check()
					
					$DownloadedFiles = $DownloadedFiles + $file + ",";
				}
				else
				{
					#LogWrite("File Not Exists") 
					$MissingFiles = $MissingFiles + $file + ",";
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
		$errorMessage = $($_.Exception.Message);
		LogWrite("Send-ToEmail - Exception Message: $($_.Exception.Message)")
	}
		
	if ($MissingFiles -eq "")
	{
		$CheckAndDownloadFiles = 0;
		
		LogWrite("Downloaded files: $DownloadedFiles")
		LogWrite("Missing files: $MissingFiles")
	}
	else
	{
		$MissingFiles = $MissingFiles.Substring(0,$MissingFiles.Length-1)
		
		LogWrite("Downloaded files: $DownloadedFiles")
		LogWrite("Missing files: $MissingFiles")
		
		Start-Sleep -Seconds $timeInterval
	}
		
} while(($counter -lt $totalAttempts) -and ($CheckAndDownloadFiles -eq 1))

if ($MissingFiles -ne "")
{
	LogWrite("")
	LogWrite("Process completed and given files are missing, Please co-ordinate with the support team for same. ")
	LogWrite("Missing files: $MissingFiles")
}
	
LogWrite("----------------- PROCESSING STOPED -----------------") 	
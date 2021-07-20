
#This script will download the TradeDateCash files from WSO SFTP. 

#Default variables
$jobName = "FileDownload_TradeDateCash"
$fileDate = Get-Date -Format "yyyyMMdd";
# $fileDate = (Get-Date -Format "yyyyMMdd")-1;
$currentDateTime = Get-Date -Format "yyyyMMddhhmss";
$logFile = "C:\KLIM\Logs\$jobName$currentDateTime.txt"

#Function to be used for logging
New-Item $logFile
Function LogWrite
{
	Param ([string]$logstring)
	$DateStr = Get-Date -Format "MM/dd/yyyy HH:mm:ss "
	Add-content $logFile -value "$DateStr - $logstring"
}

LogWrite("----------------- Processing START -----------------") 

#File download variables
$sftpDownlodScriptPath = "C:\KLIM\Scripts\SFTPDownload_WSO.ps1"
$fileDownloadList = "TradeDateCashCLO1$fileDate.csv,TradeDateCashCLO2$fileDate.csv,TradeDateCashCLO3$fileDate.csv,TradeDateCashCLO4$fileDate.csv,TradeDateCashCLO5$fileDate.csv,TradeDateCashCLO6$fileDate.csv,TradeDateCashCLO7$fileDate.csv,TradeDateCashCLO8$fileDate.csv,TradeDateCashCLO9$fileDate.csv"
$fileDownloadPath = "\\KL02WSODB\Backups\WSOFiles\TradeDateCash\"
$ftpRemotePath = "/"
$totalAttempts = 12
$attemptTimeInterval = 300		# in seconds

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
$toEmail = @('rakesh.patkar@ap.linedata.com','Diwakar.Singh@ap.linedata.com')
$ccEmail = ""
$subject = "ALERT: Job $jobName Failed"
$body = ""
$attachment = ""

LogWrite("Calling ftp download script") 
LogWrite("$sftpDownlodScriptPath -fileDownloadList $fileDownloadList -fileDownloadPath $fileDownloadPath -ftpRemotePath $ftpRemotePath -totalAttempts $totalAttempts -attemptTimeInterval $attemptTimeInterval -logFile $logFile") 
$returnMessage = &"$sftpDownlodScriptPath" -fileDownloadList "$fileDownloadList" -fileDownloadPath "$fileDownloadPath" -ftpRemotePath "$ftpRemotePath" -totalAttempts "$totalAttempts" -attemptTimeInterval "$attemptTimeInterval" -logFile "$logFile"

LogWrite("returnMessage = "+$returnMessage) 

if (($returnMessage -eq "") -or ($returnMessage -eq $null))
{
	LogWrite("ftp download process completed") 
}
else
{
	LogWrite("Calling send email script") 
	LogWrite("$sendEmailScriptPath -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $body -attachment $logFile -logFile $logFile") 
	$returnMessage = &"$sendEmailScriptPath" -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $returnMessage -attachment $logFile -logFile $logFile
	
	exit 1
}

LogWrite("")
LogWrite("----------------- Processing END -----------------") 
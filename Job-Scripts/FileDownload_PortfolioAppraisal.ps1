
#This script will download the Portfolio Position Appraisal files from WSO SFTP. 

#Default variables
$jobName = "FileDownload_PortfolioAppraisal"
$fileDate = Get-Date -Format "yyyyMMdd";
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
$fileDownloadList = 'CLO1PortfolioPositionAppraisal$fileDate.pdf,CLO2PortfolioPositionAppraisal$fileDate.pdf,CLO3PortfolioPositionAppraisal$fileDate.pdf,CLO4PortfolioPositionAppraisal$fileDate.pdf,CLO5PortfolioPositionAppraisal$fileDate.pdf,CLO6PortfolioPositionAppraisal$fileDate.pdf,CLO7PortfolioPositionAppraisal$fileDate.pdf,CLO8PortfolioPositionAppraisal$fileDate.pdf,CLO9PortfolioPositionAppraisal$fileDate.pdf'
$fileDownloadPath = "\\KL02WSODB\Backups\WSOFiles-UAT\CLO\PortfolioAppraisal\"
$ftpRemotePath = "/mackay/"
$totalAttempts = 12
$attemptTimeInterval = 300		# in seconds

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
$toEmail = "rakesh.patkar@ap.linedata.com,diwakar.singh@ap.linedata.com"
$ccEmail = ""
$subject = "ALERT: Job $jobName Failed"
$body = ""
$attachment = ""

LogWrite("Calling ftp download script") 
LogWrite("$sftpDownlodScriptPath -fileDownloadList $fileDownloadList -fileDownloadPath $fileDownloadPath -ftpRemotePath $ftpRemotePath -totalAttempts $totalAttempts -attemptTimeInterval $attemptTimeInterval -logFile $logFile") 
$returnMessage = &"$sftpDownlodScriptPath" -fileDownloadList "$fileDownloadList" -fileDownloadPath "$fileDownloadPath" -ftpRemotePath "$ftpRemotePath" -totalAttempts "$totalAttempts" -attemptTimeInterval "$attemptTimeInterval" -logFile "$logFile"

if ($returnMessage -eq "")
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
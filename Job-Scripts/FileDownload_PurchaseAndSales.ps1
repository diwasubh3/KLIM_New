
#This script will download the Purchase And Sales files from WSO SFTP. 

#Default variables
$jobName = "FileDownload_PurchaseAndSales"
<<<<<<< HEAD
#$fileDate = (Get-Date -Format "yyyyMMdd")-1;
=======
>>>>>>> origin/Dev
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
<<<<<<< HEAD
$fileDownloadList = "CLOPurchaseSalewithComments$fileDate.pdf"
$fileDownloadPath = "\\KL02WSODB\Backups\WSOFiles\CLO\PurchaseAndSales\"
$ftpRemotePath = "/"
=======
$fileDownloadList = 'CLOPurchaseSalewithComments$fileDate.pdf'
$fileDownloadPath = "\\KL02WSODB\Backups\WSOFiles-UAT\CLO\PurchaseAndSales\"
$ftpRemotePath = "/mackay/"
>>>>>>> origin/Dev
$totalAttempts = 12
$attemptTimeInterval = 300		# in seconds

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
<<<<<<< HEAD
$toEmail = @('rakesh.patkar@ap.linedata.com','Diwakar.Singh@ap.linedata.com')
=======
$toEmail = "rakesh.patkar@ap.linedata.com,diwakar.singh@ap.linedata.com"
>>>>>>> origin/Dev
$ccEmail = ""
$subject = "ALERT: Job $jobName Failed"
$body = ""
$attachment = ""

LogWrite("Calling ftp download script") 
LogWrite("$sftpDownlodScriptPath -fileDownloadList $fileDownloadList -fileDownloadPath $fileDownloadPath -ftpRemotePath $ftpRemotePath -totalAttempts $totalAttempts -attemptTimeInterval $attemptTimeInterval -logFile $logFile") 
$returnMessage = &"$sftpDownlodScriptPath" -fileDownloadList "$fileDownloadList" -fileDownloadPath "$fileDownloadPath" -ftpRemotePath "$ftpRemotePath" -totalAttempts "$totalAttempts" -attemptTimeInterval "$attemptTimeInterval" -logFile "$logFile"

<<<<<<< HEAD
LogWrite("returnMessage = "+$returnMessage) 

if (($returnMessage -eq "") -or ($returnMessage -eq $null))
=======
if ($returnMessage -eq "")
>>>>>>> origin/Dev
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
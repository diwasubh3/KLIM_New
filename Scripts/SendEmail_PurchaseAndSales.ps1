
#This script will email the downloaded PurchaseAndSales files from WSO SFTP. 

#Default variables
$jobName = "SendEmail_PurchaseAndSales"
#$fileDate = (Get-Date -Format "yyyyMMdd")-1;
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

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
$toEmail = @('yorkclo@yorkcapital.com','josh.smith@klimllc.com')
$ccEmail = ""
$subject = "Purchase And Sales Files - $fileDate"
$body = "Attached Purchase And Sales Files."
$attachment = (get-childitem "\\KL02WSODB\Backups\WSOFiles\CLO\PurchaseAndSales\CLOPurchaseSalewithComments*$fileDate*").fullname 


LogWrite("Calling send email script") 
LogWrite("$sendEmailScriptPath -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $body -attachment $attachment -logFile $logFile") 
$returnMessage = &"$sendEmailScriptPath" -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $body -attachment $attachment -logFile $logFile
	
LogWrite("returnMessage = "+$returnMessage) 

if (($returnMessage -eq "") -or ($returnMessage -eq $null))
{
	LogWrite("Send email process completed.") 
}
else
{
	#LogWrite("Send email process failed with error message: $returnMessage") 
	exit 1
}

LogWrite("")
LogWrite("----------------- Processing END -----------------") 
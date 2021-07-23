
#input params
param ($toEmail, $ccEmail, $subject, $body, $attachment, $logFile)
$subject = "PROD - " + $subject;

# Use below block for testing
#$toEmail = "rakesh.patkar@ap.linedata.com"
#$ccEmail = ""
#$subject = "Test email"
#$body = "Test email from Email script"
#$attachment = ""
#$logFile = "C:\Users\rpatkar\Desktop\KLIM\Send_Email_20210624.txt"

#Function to be used for logging
Function LogWrite
{
	Param ([string]$logstring)
	$DateStr = Get-Date -Format "MM/dd/yyyy HH:mm:ss "
	Add-content $logFile -value "$DateStr - $logstring"
}

LogWrite("----------------- Send Email processing START -----------------") 
LogWrite("Input param list:") 
LogWrite("toEmail: $toEmail") 
LogWrite("ccEmail: $ccEmail") 
LogWrite("subject: $subject") 
LogWrite("body: $body") 
LogWrite("attachment: $attachment") 
LogWrite("logFile: $logFile") 
LogWrite("")

#Default variables
$currentDateTime = Get-Date -Format "yyyyMMddhhmss";
$fromEmail = "Job-Notification@klimllc.com"
$SmtpServer = "klimllc-com.mail.protection.outlook.com"

LogWrite("Sending email") 
try
{
	if (($ccEmail -ne "") -and ($attachment -ne ""))
	{
		LogWrite("Condition 1") 
		LogWrite("Send-MailMessage -From $fromEmail -To $toEmail -Cc $ccEmail -Subject $subject -Body $body -Attachments $attachment -SmtpServer $SmtpServer") 
		Send-MailMessage -From $fromEmail -To $toEmail -Cc $ccEmail -Subject $subject -Body $body -Attachments $attachment -SmtpServer $SmtpServer
	}
	elseif (($ccEmail -eq "") -and ($attachment -ne ""))
	{
		LogWrite("Condition 2") 
		LogWrite("Send-MailMessage -From $fromEmail -To $toEmail -Subject $subject -Body $body -Attachments $attachment -SmtpServer $SmtpServer")
		Send-MailMessage -From $fromEmail -To $toEmail -Subject $subject -Body $body -Attachments $attachment -SmtpServer $SmtpServer
	}
	elseif (($ccEmail -ne "") -and ($attachment -eq ""))
	{
		LogWrite("Condition 3") 
		LogWrite("Send-MailMessage -From $fromEmail -To $toEmail -Cc $ccEmail -Subject $subject -Body $body -SmtpServer $SmtpServer")
		Send-MailMessage -From $fromEmail -To $toEmail -Cc $ccEmail -Subject $subject -Body $body -SmtpServer $SmtpServer
	}
	elseif (($ccEmail -eq "") -and ($attachment -eq ""))
	{
		LogWrite("Condition 4") 
		LogWrite("Send-MailMessage -From $fromEmail -To $toEmail -Subject $subject -Body $body -SmtpServer $SmtpServer")
		Send-MailMessage -From $fromEmail -To $toEmail -Subject $subject -Body $body -SmtpServer $SmtpServer
	}
	
}
catch
{
	$errorMessage = "Error in sending email - Exception Message: $($_.Exception.Message)";
	LogWrite("")
	LogWrite($errorMessage)
	return $errorMessage
}

LogWrite("")	
LogWrite("----------------- Send Email processing END -----------------") 
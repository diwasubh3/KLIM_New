
#This script will Execute the .exe

#Default variables
$jobName = "RunCmd_SendRatingsChangeEmail"
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

function ThrowOnNativeFailure {
    if (-not $?)
    {
        throw 'Native Failure'
    }
}

LogWrite("----------------- Processing START -----------------") 

#Run Command Variables
<<<<<<< HEAD
$runCommand = "Invoke-WebRequest -Uri http://clo.kl.local/calculation/sendratingschangeemail -Method POST"

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
$toEmail = @('rakesh.patkar@ap.linedata.com','Diwakar.Singh@ap.linedata.com')
=======
$runCommand = "Invoke-WebRequest -Uri http://clo-uat.kl.local/calculation/sendratingschangeemail -Method POST"

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
# $toEmail = "rakesh.patkar@ap.linedata.com,diwakar.singh@ap.linedata.com"
$toEmail = "arti.mukherjee@ap.linedata.com"
>>>>>>> origin/Dev
$ccEmail = ""
$subject = "ALERT: Job $jobName Failed"
$body = ""
$attachment = ""

try
{
	LogWrite("")
	LogWrite("Running command:- $runCommand")
	
<<<<<<< HEAD
	$returnMessage = Invoke-WebRequest -Uri http://clo.kl.local/calculation/sendratingschangeemail -Method POST
=======
	$returnMessage = Invoke-WebRequest -Uri http://clo-uat.kl.local/calculation/sendratingschangeemail -Method POST
>>>>>>> origin/Dev
	ThrowOnNativeFailure
	
	LogWrite("")
	LogWrite("Run command completed with message: $returnMessage")
}
catch
{
	$errorMessage = "Error in run command - Exception Message: $($_.Exception.Message)";
	LogWrite("")
	LogWrite($errorMessage)
	LogWrite("")
	LogWrite("Sending Alert email") 
	LogWrite("$sendEmailScriptPath -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $body -attachment $logFile -logFile $logFile") 
	$returnMessage = &"$sendEmailScriptPath" -toEmail $toEmail -ccEmail $ccEmail -subject $subject -body $errorMessage -attachment $logFile -logFile $logFile
	
	exit 1
}


LogWrite("")
LogWrite("----------------- Processing END -----------------") 
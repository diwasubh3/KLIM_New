
#This script will Execute the .exe

#Default variables
$jobName = "RunCmd_ProcessTradeFiles"
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
$runCommand = "Invoke-WebRequest -Uri http://clo-uat.kl.local/calculation/processtradefiles -Method POST"

#Send Email variables
$sendEmailScriptPath = "C:\KLIM\Scripts\Send_Email.ps1"
$toEmail = "rakesh.patkar@ap.linedata.com,diwakar.singh@ap.linedata.com"
$ccEmail = ""
$subject = "ALERT: Job $jobName Failed"
$body = ""
$attachment = ""

try
{
	LogWrite("")
	LogWrite("Running command:- $runCommand")
	
	$returnMessage = Invoke-WebRequest -Uri http://clo-uat.kl.local/calculation/processtradefiles -Method POST
	#$returnMessage = Invoke-WebRequest -Uri https://api.narrativescience.com/v4/qa/5d07f0307dfa39761fb0fd97/story/ -Method POST
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
$user = "dkvpn" 
$pwd = "!Tech2010!@" 
$secure_pwd = $pwd | ConvertTo-SecureString -AsPlainText -Force 
$creds = New-Object System.Management.Automation.PSCredential -ArgumentList $user, $secure_pwd

Invoke-Command -ComputerName "os-dcpp102.cardinalpeak.com" -credential $creds -ErrorAction Stop -ScriptBlock {Invoke-Expression -Command:"cmd.exe /c 'C:\Users\dkvpn\Desktop\gcdssync.bat'"}
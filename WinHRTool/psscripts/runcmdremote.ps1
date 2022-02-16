Param($param1, $param2, $param3, $param4)

$user = $param1 
$pwd = $param2 
$secure_pwd = $pwd | ConvertTo-SecureString -AsPlainText -Force 
$creds = New-Object System.Management.Automation.PSCredential -ArgumentList $user, $secure_pwd

Invoke-Command -ComputerName $param3 -credential $creds -ErrorAction Stop -ScriptBlock {Invoke-Expression -Command:$param4}
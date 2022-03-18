<#
    Author: Derek Baugh
    Created: 01/25/21
    Title: All Users
    Description: Retrieves and edits AD account properties
#>
$user='cardinalpeak.com\chewie'; 
$pass='!Tech2010!@'; 

try { Invoke-Command -ScriptBlock {Get-ADUser -Filter * -Path "OU=Users,OU=CardinalPeak,DC=cardinalpeak,DC=com" -Server OS-DCPP102 } 
-ComputerName OS-DCPP102 -Credential (New-Object System.Management.Automation.PSCredential $user, (ConvertTo-SecureString $pass -AsPlainText -Force)) 
} catch { Write-Output $_.Exception.Message }

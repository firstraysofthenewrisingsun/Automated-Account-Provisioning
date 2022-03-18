# S.U.M (Simple User Management)

Provides an easy interface to create necessary accounts. Automates the creation & update of accounts across common services utillized by CP

## Description

### Creates accounts for:

* Active Directory (.NET AD modules)
* Google (.bat stored on os-dccp102 called from app runs GCDS)
* Last Pass (REST API), removed AD sync tool due to replication errors
* Harvest (REST API), assigns to PTO, Bench & Holiday groups
* Sends SMS to user with temporary AD credentials (Twilio API)
* Sends notification emails to relevant personnel for all new accounts and edits (.NET SMTP Client)

#### Note: AD is source of truth. Password changes start from AD and push to all services. Implementing federation/SSO as part of infrastructure update

## Getting Started

### Installing

* Installer will be added soon

### Executing program

* Install the executable and run "WinHRTool" 

### Troubleshooting
* Custom error codes and common fixes can be found in the troubleshooting guide in Confluence.

## Authors

Derek is the primary contact for this project




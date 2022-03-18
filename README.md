# Automated User Account Provisioning

Automates the creation & update of user accounts. Built this to automate the user on/off boarding process for my jobs HR department.

## Creates accounts for:

* Active Directory (.NET AD modules)
* Google (batch script running GCDS stored on DC, called from app)
* Last Pass (REST API)
* Harvest (REST API)
* Sends SMS to user with temporary AD credentials (Twilio API)
* Sends notification emails to relevant personnel for all new accounts and edits (.NET SMTP Client)

### Executing program

* Build the executable and run "WinHRTool.exe" 

### Troubleshooting
* Custom error codes and common fixes:
| Syntax      | Description | Test Text     |
| :----:      |    :----:   |    :----:     |
| Header      | Title       | Here's this   |
| Paragraph   | Text        | And more      |




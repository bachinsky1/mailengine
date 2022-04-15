# Mail engine guid line
1. Clone repo to folder_for_project
2. Open MailEngine.sln in Visual Studio
3. Set mail credentionals in ImapSettings.cs file
4. Set SendGrid key in MessageController.cs in string 89
5. Run application in IIS Express or in Docker
6. Look at the port https://localhost:[PORT]/swagger/index.html and change port in index.html in 400 line to port from your started local server
8. Open index.html and use mail client for directly send mail, or send mail via SendGrid
9. By analogy, you can connect any other mail sender using the SendGrid model

## Remark
If you want to use your Gmail, firstly need to create virtual device and user password from it https://security.google.com/settings/security/apppasswords as an IMAP/SMTP password.
Another mail servers, like as outlook.com, uses a regular email password for connection

It should been ran as a remote service in Google cloud or as a local service, integrated to Katarina Manager, as a separated application. Or as a Windows Service.

Service use a Mail.dll component https://www.limilabs.com/mail


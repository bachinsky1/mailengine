# Mail engine guideline
1. Clone repo to some folder
2. Open MailEngine.sln in Visual Studio
3. Set mail credentionals in UserSettings.cs file
4. Set SendGrid key in UserSettings.cs file
5. Run application in IIS Express or in Docker
6. Look at the port https://localhost:[PORT]/swagger/index.html and change port in index.html in 408 line to port from your started local server
8. Open index.html and use mail client for directly send mail, or send mail via SendGrid
9. By analogy, you can connect any other mail sender using the SendGrid model

## Remarks
If you want to use your Gmail, firstly need to create virtual device and use password from it https://security.google.com/settings/security/apppasswords as an IMAP/SMTP password.
Another mail servers, like as outlook.com, uses a regular email password for connection

It should been ran as a remote service in Google cloud or as a local service, integrated to Katarina Manager, as a separated application. Or as a Windows Service.

Application use a not free **Mail.dll** component https://www.limilabs.com/mail. Price here https://www.limilabs.com/mail/purchase


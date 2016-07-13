###README###
* NoteShare

* Assets for Deployment

1. Visual Studio 2013 or later
2. Web Server + SQL Server 2008/2012
3. This project


* Build Instructions

1. Load Noteshare project in Visual Studio 2013+
2. Specify "Default Connection" in Web.config
3. Debug Application and register account. This will be your admin account.
4. Verify in SQL server account created and promote permission to level 2.
5. In solution right-click select Publish...
6. Create new publish profile.
7. Enter profile name.
8. Publish method FTP and enter server credentials.
9. Use "Release" configuration.
10. Click publish and see the successful publish.
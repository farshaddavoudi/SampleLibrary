# Server:
	* Using .Net 5
	* SQL Server database
	* Code-First approach
	* AutoRegisterDi as services registrar
	* AutoMapper as mapper
	* Authentication from ATA SSO 
		- ddl file and doc are included 
	* Using HttpClientFactory typed client
		- Connecting to Security client
	* Independent Server and Client approach with only a shared layer for common models and services 
	* Using HealthChecks for check Database, Server DiskStorage, SSO Service and etc; all hosted by healthcheck.app.ataair.ir
		- Do not have any authentication for the endpoint.

# Client:
	* Blazor .Net5 (standalone)
	* Bootstrap 5 rtl
	* Using SASS instead of CSS for styles
	* Using HttpClientFactory typed client
		- Connecting to Security client
	* Using DevExpress Uploader component 
	* Include roles heavily in the logic using Blazor features (refer to doc: تحلیل سامانه)
	* Using Syncfusion PDF viewer for reading PDFs without downnloading them
		- Configure lazy loading for Syncfusion PdfViewer for app initial startup faster
	* ATA layout included heavily with help of our designer Majid Badkoubeh
	* Not using custom web.config because of DevExpress uploader error and problem with IISExpress

# Publish:
	* Only Master branch to publish (develop branch to development)
	* Using Web Deploy Publish 
	* Manually in published path in server, copy PublishAssets -> InstallingLoader folder blazor.webassembly.js files (3 files) into _framework 
		- It is temporarily because of lack of Azure Pipeline CI/CD. 
	* Backup automatically creates on Server
	* No need to stop IIS to publish. It will automatically will do that :)
	* Make application like offline by app_offline.htm files modifications on the server (different senarios for Client and Server apps)

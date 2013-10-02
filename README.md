RavenDB Meets Stimulsoft Reporting
==================================

This is a small demo project that shows how you can consume data from RavenDB in a Stimulsoft Report.

- Very simple ASP.Net MVC4 application.
- Uses RavenDB 2.5 [Unbounded Results API](http://ravendb.net/docs/2.5/client-api/advanced/unbounded-results)
- Uses Stimulsoft's HTML5 (aka "Mobile") Viewer for MVC

##Setup

- Get Stimulsoft Reporting from [their Download Page](http://www.stimulsoft.com/en/downloads).
  - The trial version will work fine, but reports will be watermarked as "DEMO".
  - I used version 2013.2.
  - You can download either the "Reports.Web" or "Reports.Ultimate" product.
- Run the Stimulsoft Installer
- Clone or copy this project.
- The project references Stimulsoft's components from the following path:
    `C:\Program Files (x86)\Stimulsoft Reports.Ultimate 2013.2 Trial\Bin`  
  If you installed to a different path, update your references.
- Build the solution and run it.
- Be patient.  It has to do all of the following:
  - Download RavenDB.Embedded and other components from Nuget
  - Build the solution
  - Import the Northwind sample data (included)
  - Build indexes
  - Run the report
- After the first run, and you see the report successfully, subsequent runs should be much faster.


##NOTES / TODO
- The report definition should probably be saved in the database rather than read from the App_Data folder.
- There is currently just a single reporting index and a single report.
- The Stimulsoft designer should be wired up so you can dynamically create new reports, save them back to RavenDB, and run them at will.
- There should be some navigation on the demo site to switch between different reports and the designer.
- It would be awesome if you could build a report *directly* from a RavenDB index stored with the database.
  - Currently it has to have some C# code to load the data.
  - This should be possible, but will require some deeper thought.
- While the report data is streamed out of RavenDB and we use `yield` to return the results, I believe Stimulsoft is still holding an internal `DataSet`.
  -  Need to investigate how well this holds up with a very large report.  Say, > 1,000,000 records.
  -  Need to see if there is a way to get Stimulsoft to stream the input data differently.
  

  

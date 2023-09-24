Before running the application, kindly proceed with the following steps.

1.	Change the connection string and database name in appsettings.json
2.	Delete the Migrations Folder.
3.	Run the following commands in the “Package Manager Console”.
a.	Add-Migration “Initial Migration”
b.	Update-Database
4.	By executing the above, database and table will be created.
5.	Execute the stored procedure “GetVehicle_NonAvailable_HoursPerWeek” in the database.
    Stored procedure is available inside the solution folder with the name “StoredProcedure_GetVehicle_NonAvailable_HoursPerWeek.sql”
6.	Run the application and click the “CSV File Import” menu.
7.	Select the .csv file and click the “Import” button to upload the data to the database table.
8.	Check whether the data has been inserted into the table “GeoFencePeriods”.
9.	The “enter time” and “exit time” is getting inserted with -12 hour into the table. There was no datetime conversion implemented. 
10.	Since I haven’t validated the data, I added +12 hour to the enter time and exit time to match with the correct data. For that, please execute the following query.

UPDATE	GeoFencePeriods SET	EnterTime = DATEADD(HOUR,12,EnterTime),
ExitTime = DATEADD(HOUR,12,ExitTime)

11.	Click the “Vehicle Non Availability” menu.
12.	Select the “Month Start Date” as “01/06/2023” (1st June 2023).
13.	Click the “Display Data” button to view the data.
14.	Click the “Close” button to close the screen.

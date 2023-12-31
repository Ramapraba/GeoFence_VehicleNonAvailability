USE [GeoFencePeriod]
GO
/****** Object:  StoredProcedure [dbo].[GetVehicle_NonAvailable_HoursPerWeek]    Script Date: 4/09/2023 12:28:01 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetVehicle_NonAvailable_HoursPerWeek]
(
	@ip_MonthStartDate DATETIME -- = '2023-06-01'
)
AS
BEGIN
/*
	EXEC DBO.GetVehicle_NonAvailable_HoursPerWeek '2023-06-01 00:00:00.000'
	
BEGIN TRAN
- To update the data with +12 hour timestamp
UPDATE geoFencePeriods
SET EnterTime = DATEADD(HOUR,12,EnterTime),ExitTime = DATEADD(HOUR,12,ExitTime)
Select * from GeoFencePeriods
ROLLBACK

*/

	DECLARE @dt_MonthStartDate DATETIME = @ip_MonthStartDate,@dt_MonthEndDate DATETIME, @dt_WorkStartDt DATETIME,@dt_WorkEndDt DATETIME,@isWeekDay VARCHAR(10), 
			@distinctVehicleCount INT,@VehicleAvailableCount INT, @VehicleNotAvailableCount INT,@dt_Plus15minuteDate DATETIME
	
	-- Calculate Month End Date
	SELECT @dt_MonthEndDate = EOMONTH(@ip_MonthStartDate)
	-- Get the distinct vehicle count
	SELECT @distinctVehicleCount = COUNT(DISTINCT VehicleId)
	FROM GeoFencePeriods
	
	CREATE TABLE DBO.#tmpVehicleAvailDetails
	(
		PKID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		StartDateTime DATETIME NOT NULL,
		EndDateTime DATETIME NOT NULL,
		VehiclesNOTAvailableCount INT NOT NULL,
		VehiclesAvailableCount INT NOT NULL
	)
	-- Loop through every day in the month
	WHILE (@dt_MonthStartDate <= @dt_MonthEndDate)
	BEGIN
	
		-- Check if the MonthStartDate is a Week Day or Weekend Day. Skip if it is Weekend Day
		SELECT @isWeekDay = DATENAME(WEEKDAY,@dt_MonthStartDate)
		-- Exluding Saturday and Sunday
		IF (@isWeekDay NOT IN ('Saturday','Sunday')) 
		BEGIN
			SET @dt_WorkStartDt = DATEADD(HOUR,8,@dt_MonthStartDate)
			SET @dt_WorkEndDt = DATEADD(HOUR,9,@dt_WorkStartDt)
			SET @dt_WorkStartDt = DATEADD(MINUTE,30,@dt_WorkStartDt)
			-- Loop through each day with every 15 minutes starting from 8:30 AM till 17:00 PM
			WHILE(@dt_WorkStartDt < @dt_WorkEndDt)
			BEGIN

				SELECT @dt_Plus15minuteDate = DATEADD(minute,15,@dt_WorkStartDt)

				Select @VehicleAvailableCount = COUNT(DISTINCT VehicleId)
				from geoFencePeriods 
				Where (EnterTime BETWEEN @dt_WorkStartDt and @dt_Plus15minuteDate)
				OR (ExitTime BETWEEN @dt_WorkStartDt and @dt_Plus15minuteDate)
				OR (@dt_WorkStartDt BETWEEN EnterTime AND ExitTime)
				OR (@dt_Plus15minuteDate BETWEEN EnterTime AND ExitTime)

				-- To find the Vehicle Not Available count, subtract the available count from the distinct vehicle count
				SET @VehicleNOTAvailableCount = @distinctVehicleCount - @VehicleAvailableCount

				INSERT INTO DBO.#tmpVehicleAvailDetails
				SELECT @dt_WorkStartDt,@dt_Plus15minuteDate,@VehicleNOTAvailableCount,@VehicleAvailableCount

				-- Add 15 minutes time block
				SET @dt_WorkStartDt = DATEADD(MINUTE,15,@dt_WorkStartDt)
			END
		END

		SET @dt_MonthStartDate = DATEADD(DAY,1,@dt_MonthStartDate)
	END;

	CREATE TABLE DBO.#tmpResultData
	(
		PKID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
		VehiclesNOTAvailableCount INT NULL,
		AvgWeekHour FLOAT,
		NoOfHoursPerWeek FLOAT
	)
	
	DECLARE @NoOfWeeks FLOAT
	-- To calculate the number of weeks in a month
	SELECT @NoOfWeeks = (DATEPART(dd, EOMONTH(@ip_MonthStartDate)) / 7) + 
	CASE WHEN (DATEPART(dd, EOMONTH(@ip_MonthStartDate)) % 7) > 0 THEN 1 ELSE 0 END;

	-- Divide the Vehicles NOT Available count by 4 (15 mins time block) and again by number of weeks (5 weeks in June 23)
	Insert INTO DBO.#tmpResultData
	Select VehiclesNOTAvailableCount,((COUNT(PKID) / 4.00)/ @NoOfWeeks),NULL
	from	DBO.#tmpVehicleAvailDetails 
	GROUP BY VehiclesNOTAvailableCount;
	
	WITH CTE_Data 
	AS (Select MIN(VehiclesNOTAvailableCount) - 1 AS NotAvailCnt,0.00 as AvgWeekHour,0.00 as NoOfHoursPerWeek
		from DBO.#tmpResultData
		UNION ALL
		SELECT NotAvailCnt - 1,0.00,0.00 FROM CTE_Data WHERE NotAvailCnt > 0)
	
	INSERT INTO DBO.#tmpResultData
	SELECT * FROM CTE_Data
	
	Declare @totRecordCnt INT,@i INT = 1,@VehiclesNOTAvailableCount INT

	SELECT	@totRecordCnt = COUNT(PKID)
	FROM	DBO.#tmpResultData 

	WHILE (@i <= @totRecordCnt)
	BEGIN
		Select	@VehiclesNOTAvailableCount = VehiclesNOTAvailableCount 
		FROM	DBO.#tmpResultData 
		WHERE	PKID = @i

		UPDATE	DBO.#tmpResultData
		SET		NoOfHoursPerWeek = (SELECT SUM(AvgWeekHour) FROM DBO.#tmpResultData WHERE VehiclesNOTAvailableCount <= @VehiclesNOTAvailableCount)
		WHERE	PKID = @i

		SET @i = @i + 1;

	END

	-- Final Result Set
	SELECT	VehiclesNOTAvailableCount,ROUND(NoOfHoursPerWeek,2) AS NoOfHoursPerWeek
	FROM	DBO.#tmpResultData 
	ORDER BY VehiclesNOTAvailableCount
	
END
GO

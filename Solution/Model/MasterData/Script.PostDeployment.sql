/*
Skripta koja na osnovu publish profila radi verzioniranje baze i seedovanje podataka
*/

DECLARE @Environment varchar(100)
SET @Environment = 'LOCAL'

-- Clear and reseed Database
:r .\ClearAndReseed.sql

-- Insert version info
:r .\Script.DatabaseVersioning.sql

-- Always execute these scripts
:r .\UserTypes.Table.sql
:r .\BoatOwnerResponsibilityTypes.Table.sql
:r .\ServiceTypes.Table.sql
:r .\BoatTypes.Table.sql
:r .\Countries.Table.sql
:r .\Cities.Table.sql
:r .\Users.Table.sql

IF @Environment = 'LOCAL'
	BEGIN
		-- Scripts run for local configurations 
		:r .\LOCAL\SystemConfiguration.Table.sql
		:r .\LOCAL\Services.Table.sql
		:r .\LOCAL\Reservations.Table.sql
		:r .\LOCAL\PromoActions.Table.sql
		:r .\LOCAL\AdditionalInstructorInfos.Table.sql
	END
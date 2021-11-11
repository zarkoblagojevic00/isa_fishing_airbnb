/*
Skripta koja na osnovu publish profila radi verzioniranje baze i seedovanje podataka
*/

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
:r .\Services.Table.sql

IF '$(Environment)' = 'LOCAL'
	BEGIN
		-- Scripts run for local configurations 
		:r .\LOCAL\SystemConfiguration.Table.sql
	END
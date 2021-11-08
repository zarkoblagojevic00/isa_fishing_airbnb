﻿/*
Skripta koja na osnovu publish profila radi verzioniranje baze i seedovanje podataka
*/

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

IF '$(Environment)' = 'LOCAL'
	BEGIN
		-- Scripts run for local configurations 
		:r .\LOCAL\SystemConfiguration.Table.sql
	END
﻿CREATE TABLE [dbo].[Cities]
(
	[CityID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(MAX) NULL, 
    [CountryId] INT NULL
)

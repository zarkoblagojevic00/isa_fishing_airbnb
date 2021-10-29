﻿CREATE TABLE [dbo].[Services]
(
	[ServiceID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OwnerId] INT NULL, 
    [ServiceType] INT NULL, 
    [Name] NVARCHAR(MAX) NULL, 
    [PricePerDay] DECIMAL(2, 0) NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    [Longitude] DECIMAL(10, 0) NULL, 
    [Latitude] DECIMAL(10) NULL, 
    [PromoDescription] NVARCHAR(MAX) NULL, 
    [TermsOfUse] NVARCHAR(MAX) NULL, 
    [AdditionalEquipment] NVARCHAR(MAX) NULL, 
    [AvailableFrom] DATETIME2 NULL, 
    [AvailableTo] DATETIME2 NULL, 
    [Capacity] INT NULL, 
    [IsProcentageTakenFromCancelation] BIT NULL, 
    [ProcentageToTake] DECIMAL(2) NULL
)
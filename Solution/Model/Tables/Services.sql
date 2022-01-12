CREATE TABLE [dbo].[Services]
(
	[ServiceId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OwnerId] INT NULL, 
    [ServiceType] INT NULL, 
    [Name] NVARCHAR(MAX) NULL, 
    [PricePerDay] DECIMAL(10, 5) NULL,
    [CityId] INT NULL,
    [Address] NVARCHAR(MAX) NULL, 
    [Longitude] DECIMAL(18, 10) NULL, 
    [Latitude] DECIMAL(18, 10) NULL, 
    [PromoDescription] NVARCHAR(MAX) NULL, 
    [TermsOfUse] NVARCHAR(MAX) NULL, 
    [AdditionalEquipment] NVARCHAR(MAX) NULL, 
    [AvailableFrom] DATETIME2 NULL, 
    [AvailableTo] DATETIME2 NULL, 
    [Capacity] INT NULL, 
    [IsPercentageTakenFromCanceledReservations] BIT NULL, 
    [PercentageToTake] DECIMAL(10, 5) NULL
)

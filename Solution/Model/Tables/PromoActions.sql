CREATE TABLE [dbo].[PromoActions]
(
	[PromoActionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ServiceId] INT NULL, 
    [StartDateTime] DATETIME2 NULL, 
    [EndDateTime] DATETIME2 NULL, 
    [PricePerDay] DECIMAL(2, 0) NULL, 
    [IsTaken] BIT NULL, 
    [Capacity] INT NULL, 
    [AddedBenefits] NVARCHAR(MAX) NULL
)

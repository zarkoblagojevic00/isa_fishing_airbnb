CREATE TABLE [dbo].[PricelistItems]
(
	[PricelistItemId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NULL, 
    [Price] DECIMAL(18, 3) NULL, 
    [ServiceId] INT NULL
)

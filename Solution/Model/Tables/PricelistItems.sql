CREATE TABLE [dbo].[PricelistItems]
(
	[PricelistItemId] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(MAX) NULL, 
    [Price] DECIMAL NULL, 
    [ServiceId] INT NULL
)

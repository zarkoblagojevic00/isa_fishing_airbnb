CREATE TABLE [dbo].[Cities]
(
	[CityId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(MAX) NULL, 
    [CountryId] INT NULL 
)

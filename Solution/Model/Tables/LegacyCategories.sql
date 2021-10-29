CREATE TABLE [dbo].[LegacyCategories]
(
	[LegacyCategoryID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(MAX) NULL, 
    [MinPoints] INT NULL, 
    [MaxPoints] INT NULL, 
    [Benefits] NVARCHAR(MAX) NULL, 
    [Discount] DECIMAL(2) NULL
)

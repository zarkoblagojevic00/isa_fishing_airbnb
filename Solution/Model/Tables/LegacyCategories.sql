CREATE TABLE [dbo].[LegacyCategories]
(
	[LegacyCategoryId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(MAX) NULL, 
    [MinPoints] INT NULL, 
    [MaxPoints] INT NULL, 
    [Benefits] NVARCHAR(MAX) NULL, 
    [Discount] DECIMAL(18, 3) NULL
)

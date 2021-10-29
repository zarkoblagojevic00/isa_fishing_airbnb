CREATE TABLE [dbo].[Marks]
(
	[MarkID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [Mark] DECIMAL(1, 0) NULL, 
    [Description] NVARCHAR(MAX) NULL
)

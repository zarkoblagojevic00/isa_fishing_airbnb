CREATE TABLE [dbo].[Issues]
(
	[IssueID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [IssuedEntityId] INT NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIME2 NULL
)

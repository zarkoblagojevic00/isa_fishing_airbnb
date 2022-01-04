CREATE TABLE [dbo].[Issues]
(
	[IssueId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [IssuedEntityId] INT NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIME2 NULL, 
    [IsReviewed] BIT NULL
)

CREATE TABLE [dbo].[AccountDeletionRequests]
(
	[UserId] INT NOT NULL, 
    [RequestedDateTime] DATETIME2 NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [IsReviewed] BIT NULL, 
    CONSTRAINT [PK_AccountDeletionRequests] PRIMARY KEY ([UserId]) 
)

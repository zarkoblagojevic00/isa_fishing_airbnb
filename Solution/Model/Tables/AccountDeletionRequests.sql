CREATE TABLE [dbo].[AccountDeletionRequests]
(
	[UserId] INT NOT NULL, 
    [RequestedDateTime] DATETIME2 NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [IsReviewed] BIT NULL 
)

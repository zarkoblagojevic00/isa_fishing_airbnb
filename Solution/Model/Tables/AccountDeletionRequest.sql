CREATE TABLE [dbo].[AccountDeletionRequest]
(
	[UserId] INT NOT NULL, 
    [RequestedDateTime] DATETIME2 NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [IsReviewed] BIT NULL 
)

CREATE TABLE [dbo].[Marks]
(
	[MarkId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [GivenMark] DECIMAL(1, 0) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [ServiceId] INT NULL, 
    [IsApproved] BIT NULL, 
    [IsReviewed] BIT NULL
)

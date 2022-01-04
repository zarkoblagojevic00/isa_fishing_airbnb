CREATE TABLE [dbo].[Marks]
(
	[MarkId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [GivenMark] DECIMAL(18, 3) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [ServiceId] INT NULL, 
    [IsApproved] BIT NULL, 
    [IsReviewed] BIT NULL
)

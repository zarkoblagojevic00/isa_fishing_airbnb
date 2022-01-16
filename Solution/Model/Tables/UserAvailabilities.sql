CREATE TABLE [dbo].[UserAvailabilities]
(
	[UserId] INT NOT NULL, 
    [PeriodStart] DATETIME2 NULL, 
    [PeriodEnd] DATETIME2 NULL, 
    [Status] BIT NULL, 
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
)

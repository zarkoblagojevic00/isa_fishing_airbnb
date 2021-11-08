CREATE TABLE [dbo].[Subscriptions]
(
	[UserId] INT NULL , 
    [ServiceId] INT NULL, 
    [Id] INT NOT NULL, 
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY ([Id]) 
)

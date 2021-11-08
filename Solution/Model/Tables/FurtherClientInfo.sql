CREATE TABLE [dbo].[FurtherClientInfo]
(
	[UserId] INT NOT NULL , 
    [CollectedPoints] INT NULL, 
    [NumberOfPenalties] INT NULL, 
    CONSTRAINT [PK_FurtherClientInfo] PRIMARY KEY ([UserId])
)

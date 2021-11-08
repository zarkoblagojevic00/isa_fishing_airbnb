CREATE TABLE [dbo].[AdditionalVillaServiceInfos]
(
	[ServiceId] INT NOT NULL , 
    [NumberOfBeds] INT NULL, 
    [NumberOfRooms] INT NULL, 
    CONSTRAINT [PK_AdditionalVillaServiceInfos] PRIMARY KEY ([ServiceId])
)

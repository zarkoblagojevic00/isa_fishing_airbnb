CREATE TABLE [dbo].[LinkNavigationBoat]
(
	[ServiceId] INT NOT NULL , 
    [BoatServiceNavigationToolsId] INT NOT NULL, 
    [Id] INT NOT NULL IDENTITY, 
    CONSTRAINT [PK_LinkNavigationBoat] PRIMARY KEY ([Id])
)

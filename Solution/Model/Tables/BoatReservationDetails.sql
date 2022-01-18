CREATE TABLE [dbo].[BoatReservationDetails]
(
	[Id] INT NOT NULL IDENTITY, 
    [BoatOwnerResponsibilityType] INT NULL, 
    [RelevantId] INT NULL, 
    [IsPromo] BIT NULL, 
    CONSTRAINT [PK_BoatReservationDetails] PRIMARY KEY ([Id])
)

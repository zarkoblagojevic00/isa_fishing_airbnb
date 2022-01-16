CREATE TABLE [dbo].[BoatReservationDetails]
(
	[BoatReservationId] INT NOT NULL, 
    [BoatOwnerResponsibilityType] INT NULL, 
    CONSTRAINT [PK_BoatReservationDetails] PRIMARY KEY ([BoatReservationId])
)

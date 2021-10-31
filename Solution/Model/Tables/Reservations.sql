CREATE TABLE [dbo].[Reservations]
(
	[ReservationsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NULL, 
    [ServiceId] INT NULL, 
    [ReservedDateTime] DATETIME2 NULL, 
    [IsPromo] BIT NULL, 
    [StartDateTime] DATETIME2 NULL, 
    [EndDateTime] DATETIME2 NULL, 
    [IsCanceled] BIT NULL, 
    [IsServiceUnavailableMarker] BIT NULL, 
    [ReportId] INT NULL, 
    [MarkId] INT NULL, 
    [AdditionalEquipment] NVARCHAR(MAX) NULL, 
    [Price] DECIMAL(2, 0) NULL
)

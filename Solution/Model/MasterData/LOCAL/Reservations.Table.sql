INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, '2021-12-02 15:43:38.457', 0, 0, 0, null, null,'', 12.22, '2022-10-02 15:43:38.457', '2022-12-02 15:43:38.457')
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (2, 1, DATEADD(day, -3, GETDATE()), 0, 0, 1, null, null,'', 12.22, DATEADD(day, 8, GETDATE()), DATEADD(day, 12, GETDATE()))
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -10, GETDATE()), DATEADD(day, -7, GETDATE()))
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -13, GETDATE()), DATEADD(day, -11, GETDATE()))

--Reservations with quickactions
DECLARE @StartDate AS DATETIME2 = DATEADD(day, 3, GETDATE())
DECLARE @EndDate AS DATETIME2 = DATEADD(day, 6, GETDATE())
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, DATEADD(day, -3, GETDATE()), 1, 0, 0, null, null,'', 12.22, @StartDate, @EndDate)
INSERT INTO PromoActions (ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (1, @StartDate, @EndDate, 12.22, 1, 10, null)

--Reservations with reports
SET IDENTITY_INSERT Reports ON;

DECLARE @ReportId as INT = 1
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, DATEADD(day, -20, GETDATE()), 0, 0, 0, @ReportId, null,'', 12.22, DATEADD(day, -17, GETDATE()), DATEADD(day, -14, GETDATE()))
INSERT INTO Reports (ReportId, ReportText, CreatedDateTime) VALUES (@ReportId, 'Created through seeding the database', GETDATE())

SET IDENTITY_INSERT Reports OFF;

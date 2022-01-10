SET IDENTITY_INSERT Reservations ON;

INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (0, 4, 0, '2021-12-02 15:43:38.457', 0, 0, 0, null, null,'', 12.22, '2022-10-02 15:43:38.457', '2022-12-02 15:43:38.457')
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, 0, DATEADD(day, -3, GETDATE()), 0, 0, 1, null, null,'', 12.22, DATEADD(day, 8, GETDATE()), DATEADD(day, 12, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (2, 4, 0, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -10, GETDATE()), DATEADD(day, -7, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (3, 4, 0, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -13, GETDATE()), DATEADD(day, -11, GETDATE()))

INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (4, 3, 1, '2021-12-02 15:43:38.457', 0, 0, 0, null, null,'', 12.22, '2022-12-03 15:43:38.457', '2021-12-04 15:43:38.457')
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (5, 6, 1, DATEADD(day, -3, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, 8, GETDATE()), DATEADD(day, 12, GETDATE()))

SET IDENTITY_INSERT Reservations OFF;

--Reservations with quickactions
DECLARE @StartDate AS DATETIME2 = DATEADD(day, 3, GETDATE())
DECLARE @EndDate AS DATETIME2 = DATEADD(day, 6, GETDATE())
SET IDENTITY_INSERT Reservations ON;
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (6, 4, 0, DATEADD(day, -3, GETDATE()), 1, 0, 0, null, null,'', 12.22, @StartDate, @EndDate)
SET IDENTITY_INSERT Reservations OFF;
SET IDENTITY_INSERT PromoActions ON;
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (3, 0, @StartDate, @EndDate, 12.22, 1, 10, null)
SET IDENTITY_INSERT PromoActions OFF;

--Reservations with reports
DECLARE @ReportId as INT = 1
SET IDENTITY_INSERT Reservations ON;
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (7, 4, 0, DATEADD(day, -20, GETDATE()), 0, 0, 0, @ReportId, null,'', 12.22, DATEADD(day, -17, GETDATE()), DATEADD(day, -14, GETDATE()))
SET IDENTITY_INSERT Reservations OFF;
SET IDENTITY_INSERT Reports ON;
INSERT INTO Reports (ReportId, ReportText, CreatedDateTime, ShownUp, SuggestPenalty) VALUES (@ReportId, 'Created through seeding the database', GETDATE(), 1, 0)
SET IDENTITY_INSERT Reports OFF;

SET IDENTITY_INSERT Reservations ON;

--Boat reservations
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (8, 5, 2, DATEADD(day, -30, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -20, GETDATE()), DATEADD(day, -17, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (9, 5, 2, DATEADD(day, -29, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -15, GETDATE()), DATEADD(day, -10, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (10, 5, 2, DATEADD(day, -28, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -3, GETDATE()), DATEADD(day, 5, GETDATE()))

SET IDENTITY_INSERT Reservations OFF;

--Reservations with quickactions
SET @StartDate = DATEADD(day, 13, GETDATE())
SET @EndDate = DATEADD(day, 16, GETDATE())
SET IDENTITY_INSERT Reservations ON;
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (11, 5, 2, DATEADD(day, -3, GETDATE()), 1, 0, 0, null, null,'', 12.22, @StartDate, @EndDate)
SET IDENTITY_INSERT Reservations OFF;
SET IDENTITY_INSERT PromoActions ON;
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (4, 2, @StartDate, @EndDate, 12.22, 1, 10, null)
SET IDENTITY_INSERT PromoActions OFF;

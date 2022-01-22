SET IDENTITY_INSERT Reservations ON;

INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (0, 4, 0, '2021-12-02 15:43:38.457', 0, 0, 0, null, null,'', 12.22, '2022-10-02 15:43:38.457', '2022-12-02 15:43:38.457')
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, 0, DATEADD(day, -3, GETDATE()), 0, 0, 1, null, null,'', 12.22, DATEADD(day, 8, GETDATE()), DATEADD(day, 12, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (2, 4, 0, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -10, GETDATE()), DATEADD(day, -7, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (3, 4, 0, DATEADD(day, -20, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -13, GETDATE()), DATEADD(day, -11, GETDATE()))

INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (4, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -1, GETDATE()), DATEADD(hour, 2, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (5, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -3, GETDATE()), DATEADD(hour, -2, GETDATE()))
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

SET IDENTITY_INSERT BoatReservationDetails ON;

INSERT INTO BoatReservationDetails (Id, BoatOwnerResponsibilityType, RelevantId, IsPromo) values (0, 0, 11, 0)

SET IDENTITY_INSERT BoatReservationDetails OFF;

SET IDENTITY_INSERT Reservations ON; 

INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (12, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -27, GETDATE()), DATEADD(day, -22, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (13, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -34, GETDATE()), DATEADD(day, -29, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (14, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -44, GETDATE()), DATEADD(day, -37, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (15, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -60, GETDATE()), DATEADD(day, -47, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (16, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -65, GETDATE()), DATEADD(day, -61, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (17, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -72, GETDATE()), DATEADD(day, -68, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (18, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -79, GETDATE()), DATEADD(day, -74, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (19, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -83, GETDATE()), DATEADD(day, -81, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (20, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -89, GETDATE()), DATEADD(day, -85, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (21, 5, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -93, GETDATE()), DATEADD(day, -90, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (22, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -100, GETDATE()), DATEADD(day, -95, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (23, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -105, GETDATE()), DATEADD(day, -101, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (24, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -109, GETDATE()), DATEADD(day, -106, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (25, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -115, GETDATE()), DATEADD(day, -110, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (26, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -126, GETDATE()), DATEADD(day, -119, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (27, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -136, GETDATE()), DATEADD(day, -129, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (28, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -139, GETDATE()), DATEADD(day, -137, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (29, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -144, GETDATE()), DATEADD(day, -141, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (30, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -149, GETDATE()), DATEADD(day, -145, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (31, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -151, GETDATE()), DATEADD(day, -150, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (32, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -156, GETDATE()), DATEADD(day, -152, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (33, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -160, GETDATE()), DATEADD(day, -158, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (34, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -164, GETDATE()), DATEADD(day, -162, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (35, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -169, GETDATE()), DATEADD(day, -165, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (36, 5, 2, DATEADD(day, -300, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -186, GETDATE()), DATEADD(day, -180, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (37, 5, 2, DATEADD(day, -200, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -196, GETDATE()), DATEADD(day, -189, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (38, 5, 2, DATEADD(day, -300, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -202, GETDATE()), DATEADD(day, -197, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (39, 5, 2, DATEADD(day, -300, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -226, GETDATE()), DATEADD(day, -220, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (40, 5, 2, DATEADD(day, -400, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -396, GETDATE()), DATEADD(day, -390, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (41, 5, 2, DATEADD(day, -900, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -810, GETDATE()), DATEADD(day, -800, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (42, 5, 2, DATEADD(day, -900, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(day, -819, GETDATE()), DATEADD(day, -811, GETDATE()))

--instructor's
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (43, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -100, GETDATE()), DATEADD(hour, -98, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (44, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -96, GETDATE()), DATEADD(hour, -95, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (45, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -90, GETDATE()), DATEADD(hour, -88, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (46, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, -80, GETDATE()), DATEADD(hour, -75, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (47, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, 100, GETDATE()), DATEADD(hour, 103, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (48, 11, 1, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'', 12.22, DATEADD(hour, 110, GETDATE()), DATEADD(hour, 112, GETDATE()))

-- registered history
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (49, 8, 1, DATEADD(day, -100, GETDATE()), 0, 0, 1, null, null,'fruit:0;fine wine:0;desert:0', 35.22, DATEADD(day, -110, GETDATE()), DATEADD(day, -105, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (50, 8, 2, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'mango:0;fine whiskey:0;cake:0', 45.22, DATEADD(day, -120, GETDATE()), DATEADD(day, -115, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (51, 8, 2, DATEADD(day, -100, GETDATE()), 1, 0, 0, null, null,'fruit:0;papaya:0;pie:0', 15.22, DATEADD(day, -130, GETDATE()), DATEADD(day, -125, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (52, 8, 4, DATEADD(day, -100, GETDATE()), 0, 0, 0, null, null,'coconuts:0;fine wine:0;whiskey:0', 29, DATEADD(day, -140, GETDATE()), DATEADD(day, -135, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (53, 8, 4, DATEADD(day, -100, GETDATE()), 1, 0, 0, null, null,'fine wine:0;fine wine:0;chocolate:0', 17.22, DATEADD(day, -150, GETDATE()), DATEADD(day, -145, GETDATE()))

-- registered future - hardcoding final price per day - not calculating
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (54, 8, 1, DATEADD(day, 0, GETDATE()), 0, 0, 1, null, null,'fruit:0;fine wine:0;desert:0', 25, DATEADD(day, 110, GETDATE()), DATEADD(day, 115, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (55, 8, 2, DATEADD(day, 0, GETDATE()), 0, 0, 0, null, null,'mango:0;fine whiskey:0;cake:0', 26.22, DATEADD(day, 120, GETDATE()), DATEADD(day, 125, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (56, 8, 2, DATEADD(day, 0, GETDATE()), 0, 0, 0, null, null,'fruit:0;papaya:0;pie:0', 42.22, DATEADD(day, 130, GETDATE()), DATEADD(day, 135, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (57, 8, 4, DATEADD(day, 0, GETDATE()), 0, 0, 0, null, null,'coconuts:0;fine wine:0;whiskey:0', 32.22, DATEADD(day, 140, GETDATE()), DATEADD(day, 145, GETDATE()))
INSERT INTO Reservations (ReservationId, UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (58, 8, 4, DATEADD(day, 0, GETDATE()), 0, 0, 0, null, null,'fine wine:0;fine wine:0;chocolate:0', 80.22, DATEADD(day, 150, GETDATE()), DATEADD(day, 155, GETDATE()))


SET IDENTITY_INSERT Reservations OFF;

﻿INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, '2021-12-02 15:43:38.457', 0, 0, 0, null, null,'', 12.22, '2022-10-02 15:43:38.457', '2022-12-02 15:43:38.457')
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (2, 1, DATEADD(day, -3, GETDATE()), 0, 0, 1, null, null,'', 12.22, DATEADD(day, 8, GETDATE()), DATEADD(day, 12, GETDATE()))

--Reservations with quickactions
DECLARE @StartDate AS DATETIME2 = DATEADD(day, 3, GETDATE())
DECLARE @EndDate AS DATETIME2 = DATEADD(day, 6, GETDATE())
INSERT INTO Reservations (UserId, ServiceId, ReservedDateTime, IsPromo, IsCanceled, IsServiceUnavailableMarker, ReportId, MarkId, AdditionalEquipment, Price, StartDateTime, EndDateTime) VALUES (1, 1, DATEADD(day, -3, GETDATE()), 1, 0, 0, null, null,'', 12.22, @StartDate, @EndDate)
INSERT INTO PromoActions (ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (1, @StartDate, @EndDate, 12.22, 1, 10, null)
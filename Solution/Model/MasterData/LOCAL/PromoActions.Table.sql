SET IDENTITY_INSERT PromoActions ON;

INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (0, 0, DATEADD(day, 30,GETDATE()), DATEADD(day, 33, GETDATE()), 12.22, 0, 10, null)
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (1, 0, DATEADD(day, 36,GETDATE()), DATEADD(day, 40, GETDATE()), 12.22, 0, 10, null)
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (2, 0, DATEADD(day, 42,GETDATE()), DATEADD(day, 47, GETDATE()), 12.22, 0, 10, null)

SET IDENTITY_INSERT PromoActions OFF;

SET IDENTITY_INSERT PromoActions ON;

INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (0, 0, DATEADD(day, 30,GETDATE()), DATEADD(day, 33, GETDATE()), 12.22, 0, 10, 'whiskey:0;fruits:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (1, 0, DATEADD(day, 36,GETDATE()), DATEADD(day, 40, GETDATE()), 12.22, 0, 10, 'sound system:0;wine:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (2, 0, DATEADD(day, 42,GETDATE()), DATEADD(day, 47, GETDATE()), 12.22, 0, 10, 'fruits:0;wine:0;')

-- Seeds with ids 3, 4, 5 are added through Reservations.Table.sql
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (6, 2, DATEADD(day, 60,GETDATE()), DATEADD(day, 65, GETDATE()), 10.00, 0, 10, 'whiskey:0;fruits:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (7, 2, DATEADD(day, 66,GETDATE()), DATEADD(day, 71, GETDATE()), 15.22, 0, 10, 'fruits:0;wine:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (8, 2, DATEADD(day, 80,GETDATE()), DATEADD(day, 85, GETDATE()), 13.22, 0, 10, 'dragon fruit:0;white wine:0')

-- instructor's
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (9, 1, DATEADD(hour, 150, GETDATE()), DATEADD(hour, 153, GETDATE()), 13.22, 0, 10, 'dragon fruit:0;white wine:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (10, 1, DATEADD(hour, 160, GETDATE()), DATEADD(hour, 163, GETDATE()), 13.22, 0, 10, 'dragon fruit:0;white wine:0')
INSERT INTO PromoActions (PromoActionId, ServiceId, StartDateTime, EndDateTime, PricePerDay, IsTaken, Capacity, AddedBenefits) VALUES (11, 1, DATEADD(hour, 170, GETDATE()), DATEADD(hour, 173, GETDATE()), 13.22, 0, 10, 'dragon fruit:0;white wine:0')



SET IDENTITY_INSERT PromoActions OFF;

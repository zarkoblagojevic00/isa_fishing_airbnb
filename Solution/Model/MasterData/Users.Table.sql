DELETE Users

DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 1)

--ADMINS
INSERT INTO Users VALUES (4, 'Nikola', 'Milosavljević', 'dzontra volta', 'neka adresa 123', 38, '0655026516', 'nikolamilosa20@gmail.com', 1, 1)
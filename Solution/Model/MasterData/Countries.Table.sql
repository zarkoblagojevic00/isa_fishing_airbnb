DELETE Countries

DBCC CHECKIDENT ('[dbo].[Countries]', RESEED, 1)

INSERT INTO Countries VALUES ('Serbia')
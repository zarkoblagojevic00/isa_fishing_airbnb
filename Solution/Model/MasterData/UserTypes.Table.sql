﻿DELETE UserTypes

-- DBCC CHECKIDENT ('[dbo].[<table>]', RESEED, 0)

INSERT INTO UserTypes VALUES (0, 'Registered')
INSERT INTO UserTypes VALUES (1, 'VillaOwner')
INSERT INTO UserTypes VALUES (2, 'BoatOwner')
INSERT INTO UserTypes VALUES (3, 'Instructor')
INSERT INTO UserTypes VALUES (4, 'Admin')
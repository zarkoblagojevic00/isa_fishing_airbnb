SET IDENTITY_INSERT Users ON;

INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (0, 1, 'Nikola', 'Milosavljević', 'dzontra volta', 'neka adresa 123', 138, '0655026516', 'nikolamilosa20@gmail.com', 1, 1)

INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (1, 1, 'VillaOwner', 'VillaOwner', 'vilaowner123' , 'neka adresa 123', 138, '0623245651', 'testvillaowner@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (2, 1, 'AnotherVillaOwner', 'AnotherVillaOwner', 'vilaowner123' , 'neka adresa 123', 138, '0623245651', 'anothertestvillaowner@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (3, 3, 'Stefan', 'Ljubovic', 'ljubaljuba', 'Bulevar vecnih ratnika 100', 138, '066103130', 'stef@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (4, 0, 'Basic', 'OnlyRegistered', 'password123','guy for testing 123', 138, '0623245651', 'testingguy@gmail.com', 1, 1)
	INSERT INTO FurtherClientInfo (UserId, CollectedPoints, NumberOfPenalties) VALUES (4, 0, 0)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (5, 0, 'Klijento', 'Klica', 'klijo', 'Bulevar vecnih ratnika 110', 138, '066103131', 'ljubo@gmail.com', 1, 1)
	INSERT INTO FurtherClientInfo (UserId, CollectedPoints, NumberOfPenalties) VALUES (5, 0, 0)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (6, 2, 'Brodisa', 'Vlasnikovic', 'brodisa123', 'Bulevar vecnih ratnika 140', 138, '066103132', 'ljubovicstefan@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (7, 4, 'Admin', 'Adminovic', 'adminadmin', 'Bulevar vecnih ratnika 140', 138, '066103132', 'srdjansukovic@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (8, 0, 'Žarko', 'Blagojević', 'strongmf', 'Bulevar vecnih ratnika 150', 138, '0661115555', 'zarexblage00@gmail.com', 1, 1)

SET IDENTITY_INSERT Users OFF;


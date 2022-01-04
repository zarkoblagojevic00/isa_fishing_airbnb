SET IDENTITY_INSERT Users ON;

INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (0, 1, 'Nikola', 'Milosavljević', 'dzontra volta', 'neka adresa 123', 38, '0655026516', 'nikolamilosa20@gmail.com', 1, 1)

INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (1, 1, 'VillaOwner', 'VillaOwner', 'vilaowner123' , 'neka adresa 123', 38, '0623245651', 'testvillaowner@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (2, 1, 'AnotherVillaOwner', 'AnotherVillaOwner', 'vilaowner123' , 'neka adresa 123', 38, '0623245651', 'anothertestvillaowner@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (3, 3, 'Stefan', 'Ljubovic', 'ljubaljuba', 'Bulevar vecnih ratnika 100', 38, '066103130', 'stef@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (4, 0, 'Basic', 'OnlyRegistered', 'password123','guy for testing 123', 38, '0623245651', 'testingguy@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (5, 0, 'Klijento', 'Klica', 'klijo', 'Bulevar vecnih ratnika 110', 38, '066103131', 'ljubo@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (6, 2, 'Brodisa', 'Vlasnikovic', 'brodisa', 'Bulevar vecnih ratnika 140', 38, '066103132', 'ljubovicstefan@gmail.com', 1, 1)
INSERT INTO Users (UserId, UserType, Name, Surname, Password, Address, CityId, PhoneNumber, Email, IsAccountVerified, IsAccountActive) VALUES (7, 4, 'Admin', 'Adminovic', 'adminadmin', 'Bulevar vecnih ratnika 140', 38, '066103132', 'srdjansukovic@gmail.com', 1, 1)

SET IDENTITY_INSERT Users OFF;


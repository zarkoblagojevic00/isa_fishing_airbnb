CREATE TABLE [dbo].[Users]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [UserType] INT NULL, 
    [Name] NVARCHAR(MAX) NULL, 
    [Surname] NVARCHAR(MAX) NULL, 
    [Password] NVARCHAR(MAX) NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    [CityId] INT NULL, 
    [PhoneNumber] NVARCHAR(MAX) NULL, 
    [Email] NVARCHAR(MAX) NULL, 
    [IsAccountVerified] BIT NULL, 
    [IsAccountActive] BIT NULL
)

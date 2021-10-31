CREATE TABLE [dbo].[Images]
(
	[ImageId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Bytes] VARBINARY(MAX) NULL, 
    [ServiceId] INT NULL
)

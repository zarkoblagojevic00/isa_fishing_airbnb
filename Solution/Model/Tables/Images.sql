CREATE TABLE [dbo].[Images]
(
	[ImageID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Bytes] VARBINARY(MAX) NULL, 
    [ServiceId] INT NULL
)

CREATE TABLE [dbo].[AdditionalInstructorInfos]
(
	[UserId] INT NOT NULL PRIMARY KEY, 
    [AvailableFrom] DATETIME2 NULL, 
    [AvailableTo] DATETIME2 NULL, 
    [ShortBiography] NVARCHAR(MAX) NULL
)

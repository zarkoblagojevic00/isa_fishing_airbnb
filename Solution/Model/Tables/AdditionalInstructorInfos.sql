CREATE TABLE [dbo].[AdditionalInstructorInfos]
(
	[InstructorId] INT NOT NULL PRIMARY KEY, 
    [AvailableFrom] DATETIME2 NULL, 
    [AvailableTo] DATETIME2 NULL, 
    [ShortBiography] NVARCHAR(MAX) NULL
)

CREATE TABLE [dbo].[Reports]
(
	[ReportId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReportText] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIME2 NULL
)

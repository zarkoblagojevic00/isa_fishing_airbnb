CREATE TABLE [dbo].[Reports]
(
	[ReportID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReportText] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIME2 NULL
)

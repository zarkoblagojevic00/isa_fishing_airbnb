CREATE TABLE [dbo].[Reports]
(
	[ReportId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReportText] NVARCHAR(MAX) NULL, 
    [CreatedDateTime] DATETIME2 NULL, 
    [ShownUp] BIT NULL, 
    [SuggestPenalty] BIT NULL
)

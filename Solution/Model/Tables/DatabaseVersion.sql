CREATE TABLE [dbo].[DatabaseVersion]
(
	[DeploymentDate] DATETIME2 NOT NULL PRIMARY KEY, 
    [DeploymentUser] NVARCHAR(50) NULL, 
    [DeploymentProfile] NVARCHAR(50) NULL
)

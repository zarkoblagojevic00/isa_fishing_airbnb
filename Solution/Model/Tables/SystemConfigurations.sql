CREATE TABLE [dbo].[SystemConfigurations]
(
	[Name] NVARCHAR(MAX) NULL, 
    [Value] NVARCHAR(MAX) NULL, 
    [Id] INT NOT NULL IDENTITY, 
    CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY ([Id]) 
)

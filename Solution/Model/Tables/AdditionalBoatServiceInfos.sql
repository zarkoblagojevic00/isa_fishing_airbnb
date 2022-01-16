CREATE TABLE [dbo].[AdditionalBoatServiceInfos]
(
	[ServiceId] INT NOT NULL, 
    [Length] DECIMAL(18, 3) NULL, 
    [BoatType] INT NULL, 
    [NumberOfEngines] INT NULL, 
    [PowerOfEngines] DECIMAL(18, 3) NULL, 
    [MaxSpeed] DECIMAL(18, 3) NULL, 
    CONSTRAINT [PK_AdditionalBoatServiceInfos] PRIMARY KEY ([ServiceId]) 
)

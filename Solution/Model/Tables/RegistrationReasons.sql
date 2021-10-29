CREATE TABLE [dbo].[RegistrationReasons]
(
	[UserId] INT NOT NULL, 
    [Reason] NVARCHAR(MAX) NULL, 
    [DenialReason] NVARCHAR(MAX) NULL, 
    [IsReviewed] BIT NULL 
)

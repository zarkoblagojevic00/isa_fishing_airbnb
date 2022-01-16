CREATE TABLE [dbo].[UserVerificationKeys]
(
    [UserId] INT NOT NULL, 
    [VerificationKey] NVARCHAR(MAX) NULL, 
    [IsUsed] BIT NULL, 
    PRIMARY KEY ([UserId])
)

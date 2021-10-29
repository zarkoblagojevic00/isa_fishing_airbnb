DECLARE @deploymentDate DATETIME2
DECLARE @deploymentUser NVARCHAR(50)
DECLARE @deploymentProfile NVARCHAR(50)

SET @deploymentDate = GETDATE()
SET @deploymentUser = suser_name()
SET @deploymentProfile = '$(Environment)'

INSERT [dbo].[DatabaseVersion] ([DeploymentDate], [DeploymentUser], [DeploymentProfile]) 
	VALUES (@deploymentDate, @deploymentUser, @deploymentProfile)

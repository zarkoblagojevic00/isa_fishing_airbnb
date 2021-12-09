DELETE AccountDeletionRequests
DELETE AdditionalBoatServiceInfos
DELETE AdditionalVillaServiceInfos
DELETE AdditionalAdventureInfos
DELETE AdditionalInstructorInfos
DELETE BoatReservationDetails
DELETE BoatServiceNavigationTools
DELETE Cities
DELETE Countries
DELETE FurtherClientInfo
DELETE Images
DELETE Issues
DELETE LegacyCategories
DELETE Marks
DELETE PromoActions
DELETE RegistrationReasons
DELETE Reports
DELETE Reservations
DELETE Services
DELETE Subscriptions
DELETE SystemConfigurations
DELETE Users
DELETE UserVerificationKeys

DELETE ServiceTypes
DELETE BoatTypes
DELETE UserTypes

DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Cities]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Countries]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Images]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Issues]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[LegacyCategories]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Marks]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[PromoActions]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Reports]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Reservations]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[Services]', RESEED, 1)
DBCC CHECKIDENT ('[dbo].[SystemConfigurations]', RESEED, 1)
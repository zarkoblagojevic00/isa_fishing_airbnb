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
DELETE LinkNavigationBoat
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
DELETE BoatReservationDetails

DELETE ServiceTypes
DELETE BoatTypes
DELETE UserTypes

DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Cities]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Countries]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Images]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Issues]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[LegacyCategories]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[LinkNavigationBoat]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Marks]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[PromoActions]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Reports]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Reservations]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[Services]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[SystemConfigurations]', RESEED, 100)
DBCC CHECKIDENT ('[dbo].[BoatReservationDetails]', RESEED, 100)
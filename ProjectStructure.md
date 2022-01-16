# Dogovorena struktura projekta


```
ISA.sln
	Database
		Model.sql
			Tables - lista tabela
			Publish - publish profili
			MasterData - skripte sa konkretnim vrednostima
		Domain - dll 
			Entities
			Mappings
			Repositories
			UnitOfWork
	Service
		Services - dll
	Web
		API
			Controllers
			wwwroot
				css
				js 
				html
			Startup.cs
			Program.cs
			appsettings.json >
				appsettings.Debug.json
				appsettings.Production.json
				appsettings.Staging.json
	Tests
		Unit
			- xUnit
		Integration
			- Specflow - skinuti kao extension kroz VS

```
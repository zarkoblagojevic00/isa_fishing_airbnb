Feature: CityActions
	
Scenario: Get one city
	Given a city with the name "Novi Sad" is found in the database
	When a request is sent to the API
	| Field      | Value                    |
	| HttpMethod | get                      |
	| BaseUrl    | https://localhost:44383/ |
	Then a "200" status code should be received with data
	| Key       | Value    |
	| Name      | Novi Sad |
	| CountryId | 1        |
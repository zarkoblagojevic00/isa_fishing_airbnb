Feature: CityActions
	
Scenario: Get one city
	Given a city with the name "Novi Sad" is found in the database
	When a request is sent to the API
	| Field               | Value             |
	| HttpMethod          | get               |
	| RelativeResourceUrl | /api/City/GetCity |
	Then a "200" status code should be received
	And the response will come with following json object
	| Key       | Value    |
	| name      | Novi Sad |
	| countryId | 1        |

Scenario: Get all cities
	When a request is sent to the API
	| Field               | Value               |
	| HttpMethod          | get                 |
	| RelativeResourceUrl | /api/City/GetCities |
	
	Then a "200" status code should be received
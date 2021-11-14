Feature: VillaManagement

Scenario: Create New Villa
	Given there was no villa with the name "Test Villa" linked with test villa owner
	And a new villa DTO was created with the following properties
	| Filed            | Value            |
	| Name             | Test Villa       |
	| PricePerDay      | 10.00            |
	| Address          | Some Address 123 |
	| Longitude        | 0                |
	| Lattitude        | 0                |
	| PromoDescription | Test description |
	| TermsOfUse       | some terms       |
	| Capacitiy        | 5                |
	| NumberOfBeds     | 4                |
	| NumberOfRooms    | 2                |

	When a request is sent to the API
	| Field               | Value                        |
	| HttpMethod          | post                         |
	| RelativeResourceUrl | /VillaManagement/CreateVilla |
	| CookieUserId        | 2                            |
	| CookieEmail         | testvillaowner@gmail.com     |
	
	Then a "200" status code should be received
	And a villa with the name "Test Villa" will be created and will be owned by "testvillaowner@gmail.com"
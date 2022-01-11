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
	| CityName         | Novi Sad         |

	When a request is sent to the API
	| Field               | Value                            |
	| HttpMethod          | post                             |
	| RelativeResourceUrl | /api/VillaManagement/CreateVilla |
	| CookieUserId        | 2                                |
	| CookieEmail         | testvillaowner@gmail.com         |
	
	Then a "200" status code should be received
	And a villa with the name "Test Villa" will be created and will be owned by "testvillaowner@gmail.com"

Scenario: Delete Existing Villa
	Given there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And the villa with the name "Test Villa" that is linked with test villa owner had no reservations
	And an id of the villa named "Test Villa" which is linked to the test owner is included as path parameter
	
	When a request is sent to the API
	| Field               | Value                            |
	| HttpMethod          | delete                           |
	| RelativeResourceUrl | /api/VillaManagement/DeleteVilla |
	| CookieEmail         | testvillaowner@gmail.com         |
	
	Then a "200" status code should be received
	And the villa with the name "Test Villa" will be deleted for the owner "testvillaowner@gmail.com"

Scenario: Villa Update With existing Reservations
	Given there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And a new villa DTO was formed based on the villa from the database that is linked with test owner
	| Filed            | Value              |
	| Name             | Test Villa         |
	| PricePerDay      | 20.00              |
	| Address          | Some Address 123   |
	| Longitude        | 0                  |
	| Lattitude        | 0                  |
	| PromoDescription | Test description   |
	| TermsOfUse       | some changed terms |
	| Capacitiy        | 5                  |
	| NumberOfBeds     | 4                  |
	| NumberOfRooms    | 2                  |

	When a request is sent to the API
	| Field               | Value                            |
	| HttpMethod          | put                              |
	| RelativeResourceUrl | /api/VillaManagement/UpdateVilla |

	Then a "401" status code should be received
	
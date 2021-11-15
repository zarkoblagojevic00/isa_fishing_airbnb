Feature: QuickActions

Scenario: Create new Promo action
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a promo action for the service "Test Villa" lasting from "1" days from now until "3" days from now
	And there was a reservation for the service "Test Villa" lasting from "4" days from now until "6" days from now
	And a QuickActionParameter was created for the villa "Test Villa" with the following properties
	| Field               | Value |
	| BeginDaysAfterToday | 7     |
	| EndDaysAfterToday   | 9     |
	| PricePerDay         | 5     |
	| Capacity            | 3     |

	When a request is sent to the API
	| Field               | Value                             |
	| HttpMethod          | post                              |
	| RelativeResourceUrl | /QuickAction/CreateNewQuickAction |
	| CookieEmail         | testvillaowner@gmail.com          |

	Then a "200" status code should be received
	And service with the name "Test Villa" should have the promo action with the properties from current content

Scenario: Update existing Promo action that wasn't taken
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a promo action for the service "Test Villa" lasting from "1" days from now until "3" days from now
	And there was a reservation for the service "Test Villa" lasting from "4" days from now until "6" days from now
	And a QuickActionParameter was created for the villa "Test Villa" with the following properties
	| Field               | Value |
	| BeginDaysAfterToday | 1     |
	| EndDaysAfterToday   | 2     |
	| PricePerDay         | 5     |
	| Capacity            | 3     |
	And that QuickActionDTO had the PromoActionId set previously

	When a request is sent to the API
	| Field               | Value                          |
	| HttpMethod          | put                            |
	| RelativeResourceUrl | /QuickAction/UpdateQuickAction |
	| CookieEmail         | testvillaowner@gmail.com       |

	Then a "200" status code should be received
	And service with the name "Test Villa" should have the promo action with the properties from current content

Scenario: Update existin Promo action that is taken
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a promo action for the service "Test Villa" lasting from "1" days from now until "3" days from now
	And previously created promo action was taken
	And a QuickActionParameter was created for the villa "Test Villa" with the following properties
	| Field               | Value |
	| BeginDaysAfterToday | 1     |
	| EndDaysAfterToday   | 2     |
	| PricePerDay         | 5     |
	| Capacity            | 3     |
	And that QuickActionDTO had the PromoActionId set previously

	When a request is sent to the API
	| Field               | Value                          |
	| HttpMethod          | put                            |
	| RelativeResourceUrl | /QuickAction/UpdateQuickAction |
	| CookieEmail         | testvillaowner@gmail.com       |

	Then a "400" status code should be received
	
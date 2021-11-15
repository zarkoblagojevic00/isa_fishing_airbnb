Feature: GeneralService

Scenario: Submitting a report
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a reservation for the service "Test Villa" lasting from "-6" days from now until "-3" days from now
	And the ReportDTO was created and linked to previously created reservation as content with following properties
	| Field      | Value                             |
	| ReportText | testing submitting report feature |

	When a request is sent to the API
	| Field               | Value                        |
	| HttpMethod          | post                         |
	| RelativeResourceUrl | /GeneralService/SubmitReport |
	| CookieEmail         | testvillaowner@gmail.com     |

	Then a "200" status code should be received
	Then a new report will be created for the service with name "Test Villa" in "60" seconds

Scenario: Creating a reservation for user who has an overlapping reservation
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a normal user in the database with the following information
	| Field    | Value                  |
	| Email    | testnormuser@gmail.com |
	| Name     | TestUser               |
	| Surname  | TestUser               |
	| Password | tester                 |
	| Address  | test adr               |
	And there were no reservations for previously created user
	And there was a reservation for previously created user lasting from "1" days from now until "5" days from now
	And there was a reservation for previously created user lasting from "6" days from now until "9" days from now
	And a NewReservationParameter was created as content for the previously created user and service with name "Test Villa" with the following properties
	| Field               | Value |
	| BeginDaysAfterToday | 3     |
	| EndDaysAfterToday   | 6     |
	| PricePerDay         | 10    |
	
	When a request is sent to the API
	| Field               | Value                                    |
	| HttpMethod          | post                                     |
	| RelativeResourceUrl | /GeneralService/CreateReservationForUser |
	| CookieEmail         | testvillaowner@gmail.com                 |

	Then a "400" status code should be received

Scenario: Creating a reservation for user who has no overlapping reservations
	Given there was a test villa owner with mail "testvillaowner@gmail.com" in database
	And there was a villa in the database named "Test Villa" linked with villa owner with email "testvillaowner@gmail.com"
	And there were no quick actions nur reservations in past for the service "Test Villa"
	And there was a normal user in the database with the following information
	| Field    | Value                  |
	| Email    | testnormuser@gmail.com |
	| Name     | TestUser               |
	| Surname  | TestUser               |
	| Password | tester                 |
	| Address  | test adr               |
	And there were no reservations for previously created user
	And there was a reservation for previously created user lasting from "1" days from now until "5" days from now
	And there was a reservation for previously created user lasting from "6" days from now until "9" days from now
	And a NewReservationParameter was created as content for the previously created user and service with name "Test Villa" with the following properties
	| Field               | Value |
	| BeginDaysAfterToday | 10    |
	| EndDaysAfterToday   | 16    |
	| PricePerDay         | 10    |
	
	When a request is sent to the API
	| Field               | Value                                    |
	| HttpMethod          | post                                     |
	| RelativeResourceUrl | /GeneralService/CreateReservationForUser |
	| CookieEmail         | testvillaowner@gmail.com                 |

	Then a "200" status code should be received
	And there should be a new reservation based on sent content created for the previously created user and service with name "Test Villa"
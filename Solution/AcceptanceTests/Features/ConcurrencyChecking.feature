Feature: ConcurrencyChecking

@mytag
Scenario: Multiple requests received for creating reservation from the same villa owner
	Given there was a villa in the database named "Test villa 2" linked with villa owner with email "testvillaowner2@gmail.com"
	And there were no actions or reservations for the service named "Test villa 2" and linked to owner with mail "testvillaowner2@gmail.com"
	And there was a normal user in the database with the following information
		| Field    | Value                  |
		| Email    | testnormuser@gmail.com |
		| Name     | TestUser               |
		| Surname  | TestUser               |
		| Password | tester                 |
		| Address  | test adr               |
	And there were no reservations for previously created user
	When The user with mail "testvillaowner2@gmail.com" sends multiple requests for service with name "Test villa 2" at the same time with the following properties
		| UserMail               | Price | AdditionalEquipment | From | To |
		| testnormuser@gmail.com | 5     |                     | 1    | 5  |
		| testnormuser@gmail.com | 5     |                     | 1    | 5  |
		| testnormuser@gmail.com | 5     |                     | 1    | 5  |
		| testnormuser@gmail.com | 5     |                     | 1    | 5  |
		| testnormuser@gmail.com | 5     |                     | 1    | 5  |
	Then only one request should ok

Scenario: Multiple requests received for creating reservation and quick action for the same villa owner
	Given there was a villa in the database named "Test villa 2" linked with villa owner with email "testvillaowner2@gmail.com"
	And there were no actions or reservations for the service named "Test villa 2" and linked to owner with mail "testvillaowner2@gmail.com"
	And there was a normal user in the database with the following information
		| Field    | Value                  |
		| Email    | testnormuser@gmail.com |
		| Name     | TestUser               |
		| Surname  | TestUser               |
		| Password | tester                 |
		| Address  | test adr               |
	And there were no reservations for previously created user
	When The user with mail "testvillaowner2@gmail.com" sends multiple reservations and quick action requests for service with name "Test villa 2" at the same time with the following properties
		| UserMail               | Price | AdditionalEquipment | From | To | IsQuickAction |
		|                        | 5     | some benefits       | 3    | 5  | true          |
		| testnormuser@gmail.com | 5     |                     | 2    | 4  | false         |
		| testnormuser@gmail.com | 5     |                     | 2    | 4  | false         |
		|                        | 5     | some benefits       | 3    | 5  | true          |
		|                        | 5     | some benefits       | 3    | 5  | true          |
		| testnormuser@gmail.com | 5     |                     | 2    | 4  | false         |
		|                        | 5     | some benefits       | 3    | 5  | true          |
		|                        | 5     | some benefits       | 3    | 5  | true          |
		| testnormuser@gmail.com | 5     |                     | 2    | 4  | false         |
	Then only one request should ok
	
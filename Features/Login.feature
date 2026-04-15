@smoke @login
Feature: Login functionality

@smoke @login
Scenario: Valid Login
  Given I navigate to login page
  When I enter valid credentials
  Then I should see dashboard

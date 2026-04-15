@regression @orangehrm
Feature: OrangeHRM end to end smoke automation

Background:
  Given I am logged into OrangeHRM as an administrator

@smoke @docker
Scenario Outline: Verify primary OrangeHRM modules are accessible
  When I open the "<Module>" module
  Then the "<Module>" module should be displayed

Examples:
  | Module      |
  | Admin       |
  | PIM         |
  | Leave       |
  | Time        |
  | Recruitment |
  | My Info     |
  | Performance |
  | Dashboard   |
  | Directory   |
  | Maintenance |
  | Claim       |
  | Buzz        |

@regression @pim @docker
Scenario: Create, search, and delete an employee through PIM
  When I create a new employee in PIM
  Then the employee should be searchable in PIM
  When I delete the employee from PIM
  Then the employee should not appear in PIM search results

using System;
using System.Linq;
using OpenQA.Selenium;

namespace SeleniumCSharpBDD.Pages
{
    public class PimPage : BasePage
    {
        public PimPage(IWebDriver driver)
            : base(driver)
        {
        }

        private static By SaveButton => By.CssSelector("button[type='submit']");
        private static By SearchButton => By.XPath("//button[@type='submit' and normalize-space()='Search']");
        private static By ResetButton => By.XPath("//button[@type='button' and normalize-space()='Reset']");

        public EmployeeData CreateEmployee()
        {
            var suffix = DateTime.UtcNow.ToString("mmssfff");
            var employee = new EmployeeData(
                "E2E",
                $"Auto{suffix}",
                "User",
                $"9{suffix}");

            Driver.Navigate().GoToUrl($"{AppUrl}/web/index.php/pim/addEmployee");

            Type(By.Name("firstName"), employee.FirstName);
            Type(By.Name("middleName"), employee.MiddleName);
            Type(By.Name("lastName"), employee.LastName);
            Type(FieldByLabel("Employee Id"), employee.EmployeeId);
            Click(SaveButton);

            Wait.Until(driver =>
                driver.Url.Contains("viewPersonalDetails", StringComparison.OrdinalIgnoreCase));

            return employee;
        }

        public bool SearchEmployeeById(string employeeId)
        {
            Driver.Navigate().GoToUrl($"{AppUrl}/web/index.php/pim/viewEmployeeList");
            Type(FieldByLabel("Employee Id"), employeeId);
            Click(SearchButton);

            return Wait.Until(driver =>
                driver.FindElements(By.XPath($"//div[contains(@class,'oxd-table-card')][.//*[normalize-space()='{employeeId}']]")).Any() ||
                driver.PageSource.Contains("No Records Found", StringComparison.OrdinalIgnoreCase));
        }

        public bool IsEmployeePresent(string employeeId)
        {
            return Driver.FindElements(By.XPath($"//div[contains(@class,'oxd-table-card')][.//*[normalize-space()='{employeeId}']]")).Any();
        }

        public void DeleteEmployee(string employeeId)
        {
            var deleteButton = By.XPath(
                $"//div[contains(@class,'oxd-table-card')][.//*[normalize-space()='{employeeId}']]//button[.//i[contains(@class,'bi-trash')]]");

            Click(deleteButton);
            Click(By.XPath("//button[contains(.,'Yes, Delete')]"));
            IsTextVisible("Successfully Deleted");
        }

        private static By FieldByLabel(string label)
        {
            return By.XPath($"//label[normalize-space()='{label}']/ancestor::div[contains(@class,'oxd-input-group')]//input");
        }
    }

    public record EmployeeData(string FirstName, string MiddleName, string LastName, string EmployeeId);
}

using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SeleniumCSharpBDD.Utils
{
    public static class TestDataRegistry
    {
        private const string CreatedEmployeeIdsKey = "CreatedEmployeeIds";

        public static void AddEmployee(ScenarioContext context, string employeeId)
        {
            GetEmployeeIds(context).Add(employeeId);
        }

        public static IReadOnlyCollection<string> GetEmployees(ScenarioContext context)
        {
            return GetEmployeeIds(context).ToList();
        }

        public static void RemoveEmployee(ScenarioContext context, string employeeId)
        {
            GetEmployeeIds(context).Remove(employeeId);
        }

        private static List<string> GetEmployeeIds(ScenarioContext context)
        {
            if (!context.TryGetValue(CreatedEmployeeIdsKey, out List<string> employeeIds))
            {
                employeeIds = new List<string>();
                context[CreatedEmployeeIdsKey] = employeeIds;
            }

            return employeeIds;
        }
    }
}

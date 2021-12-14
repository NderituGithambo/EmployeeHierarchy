


using EmployeesHierarchy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EmployeeUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Employees employees;
        [TestInitialize]
        public void TestInitiliaze()
        {
           
            employees = new Employees("test1.csv");
        }

        /// <summary>
        /// Tests if the Employees are added to the graph
        /// </summary>
        [TestMethod()]
        public void AddTest()
        {

            Assert.IsTrue(employees.employeeList.Contains(new Employee
            { Name = "Employee2", ManagerName = "Employee1", Salary = 800 }));
            Assert.IsTrue(employees.employeeList.Contains(new Employee
            { Name = "Employee4", ManagerName = "Employee2", Salary = 500 }));
        }

        /// <summary>
        /// Tests if Employee have subordinates added
        /// Example is using Employee2 who has two subordinates
        /// </summary>
        [TestMethod]
        public void SubOrdinate_Not_NULL()
        {
            var subordinates =employees.GetSubordinates("Employee2");
            Assert.AreEqual(2, subordinates.Count);
        }

        /// <summary>
        /// As per the test data employee 5 has no subordinates
        /// </summary>
        [TestMethod]
        public void Employee5_has_No_SubOrdinates_Test()
        {
            var subordinates =employees.GetSubordinates("Employee5");
            Assert.AreEqual(0, subordinates.Count);
        }

        /// <summary>
        /// Tests if the Lookup function returns a Employee given a valid Employee ID
        /// </summary>
        [TestMethod]
        public void LookUpTest()
        {
            Employee emp =employees.LookUp("Employee1");
            Assert.IsNotNull(emp);
        }

        /// <summary>
        /// Tests if lookup returns null on non existence id
        /// </summary>
        [TestMethod]
        public void Lookup_Wrong_emp_id_Test()
        {
            Employee emp =employees.LookUp("Employee10");
            Assert.IsNull(emp);
        }


        /// <summary>
        /// Tests if the correct budget is added  
        /// </summary>
        [TestMethod]
        public void GetBudgetTest()
        {
            Assert.AreEqual(1800,employees.getSalaries("Employee2"));
            Assert.AreEqual(500,employees.getSalaries("Employee3"));
            Assert.AreEqual(3800,employees.getSalaries("Employee1"));
        }

        /// <summary>
        /// Using test2.csv which contains employee with non number salary and negative salary
        /// Invalid Salary Employees are not added and the Graph is empty fails to pass this check
        /// </summary>
        [TestMethod]
        public void Test_Invalid_Salary_Not_Added()
        {
            Employees h2 = new Employees("test2.csv");
            Assert.IsFalse(h2.employeeList.Contains(new Employee
            { Name = "Employee5" }));
            Assert.IsFalse(h2.employeeList.Contains(new Employee
            { Name = "Employee2" }));

            Assert.AreEqual(0, h2.employeeList.Count);

        }
        /// <summary>
        /// Test3.csv contains two manager. The Graph should be Empty since manager should be one
        /// </summary>
        [TestMethod]
        public void Test_Manager_More_Than_Three()
        {
            Employees h = new Employees("test3.csv");
            Assert.IsFalse(h.employeeList.Contains(new Employee
            {Name = "Employee5" }));
            Assert.IsFalse(h.employeeList.Contains(new Employee
            { Name = "Employee1" }));
            Assert.AreEqual(0, h.employeeList.Count);

        }
        /// <summary>
        /// Test4.csv contains one employee with negative salary. 
        /// </summary>
        [TestMethod]
        public void Test_Negative_Salary_Check()
        {
            Employees h = new Employees("test4.csv");
            Assert.IsFalse(h.employeeList.Contains(new Employee
            { Name = "Employee5" }));
            Assert.AreEqual(0, h.employeeList.Count);
        }

        /// <summary>
        /// There is no manager that is not an employee, i.e. all managers are also listed in the employee column.
        /// test5.csv contain no manager record
        /// </summary>
        [TestMethod]
        public void No_Manager_Record()
        {
            Employees h = new Employees("test5.csv");
            Assert.AreEqual(0, h.employeeList.Count);
        }
    }
}

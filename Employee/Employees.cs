using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesHierarchy
{
    public class Employees
    {
        readonly Dictionary<string, List<string>> subordinateList= new Dictionary<string, List<string>>();
        public List<Employee> employeeList = new List<Employee>();

        public Employees(String csvPathFile)
        {
            if (!String.IsNullOrEmpty(csvPathFile) && File.Exists(csvPathFile))
            {
                ProcessData(csvPathFile);

                foreach (var emp in employeeList)
                {
                    Add(emp.ManagerName, emp.Name);
                }

            }
        }


        public void ProcessData(String path)
        {
            int totalManagers = 0;
            StreamReader reader = new StreamReader(File.OpenRead(path));

            while (!reader.EndOfStream)
            {
                try
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    //Create new Employee
                    var emp = new Employee();
                    emp.Name = values[0];
                    if (values[1].Equals(""))
                    {
                        emp.ManagerName = "";
                        //   totalceo++;
                        //Managers are more than one throws Exception
                        if (totalManagers > 1)
                        {
                            throw new Exception("More than one Managers found!");

                        }
                    }
                    else
                    {
                        emp.ManagerName = values[1];
                    }


                    long salary;
                    var isSalaryValid = Int64.TryParse(values[2], out salary);


                    if (isSalaryValid)
                    {

                        if (salary > 0)
                        {
                            emp.Salary = salary;
                        }
                        else
                        {
                            throw new Exception("Salary is not valid");
                        }

                    }
                    else
                    {
                        throw new Exception("Salary is not valid");
                    }

                    //add employee
                    employeeList.Add(emp);
                }
                catch (Exception ex)
                {
                    employeeList.Clear();
                    Console.WriteLine(ex.Message);
                    return;

                }
                if (totalManagers != 1)
                {
                    employeeList.Clear();
                    Console.WriteLine("Manager not found");
                }
            }
        }
        public void Add(string employeeName)
        {
            
            if (subordinateList.ContainsKey(employeeName))
            {
                return;
            }
            subordinateList.Add(employeeName, new List<string>());
        }
        /// <summary>
        /// Adds a  employee to a list of all junior staff reporting to the senior staff
        /// </summary>
        
        public void Add(string manager, string employeeName)
        {
            Add(manager);
            Add(employeeName);
            subordinateList[manager].Add(employeeName);
        }


        /// <summary>
        /// returns a list of all subordinates under Manager
        /// </summary>
        /// <param name="empName">Name of the Manager</param>
        /// <returns>List of all Junior Staffs</returns>
        public List<String> GetSubordinates(String empName)
        {
            return subordinateList[empName];
        }
        public long getSalaries(String root)
        {
            long salary = 0;
            HashSet<String> visited = new HashSet<String>();
            Stack<String> stack = new Stack<String>();
            stack.Push(root);
            while (stack.Count != 0)
            {
                String empName = stack.Pop();
                if (!visited.Contains(empName))
                {
                    visited.Add(empName);
                    foreach (String v in GetSubordinates(empName))
                    {
                        stack.Push(v);
                    }
                }
            }

            if (visited.Count == 0) return salary;
            foreach (var id in visited)
            {
                salary += LookUp(id).Salary;
            }

            return salary;
        }
        public Employee LookUp(string name)
        {
            Employee result ;

            result = (from e in employeeList
                      where e.Name == name
                      select e).First();

            return result; 
        }
    }
}

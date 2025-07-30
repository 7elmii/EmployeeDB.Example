using EmployeeDB;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDB
{

    using System;

    namespace EmployeeConsoleApp
    {
        class Program
        {
            static string connectionString = @"Server=WS-163;Database=EmployeeDB2;Trusted_Connection=True;";

            static void Main(string[] args)
            {
                var repo = new EmployeeRepository(connectionString);

                while (true)
                {
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1: Register a new employee");
                    Console.WriteLine("2: Update a current employee");
                    Console.WriteLine("3: Show top 3 highest salaries");
                    Console.WriteLine("4: Show average salary");
                    Console.WriteLine("5: View employee by ID");
                    Console.WriteLine("6: Remove employee by ID");
                    Console.WriteLine("7: Exit");
                    Console.Write("Choose an option (1-7): ");

                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            RegisterEmployee(repo);
                            break;
                        case "2":
                            UpdateEmployee(repo);
                            break;
                        case "3":
                            ShowTop3Salaries(repo);
                            break;
                        case "4":
                            ShowAverageSalary(repo);
                            break;
                        case "5":
                            ViewEmployee(repo);
                            break;
                        case "6":
                            RemoveEmployee(repo);
                            break;
                        case "7":
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please enter 1-7.");
                            break;
                    }



                }
            }

            static void RegisterEmployee(EmployeeRepository repo)
            {
                Console.Write("Enter employee name: ");
                string name = Console.ReadLine();
                Console.Write("Enter employee since (yyyy-mm-dd): ");
                string since = Console.ReadLine();
                Console.Write("Enter salary: ");
                string salaryInput = Console.ReadLine();
                Console.Write("Enter age: ");
                string ageInput = Console.ReadLine();

                if (!decimal.TryParse(salaryInput, out decimal salary) || !int.TryParse(ageInput, out int age) ||
                    !DateTime.TryParse(since, out DateTime employeeSince))
                {
                    Console.WriteLine("Invalid input. Registration failed.");
                    return;
                }

                Employee emp = new Employee
                {
                    EmployeeName = name,
                    EmployeeSince = employeeSince,
                    Salary = salary,
                    Age = age
                };
                repo.Register(emp);
                Console.WriteLine("Employee registered successfully!");
            }

            static void UpdateEmployee(EmployeeRepository repo)
            {
                Console.Write("Enter Employee ID to update: ");
                string idInput = Console.ReadLine();
                if (!int.TryParse(idInput, out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    return;
                }

                var emp = repo.GetEmployeeByID(id);
                if (emp == null)
                {
                    Console.WriteLine("No employee found with that ID.");
                    return;
                }

                Console.WriteLine("Leave blank to keep");
                Console.Write($"Enter new name (current: {emp.EmployeeName}): ");
                string name = Console.ReadLine();
                Console.Write($"Enter new salary (current: {emp.Salary}): ");
                string salaryInput = Console.ReadLine();
                Console.Write($"Enter new age (current: {emp.Age}): ");
                string ageInput = Console.ReadLine();

                emp.EmployeeName = string.IsNullOrWhiteSpace(name) ? emp.EmployeeName : name;
                if (decimal.TryParse(salaryInput, out decimal salary)) emp.Salary = salary;
                if (int.TryParse(ageInput, out int age)) emp.Age = age;

                if (repo.Update(emp))
                    Console.WriteLine("Employee updated!");
                else
                    Console.WriteLine("Update failed.");
            }

            static void ShowTop3Salaries(EmployeeRepository repo)
            {
                var top3 = repo.GetTop3Salaries();
                Console.WriteLine("\nTop 3 Salaries:");
                foreach (var emp in top3)
                {
                    Console.WriteLine(
                        $"{emp.EmployeeName}, Salary: {emp.Salary}, Age: {emp.Age}, Since: {emp.EmployeeSince.ToShortDateString()}");
                }
            }

            static void ShowAverageSalary(EmployeeRepository repo)
            {
                var avg = repo.GetAverageSalary();
                if (avg.HasValue)
                    Console.WriteLine($"Average Salary: {avg.Value:F2}");
                else
                    Console.WriteLine("No employees in database.");
            }
            static void RemoveEmployee(EmployeeRepository repo)
            {
                int id;
                while (true)
                {
                    Console.Write("Enter Employee ID to remove: ");
                    string idInput = Console.ReadLine();
                    if (int.TryParse(idInput, out id))
                        break;
                    Console.WriteLine("Invalid ID. Please enter a valid integer.");
                }

                if (repo.DeleteEmployee(id))
                {
                    Console.WriteLine("Employee removed successfully!");
                }
                else
                {
                    Console.WriteLine("No employee found with that ID.");
                }
            }

            static void ViewEmployee(EmployeeRepository repo)
            {
                Console.Write("Enter Employee ID: ");
                string idInput = Console.ReadLine();
                if (!int.TryParse(idInput, out int id))
                {
                    Console.WriteLine("Invalid ID.");
                    return;
                }

                var emp = repo.GetEmployeeByID(id);
                if (emp == null)
                {
                    Console.WriteLine("No employee found with that ID.");
                }
                else
                {
                    Console.WriteLine("===================");
                    Console.WriteLine("\nEmployee Info:");
                    Console.WriteLine($"ID: {emp.EmployeeId}");
                    Console.WriteLine($"Name: {emp.EmployeeName}");
                    Console.WriteLine($"Since: {emp.EmployeeSince.ToShortDateString()}");
                    Console.WriteLine($"Salary: {emp.Salary}");
                    Console.WriteLine($"Age: {emp.Age}");
                    Console.WriteLine("===================");
                }
            }

        }
    }
}

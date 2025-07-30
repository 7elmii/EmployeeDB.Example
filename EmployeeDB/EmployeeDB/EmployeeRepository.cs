using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeDB
{
    public class EmployeeRepository
    {
        private readonly string connectionString;

        public EmployeeRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Register(Employee emp)
        {
            string query = "INSERT INTO Employee (EmployeeName, EmployeeSince, Salary, Age) VALUES (@name, @since, @salary, @age)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@since", emp.EmployeeSince);
                cmd.Parameters.AddWithValue("@salary", emp.Salary);
                cmd.Parameters.AddWithValue("@age", emp.Age);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Update(Employee emp)
        {
            string query = "UPDATE Employee SET EmployeeName=@name, Salary=@salary, Age=@age WHERE EmployeeID=@id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@salary", emp.Salary);
                cmd.Parameters.AddWithValue("@age", emp.Age);
                cmd.Parameters.AddWithValue("@id", emp.EmployeeId);
                conn.Open();
                int affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

        public List<Employee> GetTop3Salaries()
        {
            List<Employee> list = new List<Employee>();
            string query = "SELECT TOP 3 * FROM Employee ORDER BY Salary DESC";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Employee
                        {
                            EmployeeId = (int)reader["EmployeeID"],
                            EmployeeName = reader["EmployeeName"].ToString(),
                            EmployeeSince = (DateTime)reader["EmployeeSince"],
                            Salary = (decimal)reader["Salary"],
                            Age = (int)reader["Age"]
                        });
                    }
                }
            }
            return list;
        }

        public decimal? GetAverageSalary()
        {
            string query = "SELECT AVG(Salary) FROM Employee";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                    return Convert.ToDecimal(result);
                else
                    return null;
            }
        }

        public Employee GetEmployeeByID(int id)
        {
            string query = "SELECT * FROM Employee WHERE EmployeeID=@id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee
                        {
                            EmployeeId = (int)reader["EmployeeID"],
                            EmployeeName = reader["EmployeeName"].ToString(),
                            EmployeeSince = (DateTime)reader["EmployeeSince"],
                            Salary = (decimal)reader["Salary"],
                            Age = (int)reader["Age"]
                        };
                    }
                }
            }
            return null;
        }
        public bool DeleteEmployee(int id)
        {
            string query = "DELETE FROM Employee WHERE EmployeeID = @id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                int affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

    }
}
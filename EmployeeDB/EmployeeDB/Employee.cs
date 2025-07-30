using System;

namespace EmployeeDB
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime EmployeeSince { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
    }
}
using System;

namespace Project.WebApi
{
    public class Employee
    {
        public Guid EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public double? Salary { get; set; }
    }
}
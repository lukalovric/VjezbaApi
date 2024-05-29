﻿using Microsoft.AspNetCore.Mvc;


namespace Project.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> _employees;

        static EmployeeController()
        {
         
            _employees = new List<Employee>
            {
                new Employee { EmployeeID = "1", FirstName = "Pero", LastName = "Perić", Position = "Manager", Salary = 40000 },
                new Employee { EmployeeID = "2", FirstName = "Jozo", LastName = "Jozić", Position = "Developer", Salary = 250000 },
                new Employee { EmployeeID = "3", FirstName = "Zozo", LastName = "Zozić", Position = "Designer", Salary = 20000 },
                new Employee { EmployeeID = "4", FirstName = "Momo", LastName = "Momić", Position = "Tester", Salary = 15000 },
                new Employee { EmployeeID = "5", FirstName = "Sara", LastName = "Sarić", Position = "HR", Salary = 17000 }
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            return Ok(_employees);
        }


        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(string id)
        {
            foreach (var employee in _employees)
            {
                if (employee.EmployeeID == id)
                {
                    return Ok(employee);
                }
            }
            return NotFound();
        }



        [HttpPut("{id}")]
        public IActionResult PutEmployee(string id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            Employee existingEmployee = null;
            foreach (var emp in _employees)
            {
                if (emp.EmployeeID == id)
                {
                    existingEmployee = emp;
                    break;
                }
            }

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Position = employee.Position;
            existingEmployee.Salary = employee.Salary;

            return NoContent();
        }


        [HttpPost]
        public ActionResult<Employee> PostEmployee(Employee employee)
        {
            _employees.Add(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeID }, employee);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(string id)
        {
            var employee = _employees.FirstOrDefault(e => e.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }
            _employees.Remove(employee);
            return NoContent();
        }

    }
}
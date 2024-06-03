using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Project.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly string _connectionString;
       

        public EmployeeController(IConfiguration configuration)
        {
            
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                var employees = new List<Employee>();
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT \"Id\", \"FirstName\", \"LastName\", \"Position\", \"Salary\" FROM \"Employee\"", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Position = reader.GetString(3),
                        Salary = reader.GetDouble(4)
                    });
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployee(Guid id)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT \"Id\", \"FirstName\", \"LastName\", \"Position\", \"Salary\" FROM \"Employee\" WHERE \"Id\" = @Id", conn);
                cmd.Parameters.AddWithValue("Id", id);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var employee = new Employee
                    {
                        Id = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Position = reader.GetString(3),
                        Salary = reader.GetDouble(4)
                    };
                    return Ok(employee);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("INSERT INTO \"Employee\" (\"Id\", \"FirstName\", \"LastName\", \"Position\", \"Salary\") VALUES (@Id, @FirstName, @LastName, @Position, @Salary)", conn);
                
                Guid Id = Guid.NewGuid();
                
                cmd.Parameters.AddWithValue("Id", Id);
                cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("LastName", employee.LastName);
                cmd.Parameters.AddWithValue("Position", employee.Position);
                cmd.Parameters.AddWithValue("Salary", employee.Salary);
                cmd.ExecuteNonQuery();
                return CreatedAtAction(nameof(GetEmployee), new { id = Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("UPDATE \"Employee\" SET \"FirstName\" = @FirstName, \"LastName\" = @LastName, \"Position\" = @Position, \"Salary\" = @Salary WHERE \"Id\" = @Id", conn);
                cmd.Parameters.AddWithValue("Id", employee.Id );
                cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("LastName", employee.LastName);
                cmd.Parameters.AddWithValue("Position", employee.Position);
                cmd.Parameters.AddWithValue("Salary", employee.Salary);
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                using var cmd = new NpgsqlCommand("DELETE FROM \"Employee\" WHERE \"Id\" = @Id", conn);
                cmd.Parameters.AddWithValue("Id", id);
                var rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

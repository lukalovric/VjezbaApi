namespace Project.WebApi
{
    public class Project
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid EmployeeId { get; set; }

    }
}

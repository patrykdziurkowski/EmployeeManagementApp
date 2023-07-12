namespace DataAccess.Models
{
#nullable disable
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string JobId { get; set; }
        public double? Salary { get; set; }
        public Single? CommissionPct { get; set; }
        public int? ManagerId { get; set; }
        public short? DepartmentId { get; set; }
    }
}
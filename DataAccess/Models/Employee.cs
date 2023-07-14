namespace DataAccess.Models
{
#nullable disable
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Employee
    {
#nullable disable
        public int EmployeeId { get; set; }
#nullable enable
        public string? FirstName { get; set; }
#nullable disable
        public string LastName { get; set; }
        public string Email { get; set; }
#nullable enable
        public string? PhoneNumber { get; set; }
#nullable disable  
        public DateTime HireDate { get; set; }
        public string JobId { get; set; }
#nullable enable
        public double? Salary { get; set; }
        public float? CommissionPct { get; set; }
        public int? ManagerId { get; set; }
        public short? DepartmentId { get; set; }
    }
}
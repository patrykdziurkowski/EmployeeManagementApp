namespace Domain
{
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Employee
    {
        public int? EMPLOYEE_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public DateTime? HIRE_DATE { get; set; }
        public string JOB_ID { get; set; }
        public double? SALARY { get; set; }
        public Single? COMMISSION_PCT { get; set; }
        public int? MANAGER_ID { get; set; }
        public Int16? DEPARTMENT_ID { get; set; }
    }
}
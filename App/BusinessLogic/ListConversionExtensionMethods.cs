using BusinessLogic.ViewModels;
using DataAccess.Models;

namespace BusinessLogic
{
    public static class ListConversionExtensionMethods
    {
        public static List<EmployeeDto> ToListOfEmployeeDto(this IEnumerable<Employee> employees)
        {
            List<EmployeeDto> result = new();

            foreach (Employee employee in employees)
            {
                EmployeeDto employeeDto = new(
                    employee.EmployeeId,
                    employee.LastName,
                    employee.Email,
                    employee.JobId)
                {
                    FirstName = employee.FirstName,
                    PhoneNumber = employee.PhoneNumber,
                    Salary = employee.Salary,
                    CommissionPct = employee.CommissionPct,
                    ManagerId = employee.ManagerId,
                    DepartmentId = employee.DepartmentId
                };
                result.Add(employeeDto);
            }

            return result;
        }

        public static List<DepartmentLocationDto> ToListOfDepartmentLocationDto(this IEnumerable<DepartmentLocation> departmentLocations)
        {
            List<DepartmentLocationDto> result = new();

            foreach (DepartmentLocation departmentLocation in departmentLocations)
            {
                DepartmentLocationDto departmentLocationDto = new(
                    departmentLocation.DepartmentId,
                    departmentLocation.DepartmentName,
                    departmentLocation.City)
                {
                    StateProvince = departmentLocation.StateProvince,
                    StreetAddress = departmentLocation.StreetAddress,
                    RegionName = departmentLocation.RegionName,
                    CountryName = departmentLocation.CountryName
                };
                result.Add(departmentLocationDto);
            }

            return result;
        }

        public static List<DepartmentDto> ToListOfDepartmentDto(this IEnumerable<Department> departments)
        {
            List<DepartmentDto> convertedDepartments = new();

            foreach (Department department in departments)
            {
                DepartmentDto departmentDto = new(
                    department.DepartmentId,
                    department.DepartmentName)
                {
                    ManagerId = department.ManagerId,
                    LocationId = department.LocationId
                };
                convertedDepartments.Add(departmentDto);
            }

            return convertedDepartments;
        }

        public static List<JobHistoryDto> ToListOfJobHistoryDto(this IEnumerable<JobHistory> jobHistories)
        {
            List<JobHistoryDto> result = new();

            foreach (JobHistory jobHistory in jobHistories)
            {
                JobHistoryDto jobHistoryDto = new(
                    jobHistory.EmployeeId,
                    jobHistory.JobId)
                {
                    StartDate = DateOnly.FromDateTime(jobHistory.StartDate),
                    EndDate = DateOnly.FromDateTime(jobHistory.EndDate),
                    DepartmentId = jobHistory.DepartmentId
                };
                result.Add(jobHistoryDto);
            }

            return result;
        }

        public static List<JobDto> ToListOfJobDto(this IEnumerable<Job> jobs)
        {
            List<JobDto> result = new();

            foreach (Job job in jobs)
            {
                JobDto jobDto = new(
                    job.JobId,
                    job.JobTitle)
                {
                    MaxSalary = job.MaxSalary,
                    MinSalary = job.MinSalary
                };
                result.Add(jobDto);
            }

            return result;
        }

        public static List<SalaryDto> ToListOfSalaryDto(this IEnumerable<Employee> salaries)
        {
            List<SalaryDto> result = new();

            foreach (Employee employee in salaries)
            {
                SalaryDto salaryDto = new(
                    employee.EmployeeId,
                    employee.LastName,
                    employee.JobId)
                {
                    FirstName = employee.FirstName,
                    Salary = employee.Salary,
                };
                result.Add(salaryDto);
            }

            return result;
        }
    }
}

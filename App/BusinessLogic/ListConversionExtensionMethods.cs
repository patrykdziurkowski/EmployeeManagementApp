using BusinessLogic.ViewModels;
using DataAccess.Models;

namespace BusinessLogic
{
    public static class ListConversionExtensionMethods
    {
        public static List<EmployeeDto> ToListOfEmployeeViewModel(this IEnumerable<Employee> employees)
        {
            List<EmployeeDto> result = new List<EmployeeDto>();

            foreach (Employee employee in employees)
            {
                EmployeeDto employeeViewModel = new()
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = DateOnly.FromDateTime(employee.HireDate),
                    JobId = employee.JobId,
                    Salary = employee.Salary,
                    CommissionPct = employee.CommissionPct,
                    ManagerId = employee.ManagerId,
                    DepartmentId = employee.DepartmentId
                };
                result.Add(employeeViewModel);
            }

            return result;
        }

        public static List<DepartmentLocationDto> ToListOfDepartmentLocationViewModel(this IEnumerable<DepartmentLocation> departmentLocations)
        {
            List<DepartmentLocationDto> result = new List<DepartmentLocationDto>();

            foreach (DepartmentLocation departmentLocation in departmentLocations)
            {
                DepartmentLocationDto departmentLocationViewModel = new()
                {
                    DepartmentId = departmentLocation.DepartmentId,
                    DepartmentName = departmentLocation.DepartmentName,
                    StateProvince = departmentLocation.StateProvince,
                    City = departmentLocation.City,
                    StreetAddress = departmentLocation.StreetAddress,
                    RegionName = departmentLocation.RegionName,
                    CountryName = departmentLocation.CountryName
                };
                result.Add(departmentLocationViewModel);
            }

            return result;
        }

        public static List<DepartmentDto> ToListOfDepartmentViewModel(this IEnumerable<Department> departments)
        {
            List<DepartmentDto> convertedDepartments = new List<DepartmentDto>();

            foreach (Department department in departments)
            {
                DepartmentDto DepartmentViewModel = new()
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName,
                    ManagerId = department.ManagerId,
                    LocationId = department.LocationId
                };
                convertedDepartments.Add(DepartmentViewModel);
            }

            return convertedDepartments;
        }

        public static List<JobHistoryDto> ToListOfJobHistoryViewModel(this IEnumerable<JobHistory> jobHistories)
        {
            List<JobHistoryDto> result = new List<JobHistoryDto>();

            foreach (JobHistory jobHistory in jobHistories)
            {
                JobHistoryDto jobHistoryViewModel = new()
                {
                    EmployeeId = jobHistory.EmployeeId,
                    StartDate = DateOnly.FromDateTime(jobHistory.StartDate),
                    EndDate = DateOnly.FromDateTime(jobHistory.EndDate),
                    JobId = jobHistory.JobId,
                    DepartmentId = jobHistory.DepartmentId
                };
                result.Add(jobHistoryViewModel);
            }

            return result;
        }

        public static List<JobDto> ToListOfJobViewModel(this IEnumerable<Job> jobs)
        {
            List<JobDto> result = new();

            foreach (Job job in jobs)
            {
                JobDto jobViewModel = new()
                {
                    JobId = job.JobId,
                    JobTitle = job.JobTitle,
                    MaxSalary = job.MaxSalary,
                    MinSalary = job.MinSalary
                };
                result.Add(jobViewModel);
            }

            return result;
        }

        public static List<SalaryDto> ToListOfSalaryViewModel(this IEnumerable<Employee> salaries)
        {
            List<SalaryDto> result = new List<SalaryDto>();

            foreach (Employee employee in salaries)
            {
                SalaryDto salaryViewModel = new()
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    JobId = employee.JobId,
                    Salary = employee.Salary,
                };
                result.Add(salaryViewModel);
            }

            return result;
        }
    }
}

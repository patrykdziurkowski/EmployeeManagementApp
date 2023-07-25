using BusinessLogic.ViewModels;
using DataAccess.Models;

namespace BusinessLogic
{
    public static class ListConversionExtensionMethods
    {
        public static List<EmployeeDto> ToListOfEmployeeViewModel(this IEnumerable<Employee> employees)
        {
            List<EmployeeDto> result = new();

            foreach (Employee employee in employees)
            {
                EmployeeDto employeeViewModel = new(
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
                result.Add(employeeViewModel);
            }

            return result;
        }

        public static List<DepartmentLocationDto> ToListOfDepartmentLocationViewModel(this IEnumerable<DepartmentLocation> departmentLocations)
        {
            List<DepartmentLocationDto> result = new();

            foreach (DepartmentLocation departmentLocation in departmentLocations)
            {
                DepartmentLocationDto departmentLocationViewModel = new(
                    departmentLocation.DepartmentId,
                    departmentLocation.DepartmentName,
                    departmentLocation.City)
                {
                    StateProvince = departmentLocation.StateProvince,
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
            List<DepartmentDto> convertedDepartments = new();

            foreach (Department department in departments)
            {
                DepartmentDto DepartmentViewModel = new(
                    department.DepartmentId,
                    department.DepartmentName)
                {
                    ManagerId = department.ManagerId,
                    LocationId = department.LocationId
                };
                convertedDepartments.Add(DepartmentViewModel);
            }

            return convertedDepartments;
        }

        public static List<JobHistoryDto> ToListOfJobHistoryViewModel(this IEnumerable<JobHistory> jobHistories)
        {
            List<JobHistoryDto> result = new();

            foreach (JobHistory jobHistory in jobHistories)
            {
                JobHistoryDto jobHistoryViewModel = new(
                    jobHistory.EmployeeId,
                    jobHistory.JobId)
                {
                    StartDate = DateOnly.FromDateTime(jobHistory.StartDate),
                    EndDate = DateOnly.FromDateTime(jobHistory.EndDate),
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
                JobDto jobViewModel = new(
                    job.JobId,
                    job.JobTitle)
                {
                    MaxSalary = job.MaxSalary,
                    MinSalary = job.MinSalary
                };
                result.Add(jobViewModel);
            }

            return result;
        }

        public static List<SalaryDto> ToListOfSalaryViewModel(this IEnumerable<Employee> salaries)
        {
            List<SalaryDto> result = new();

            foreach (Employee employee in salaries)
            {
                SalaryDto salaryViewModel = new(
                    employee.EmployeeId,
                    employee.LastName,
                    employee.JobId)
                {
                    FirstName = employee.FirstName,
                    Salary = employee.Salary,
                };
                result.Add(salaryViewModel);
            }

            return result;
        }
    }
}

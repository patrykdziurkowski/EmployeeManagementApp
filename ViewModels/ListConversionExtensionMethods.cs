using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public static class ListConversionExtensionMethods
    {
        public static List<EmployeeViewModel> ToListOfEmployeeViewModel(this IEnumerable<Employee> employees)
        {
            List<EmployeeViewModel> result = new List<EmployeeViewModel>();

            foreach (Employee employee in employees)
            {
                EmployeeViewModel employeeViewModel = new()
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HireDate = employee.HireDate,
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

        public static List<DepartmentLocationViewModel> ToListOfDepartmentLocationViewModel(this IEnumerable<DepartmentLocation> departmentLocations)
        {
            List<DepartmentLocationViewModel> result = new List<DepartmentLocationViewModel>();

            foreach (DepartmentLocation departmentLocation in departmentLocations)
            {
                DepartmentLocationViewModel departmentLocationViewModel = new()
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

        public static List<DepartmentViewModel> ToListOfDepartmentViewModel(this IEnumerable<Department> departments)
        {
            List<DepartmentViewModel> convertedDepartments = new List<DepartmentViewModel>();

            foreach (Department department in departments)
            {
                DepartmentViewModel DepartmentViewModel = new()
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

        public static List<JobHistoryViewModel> ToListOfJobHistoryViewModel(this IEnumerable<JobHistory> jobHistories)
        {
            List<JobHistoryViewModel> result = new List<JobHistoryViewModel>();

            foreach (JobHistory jobHistory in jobHistories)
            {
                JobHistoryViewModel jobHistoryViewModel = new()
                {
                    EmployeeId = jobHistory.EmployeeId,
                    StartDate = jobHistory.StartDate,
                    EndDate = jobHistory.EndDate,
                    JobId = jobHistory.JobId,
                    DepartmentId = jobHistory.DepartmentId
                };
                result.Add(jobHistoryViewModel);
            }

            return result;
        }

        public static List<SalaryViewModel> ToListOfSalaryViewModel(this IEnumerable<Employee> salaries)
        {
            List<SalaryViewModel> result = new List<SalaryViewModel>();

            foreach (Employee employee in salaries)
            {
                SalaryViewModel salaryViewModel = new()
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

﻿using DataAccess.Interfaces;
using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class DepartmentRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<Department>> GetAllAsync()
        {
            string query = "SELECT * FROM departments";

            return await _dataAccess
                .ExecuteSqlQueryAsync<Department>(query);
        }

        public virtual async Task<Result<Department>> GetAsync(int departmentId)
        {
            string query = $"SELECT * FROM departments WHERE department_id = :DepartmentId";

            Department? departmentWithGivenId = (await _dataAccess
                .ExecuteSqlQueryAsync<Department>(query, new { DepartmentId = departmentId }))
                .FirstOrDefault();

            if (departmentWithGivenId is null)
            {
                return Result.Fail($"No user with id {departmentId} was found");
            }

            return Result.Ok(departmentWithGivenId);
        }

        public virtual async Task<IEnumerable<Employee>> GetEmployeesForDepartmentAsync(int departmentId)
        {
            string query = $"SELECT * FROM employees WHERE department_id = :DepartmentId";
            
            return await _dataAccess
                .ExecuteSqlQueryAsync<Employee>(query, new { DepartmentId = departmentId });
        }
    }
}

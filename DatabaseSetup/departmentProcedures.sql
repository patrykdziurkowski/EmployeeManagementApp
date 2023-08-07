SET AUTOCOMMIT OFF;
SHOW AUTOCOMMIT;

--PUBLIC PROCEDURE DEFINITIONS
CREATE OR REPLACE PACKAGE departmentProcedures AS
    PROCEDURE getDepartments(
        out_departments_cur     OUT sys_refcursor
    
    );
    
    PROCEDURE getDepartment(
        in_department_id        IN departments.department_id%type,
        out_department_cur      OUT sys_refcursor
    );
    
    PROCEDURE getEmployeesForDepartment(
        in_department_id        IN departments.department_id%type,
        out_employees_cur       OUT sys_refcursor
    );
END departmentProcedures;
/
CREATE OR REPLACE PACKAGE BODY departmentProcedures AS
    PROCEDURE getDepartments(
        out_departments_cur     OUT sys_refcursor
    
    )
    IS
    BEGIN
        OPEN out_departments_cur FOR
            SELECT *
            FROM departments;
    END getDepartments;
    
    PROCEDURE getDepartment(
        in_department_id        IN departments.department_id%type,
        out_department_cur      OUT sys_refcursor
    )
    IS
    BEGIN
        OPEN out_department_cur FOR
            SELECT *
            FROM departments
            WHERE department_id = in_department_id;
    END getDepartment;

    PROCEDURE getEmployeesForDepartment(
        in_department_id        IN departments.department_id%type,
        out_employees_cur       OUT sys_refcursor
    )
    IS
    BEGIN
        OPEN out_employees_cur FOR
            SELECT *
            FROM employees
            WHERE department_id = in_department_id;
    END getEmployeesForDepartment;
    
END departmentProcedures;

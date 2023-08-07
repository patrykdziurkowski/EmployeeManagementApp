SET AUTOCOMMIT OFF;
SHOW AUTOCOMMIT;

--PUBLIC PROCEDURE DEFINITIONS
CREATE OR REPLACE PACKAGE employeeProcedures AS
    PROCEDURE updateEmployee(
        in_employee_id      IN employees.employee_id%type,
        in_first_name       IN employees.first_name%type,
        in_last_name        IN employees.last_name%type,
        in_email            IN employees.email%type,
        in_phone_number     IN employees.phone_number%type,
        in_hire_date        IN employees.hire_date%type,
        in_job_id           IN employees.job_id%type,
        in_salary           IN employees.salary%type,
        in_commission_pct   IN employees.commission_pct%type,
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    );
    
    PROCEDURE createEmployee(
        in_employee_id      IN employees.employee_id%type,
        in_first_name       IN employees.first_name%type,
        in_last_name        IN employees.last_name%type,
        in_email            IN employees.email%type,
        in_phone_number     IN employees.phone_number%type,
        in_hire_date        IN employees.hire_date%type,
        in_job_id           IN employees.job_id%type,
        in_salary           IN employees.salary%type,
        in_commission_pct   IN employees.commission_pct%type,
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    );
    
    PROCEDURE getEmployees(
        out_employees_cur   OUT sys_refcursor
        
    );
    
    PROCEDURE deleteEmployee(
        in_employee_id  IN employees.employee_id%type
        
    );
END employeeProcedures;
/
CREATE OR REPLACE PACKAGE BODY employeeProcedures AS
    --BLANK PROCEDURE DECLARATION (FOR PRIVATE PROCEDURES)
    PROCEDURE correctManagerIfDeptChanged(
        inout_manager_id    IN OUT employees.manager_id%type,
        in_employee_id      IN employees.employee_id%type,
        inout_department_id IN OUT employees.department_id%type
    );
    
    PROCEDURE raiseIfManagerChangedToInvalid(
        in_employee_id      IN employees.employee_id%type,
        inout_department_id IN OUT employees.department_id%type,
        inout_manager_id    IN OUT employees.manager_id%type
    );
    
    PROCEDURE raiseIfManagerFromAnotherDept(
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    );
    
    PROCEDURE validateCommissionPct(
        in_commission_pct   IN employees.commission_pct%type,
        inout_department_id IN OUT employees.department_id%type
    );
    
    PROCEDURE createjobHistoryEntry(
        in_employee_before_change   IN employees%rowtype
        
    );
    
    --PUBLIC PROCEDURES
    PROCEDURE deleteEmployee(
        in_employee_id  IN employees.employee_id%type
    )
    IS
    BEGIN
        DELETE FROM employees
        WHERE employee_id = in_employee_id;
    END deleteEmployee;
    
    PROCEDURE getEmployees(
        out_employees_cur   OUT sys_refcursor
    )
    IS
    BEGIN
        OPEN out_employees_cur FOR
            SELECT *
            FROM employees;
    END getEmployees;
    
    PROCEDURE updateEmployee(
        in_employee_id      IN employees.employee_id%type,
        in_first_name       IN employees.first_name%type,
        in_last_name        IN employees.last_name%type,
        in_email            IN employees.email%type,
        in_phone_number     IN employees.phone_number%type,
        in_hire_date        IN employees.hire_date%type,
        in_job_id           IN employees.job_id%type,
        in_salary           IN employees.salary%type,
        in_commission_pct   IN employees.commission_pct%type,
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    )
    IS
        v_employee_before_change employees%rowtype;
    BEGIN
        --Get employee data before the update
        SELECT *
        INTO v_employee_before_change
        FROM employees
        WHERE employee_id = in_employee_id;
    
        --Validation
        raiseIfManagerChangedToInvalid(in_employee_id, inout_department_id, inout_manager_id);
        correctManagerIfDeptChanged(inout_manager_id, in_employee_id, inout_department_id);
        
        IF v_employee_before_change.job_id <> in_job_id THEN
            --Job was changed, create a job history entry:
            createJobHistoryEntry(v_employee_before_change);
        END IF;
        
        --Update employee data
        UPDATE employees
        SET
            first_name = in_first_name,
            last_name = in_last_name,
            email = in_email,
            phone_number = in_phone_number,
            hire_date = in_hire_date,
            job_id = in_job_id,
            salary = in_salary,
            commission_pct = in_commission_pct,
            manager_id = inout_manager_id,
            department_id = inout_department_id
        WHERE
            employee_id = in_employee_id;
            
        COMMIT;
    END updateEmployee;
    
    PROCEDURE createEmployee(
        in_employee_id      IN employees.employee_id%type,
        in_first_name       IN employees.first_name%type,
        in_last_name        IN employees.last_name%type,
        in_email            IN employees.email%type,
        in_phone_number     IN employees.phone_number%type,
        in_hire_date        IN employees.hire_date%type,
        in_job_id           IN employees.job_id%type,
        in_salary           IN employees.salary%type,
        in_commission_pct   IN employees.commission_pct%type,
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    )
    IS
    BEGIN
        raiseIfManagerFromAnotherDept(inout_manager_id, inout_department_id);
        validateCommissionPct(in_commission_pct, inout_department_id);
        
        INSERT INTO employees
            (employee_id,
            first_name,
            last_name,
            email,
            phone_number,
            hire_date,
            job_id,
            salary,
            commission_pct,
            manager_id,
            department_id)
        VALUES
            (in_employee_id,
            in_first_name,
            in_last_name,
            in_email,
            in_phone_number,
            in_hire_date,
            in_job_id,
            in_salary,
            in_commission_pct,
            inout_manager_id,
            inout_department_id);
    END createEmployee;
    
    --PRIVATE PROCEDURES
    PROCEDURE createjobHistoryEntry(
        in_employee_before_change   IN employees%rowtype
    )
    IS
        v_start_date job_history.start_date%type;
        v_end_date job_history.end_date%type;
    BEGIN
        --Get start date
        SELECT MAX(end_date)
        INTO v_start_date
        FROM job_history
        WHERE employee_id = in_employee_before_change.employee_id;
    
        IF v_start_date IS NULL THEN
            SELECT hire_date
            INTO v_start_date
            FROM employees
            WHERE employee_id = in_employee_before_change.employee_id;
        END IF;
        
        --Get end date
        v_end_date := CURRENT_DATE;
        
        IF v_start_date LIKE v_end_date THEN
            raise_application_error(-20702, 'Cannot change an employee''s job twice in one day');
        END IF;
    
        BEGIN
            INSERT INTO job_history
                (employee_id,
                start_date,
                end_date,
                job_id,
                department_id)
            VALUES
                (in_employee_before_change.employee_id,
                v_start_date,
                v_end_date,
                in_employee_before_change.job_id,
                in_employee_before_change.department_id);
        EXCEPTION
            WHEN others THEN
                ROLLBACK;
                raise_application_error(-20703, SQLERRM);
        END;
    END createJobHistoryEntry;
    
    PROCEDURE correctManagerIfDeptChanged(
        inout_manager_id    IN OUT employees.manager_id%type,
        in_employee_id      IN employees.employee_id%type,
        inout_department_id IN OUT employees.department_id%type
    )
    IS
        v_new_department_manager_id departments.manager_id%type;
        v_current_department_id employees.department_id%type;
    BEGIN
        BEGIN
            SELECT department_id
            INTO v_current_department_id
            FROM employees
            WHERE employee_id = in_employee_id;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_current_department_id := NULL;
        END;
        
        IF (v_current_department_id <> inout_department_id) OR
        (v_current_department_id IS NULL AND inout_department_id IS NOT NULL) OR
        (v_current_department_id IS NOT NULL AND inout_department_id IS NULL) THEN
            --department changed
            BEGIN
                SELECT manager_id
                INTO v_new_department_manager_id
                FROM departments
                WHERE department_id = inout_department_id;
            EXCEPTION
                WHEN NO_DATA_FOUND THEN
                    v_new_department_manager_id := NULL;
            END;
            
            inout_manager_id := v_new_department_manager_id;
        END IF;
    END correctManagerIfDeptChanged;
    
    PROCEDURE raiseIfManagerChangedToInvalid(
        in_employee_id      IN employees.employee_id%type,
        inout_department_id IN OUT employees.department_id%type,
        inout_manager_id    IN OUT employees.manager_id%type
    )
    IS
        v_current_manager_id employees.manager_id%type;
        v_current_departments_manager departments.manager_id%type;
    BEGIN
        BEGIN
            SELECT manager_id
            INTO v_current_manager_id
            FROM employees
            WHERE employee_id = in_employee_id;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_current_manager_id := NULL;
        END;
        
        BEGIN
            SELECT manager_id
            INTO v_current_departments_manager
            FROM departments
            WHERE department_id = inout_department_id;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_current_departments_manager := NULL;
        END;
    
        IF (v_current_manager_id <> inout_manager_id) OR
        (v_current_manager_id IS NULL AND inout_manager_id IS NOT NULL) OR
        (v_current_manager_id IS NOT NULL AND inout_manager_id IS NULL) THEN
            --manager was changed
            IF (v_current_departments_manager <> inout_manager_id) OR
            (v_current_departments_manager IS NOT NULL AND inout_manager_id IS NULL) OR
            (v_current_departments_manager IS NULL AND inout_manager_id IS NOT NULL) THEN
                raise_application_error(-20700, 'Employees manager must manage their department');
            END IF;
        END IF;
    END raiseIfManagerChangedToInvalid;
        
    PROCEDURE validateCommissionPct(
        in_commission_pct   IN employees.commission_pct%type,
        inout_department_id IN OUT employees.department_id%type
    )
    IS
        v_sales_department_id   departments.department_id%type;
    BEGIN
        SELECT department_id
        INTO v_sales_department_id
        FROM departments
        WHERE department_name = 'Sales';
        
        IF (in_commission_pct IS NOT NULL) AND (inout_department_id <> v_sales_department_id)  THEN
            raise_application_error(-20706, 'An employee from a non-sales department cannot have a commission pct');
        END IF;
    END;
        
    PROCEDURE raiseIfManagerFromAnotherDept(
        inout_manager_id    IN OUT employees.manager_id%type,
        inout_department_id IN OUT employees.department_id%type
    )
    IS
        v_departments_manager departments.manager_id%type;
    BEGIN
        BEGIN
            SELECT manager_id
            INTO v_departments_manager
            FROM departments
            WHERE department_id = inout_department_id;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                v_departments_manager := NULL;
        END;
        
        IF (v_departments_manager <> inout_manager_id) OR
        (v_departments_manager IS NOT NULL AND inout_manager_id IS NULL) OR
        (v_departments_manager IS NULL AND inout_manager_id IS NOT NULL) THEN
            raise_application_error(-20705, 'Department ' || inout_department_id || ' is managed by employee ' || v_departments_manager || ' and not by the provided employee ' || inout_manager_id);
        END IF;
    END;
END employeeProcedures;

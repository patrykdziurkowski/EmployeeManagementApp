SET AUTOCOMMIT OFF;
SHOW AUTOCOMMIT;

--PUBLIC PROCEDURE DEFINITIONS
CREATE OR REPLACE PACKAGE jobHistoryProcedures AS
    PROCEDURE getJobHistory(
        out_job_history_cur     OUT sys_refcursor
    
    );
    
    PROCEDURE createJobHistory(
        in_employee_id      IN job_history.employee_id%type,
        in_start_date       IN job_history.start_date%type,
        in_end_date         IN job_history.end_date%type,
        in_job_id           IN job_history.job_id%type,
        in_department_id    IN job_history.department_id%type
    );
END jobHistoryProcedures;
/
CREATE OR REPLACE PACKAGE BODY jobHistoryProcedures AS
    PROCEDURE getJobHistory(
        out_job_history_cur     OUT sys_refcursor
    
    )
    IS
    BEGIN
        OPEN out_job_history_cur FOR
            SELECT *
            FROM job_history;
    END getJobHistory;
    
    PROCEDURE createJobHistory(
        in_employee_id      IN job_history.employee_id%type,
        in_start_date       IN job_history.start_date%type,
        in_end_date         IN job_history.end_date%type,
        in_job_id           IN job_history.job_id%type,
        in_department_id    IN job_history.department_id%type
    )
    IS
    BEGIN
        IF (in_end_date < in_start_date) THEN
            raise_application_error(-27011, 'Job start date is bigger than end date');
        END IF;
        
        INSERT INTO job_history
            (employee_id,
            start_date,
            end_date,
            job_id,
            department_id)
        VALUES
            (in_employee_id,
            in_start_date,
            in_end_date,
            in_job_id,
            in_department_id);   
    END createJobHistory;
    
END jobHistoryProcedures;

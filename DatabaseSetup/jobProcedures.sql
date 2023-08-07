SET AUTOCOMMIT OFF;
SHOW AUTOCOMMIT;

--PUBLIC PROCEDURE DEFINITIONS
CREATE OR REPLACE PACKAGE jobProcedures AS
    PROCEDURE getJobs(
        out_jobs_cur     OUT sys_refcursor
    
    );
END jobProcedures;
/
CREATE OR REPLACE PACKAGE BODY jobProcedures AS
    PROCEDURE getJobs(
        out_jobs_cur     OUT sys_refcursor
    
    )
    IS
    BEGIN
        OPEN out_jobs_cur FOR
            SELECT *
            FROM jobs;
    END getJobs;
END jobProcedures;

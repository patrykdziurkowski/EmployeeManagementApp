SET AUTOCOMMIT OFF;
SHOW AUTOCOMMIT;

--PUBLIC PROCEDURE DEFINITIONS
CREATE OR REPLACE PACKAGE departmentLocationProcedures AS
    PROCEDURE getDepartmentLocations(
        out_department_locations_cur     OUT sys_refcursor
    
    );
END departmentLocationProcedures;
/
CREATE OR REPLACE PACKAGE BODY departmentLocationProcedures AS
    PROCEDURE getDepartmentLocations(
        out_department_locations_cur     OUT sys_refcursor
    
    )
    IS
    BEGIN
        OPEN out_department_locations_cur FOR
            SELECT
                d.department_id,
                d.department_name,
                l.street_address,
                l.city,
                l.state_province,
                c.country_name,
                r.region_name
            FROM departments d
            LEFT JOIN locations l
            ON l.location_id = d.location_id
            LEFT JOIN countries c
            ON c.country_id = l.country_id
            LEFT JOIN regions r
            ON r.region_id = c.region_id;
    END getDepartmentLocations;
END departmentLocationProcedures;

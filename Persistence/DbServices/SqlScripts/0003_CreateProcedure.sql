Use Vaktbok_2;

DELIMITER //
    
CREATE PROCEDURE IF NOT EXISTS GetMinMaxWorkday()
BEGIN
    SELECT * FROM `Workday`
    WHERE `date` = (SELECT MAX(`date`) FROM `Workday`)
       OR `date` = (SELECT MIN(`date`) FROM `Workday`)
    ORDER BY `date` ASC;
END //

CREATE PROCEDURE IF NOT EXISTS GetAllShifts()
BEGIN
    SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
        JOIN `Employee` AS e ON s.EmployeeId = e.Id
        JOIN `Workday` AS w ON s.WorkdayId = w.Id
        JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
        JOIN `FilePath` AS fp ON s.FilePathId = fp.Id;
END //

CREATE PROCEDURE IF NOT EXISTS GetNoRemarkShifts()
BEGIN
    SELECT s.Id, e.name, w.date  FROM `Shift` AS s
        JOIN `Employee` AS e ON s.EmployeeId = e.Id
        JOIN `Workday` AS w ON s.WorkdayId = w.Id
    WHERE s.ShiftRemarkId IS NULL
    ORDER BY DATE;
END //
    
DELIMITER ;
Use Vaktbok_2;

DELIMITER //

CREATE PROCEDURE IF NOT EXISTS GetNoRemarkShifts()
BEGIN
    SELECT s.Id, e.name, w.date  FROM `Shift` AS s
        JOIN `Employee` AS e ON s.EmployeeId = e.Id
        JOIN `Workday` AS w ON s.WorkdayId = w.Id
    WHERE s.ShiftRemarkId IS NULL
    ORDER BY DATE;
END //
    
CREATE PROCEDURE IF NOT EXISTS GetMinMaxWorkday()
BEGIN
    SELECT * FROM `Workday`
    WHERE `date` = (SELECT MAX(`date`) FROM `Workday`)
       OR `date` = (SELECT MIN(`date`) FROM `Workday`)
    ORDER BY `date` ASC;
END //

CREATE PROCEDURE IF NOT EXISTS GetAllEmployees()
BEGIN
	SELECT * FROM Employee WHERE Name != "" ORDER BY NAME;
END //

CREATE PROCEDURE IF NOT EXISTS  GetAllWorkdays()
BEGIN
	SELECT * FROM Workday ORDER BY DATE;
END //

CREATE PROCEDURE IF NOT EXISTS GetAllShiftCodes()
BEGIN
	SELECT * FROM ShiftCode ORDER BY CODE;
END //

CREATE PROCEDURE IF NOT EXISTS  GetAllShiftRemarks()
BEGIN
	SELECT * FROM ShiftRemark ORDER BY REMARK;
END //

CREATE PROCEDURE IF NOT EXISTS GetAllShifts()
BEGIN
    SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
        JOIN `Employee` AS e ON s.EmployeeId = e.Id
        JOIN `Workday` AS w ON s.WorkdayId = w.Id
        JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
        JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
	ORDER BY w.Date, e.Name;
END //

CREATE PROCEDURE IF NOT EXISTS GetShiftsByWeekAndYear(IN weekParam SMALLINT, IN yearParam SMALLINT)
BEGIN
	SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
		JOIN `Employee` AS e ON s.EmployeeId = e.Id
		JOIN `Workday` AS w ON s.WorkdayId = w.Id
		JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
		JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
	WHERE w.Week = weekParam and year = yearParam
	ORDER BY w.Date, e.Name;
END //

CREATE PROCEDURE IF NOT EXISTS GetShiftsByEmployeeId(IN idParam VARCHAR(36))
BEGIN
	SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
		JOIN `Employee` AS e ON s.EmployeeId = e.Id
		JOIN `Workday` AS w ON s.WorkdayId = w.Id
		JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
		JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
	WHERE e.id = idParam
	ORDER BY w.Date, e.Name;
END //

CREATE PROCEDURE IF NOT EXISTS GetShiftsByWordayId(IN idParam VARCHAR(36))
BEGIN
	SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
		JOIN `Employee` AS e ON s.EmployeeId = e.Id
		JOIN `Workday` AS w ON s.WorkdayId = w.Id
		JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
		JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
	WHERE w.id = idParam
	ORDER BY w.Date, e.Name;
END //

CREATE PROCEDURE IF NOT EXISTS GetShiftsByEmployeeIdAndWordayId(IN eIdParam VARCHAR(36), IN wIdParam VARCHAR(36))
BEGIN
	SELECT s.Id, s.Time, s.CreatedAt, e.*, w.*, sc.*, fp.* FROM `Shift` AS s
		JOIN `Employee` AS e ON s.EmployeeId = e.Id
		JOIN `Workday` AS w ON s.WorkdayId = w.Id
		JOIN `ShiftCode`AS sc ON s.ShiftCodeId = sc.Id
		JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
	WHERE e.id = eIdParam and w.id = wIdParam
	ORDER BY w.Date, e.Name;
END //

DELIMITER ;
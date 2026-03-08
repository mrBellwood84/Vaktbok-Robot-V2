DELIMITER //

CREATE PROCEDURE IF NOT EXISTS GetAllShiftsMapped()
BEGIN
SELECT
    -- Shift
    s.Id AS shiftId,
    s.EmployeeId AS shiftEmployeeId,
    s.WorkdayId AS shiftWorkdayId,
    s.ShiftCodeId AS shiftShiftCodeId,
    s.ShiftRemarkId AS shiftShiftRemarkId,
    s.FilePathId AS shiftFilePathId,
    s.Time AS shiftTime,
    s.CreatedAt AS shiftCreatedAt,

    -- Employee
    e.Id AS employeeId,
    e.Name AS employeeName,
    e.CreatedAt AS employeeCreatedAt,

    -- Workday
    w.Id AS workdayId,
    w.Day AS workdayDay,
    w.Week AS workdayWeek,
    w.Year AS workdayYear,
    w.Date AS workdayDate,
    w.CreatedAt AS workdayCreatedAt,

    -- ShiftCode
    sc.Id AS shiftCodeId,
    sc.Code AS shiftCodeCode,
    sc.CreatedAt AS shiftCodeCreatedAt,

    -- ShiftRemark
    sr.Id AS shiftRemarkId,
    sr.Remark AS shiftRemarkRemark,
    sr.CreatedAt AS shiftRemarkCreatedAt,

    -- FilePath
    fp.Id AS filePathId,
    fp.Path AS filePathPath,
    fp.CreatedAt AS filePathCreatedAt

FROM `Shift` AS s
         JOIN `Employee` AS e ON s.EmployeeId = e.Id
         JOIN `Workday` AS w ON s.WorkdayId = w.Id
         LEFT JOIN `ShiftCode` AS sc ON s.ShiftCodeId = sc.Id
         LEFT JOIN `ShiftRemark` AS sr ON s.ShiftRemarkId = sr.Id
         LEFT JOIN `FilePath` AS fp ON s.FilePathId = fp.Id
ORDER BY w.Date ASC, e.Name ASC;
END //

DELIMITER ;
using Domain.Entities;
using Domain.SourceModels;
using System.Net.WebSockets;
using System.Text.RegularExpressions;

namespace WebHarvester.Cropper
{
    public class ShiftBookWeekCropper(
        SourceEmployeeWeek rawData,
        List<Employee> employees,
        List<Workday> workdays,
        List<ShiftCode> shiftCodes)
    {

        private readonly string _shiftCodePattern = @"[A-Z][A-Z\d\-]* \(";
        private readonly string _shiftTimePattern = @"\(\d{2}:\d{2} - \d{2}:\d{2}\)";
        private readonly string _anyTimePattern = @"\d{2}:\d{2} - \d{2}:\d{2}";
        private readonly string _singleTimePattern = @"\d{2}:\d{2}";

        public Employee NewEmployee { get; private set; }
        public List<Workday> NewWorkdays { get; private set; } = [];
        public List<ShiftCode> NewShiftCodes { get; private set; } = [];
        public List<Shift> Shifts { get; private set; } = [];

        /// <summary>
        /// Processes raw shift data and populates the collection of shifts for the current employee.
        /// </summary>
        /// <remarks>This method extracts shift information from the underlying raw data and creates
        /// corresponding shift objects. It should be called after raw data has been loaded and before accessing the
        /// populated shifts. This method does not return a value and may modify the internal state of the shifts
        /// collection.</remarks>
        public void Crop()
        {
            // extract employee model
            var employee = CropEmployee();

            // extract shift code model
            foreach (var rawShift in rawData.Shifts)
            {
                var workday = CropWorkday(rawShift);
                var shiftCode = CropShiftcode(rawShift);
                var shiftTime = CropShiftTime(rawShift);

                var shift = new Shift
                {
                    IdBinary = Guid.NewGuid().ToByteArray(),
                    EmployeeIdBinary = employee.IdBinary,
                    WorkdayIdBinary = workday.IdBinary,
                    ShiftCodeIdBinary = shiftCode.IdBinary,
                    Time = shiftTime
                };

                Shifts.Add(shift);
            }
        }


        /// <summary>
        /// Retrieves an existing employee matching the current raw data, or creates a new employee if none exists.
        /// </summary>
        /// <returns>An existing employee with a name matching the current raw data, or a newly created employee if no match is
        /// found.</returns>
        private Employee CropEmployee()
        {
            var exists = employees.Where(x => x.Name == rawData.EmployeeName).FirstOrDefault();
            if (exists != null) return exists;
            
            NewEmployee = new Employee
            {
                IdBinary = Guid.NewGuid().ToByteArray(),
                Name = rawData.EmployeeName,
            };
            
            return NewEmployee;
        }

        /// <summary>
        /// Retrieves an existing workday matching the specified shift entry date, or creates a new workday if none
        /// exists.
        /// </summary>
        /// <remarks>If a new workday is created, it is added to the <c>NewWorkdays</c> collection. This
        /// method ensures that only one workday exists for each shift date.</remarks>
        /// <param name="entry">The source shift entry containing the date and related information used to identify or create a workday.</param>
        /// <returns>A <see cref="Workday"/> instance corresponding to the shift entry date. If a matching workday already
        /// exists, it is returned; otherwise, a new workday is created and returned.</returns>
        private Workday CropWorkday(SourceShiftEntry entry)
        {
            var exists = workdays.Where(x => x.Date == entry.ShiftDate).FirstOrDefault();
            if (exists != null) return exists;

            var inNew = NewWorkdays.Where(x => x.Date != entry.ShiftDate).FirstOrDefault();
            if (inNew != null) return inNew;

            var newWorkday = new Workday
            {
                IdBinary = Guid.NewGuid().ToByteArray(),
                Day = (short)entry.Day,
                Week = (short)entry.WeekNumber,
                Year = (short)entry.Year,
                Date = entry.ShiftDate,
            };
            NewWorkdays.Add(newWorkday);
            return newWorkday;
        }

        /// <summary>
        /// Extracts the shift code from the specified source entry, returning an existing matching code if found or
        /// creating a new one.
        /// </summary>
        /// <param name="entry">The source shift entry from which to extract the shift code. Cannot be null.</param>
        /// <returns>A <see cref="ShiftCode"/> instance representing the extracted shift code. If a matching code already exists,
        /// that instance is returned; otherwise, a new instance is created and returned.</returns>
        private ShiftCode CropShiftcode(SourceShiftEntry entry)
        {
            var regex = new Regex(_shiftCodePattern);
            var match = regex.Match(entry.CellContent);
            var code = match.Success ? match.Groups[0].Value : "-";
            if (code.Length > 1) code = code.Substring(0, code.Length - 2).Trim();

            var exists = shiftCodes.Where(x => x.Code == code).FirstOrDefault();
            if (exists != null) return exists;

            var inNew = NewShiftCodes.Where(x => x.Code == code).FirstOrDefault();
            if (inNew != null) return inNew;
            
            var newShiftCode = new ShiftCode
            {
                IdBinary = Guid.NewGuid().ToByteArray(),
                Code = code,
            };

            NewShiftCodes.Add(newShiftCode);
            return newShiftCode;
        }

        private string CropShiftTime(SourceShiftEntry entry)
        {
            var data = entry.CellContent;

            // return dash if empty
            if (string.IsNullOrEmpty(data)) return null;

            // find shift time in parentheses
            var shiftTimeRegex = new Regex(_shiftTimePattern);
            var match = shiftTimeRegex.Match(data);
            var shiftTime = match.Success ? match.Groups[0].Value : null;
            if (shiftTime != null)
            {
                // remove parentheses
                shiftTime = shiftTime.Substring(1, shiftTime.Length - 2);
                return shiftTime.Trim();
            }

            // find shift time without parentheses
            var anyTimeRegex = new Regex(_anyTimePattern);
            var anyTimeMatches = anyTimeRegex.Matches(data);
            if (anyTimeMatches.Count == 1)
            {
                // return the single match
                var time = anyTimeMatches[0].Groups[0].Value;
                return time.Trim();
            }

            // find first and last time if multiple times exist
            if (anyTimeMatches.Count > 1)
            {
                var singleTimeRegex = new Regex(_singleTimePattern);
                var singleTimeMatches = singleTimeRegex.Matches(data);
                var first = singleTimeMatches[0].Groups[0].Value.Trim();
                var last = singleTimeMatches[singleTimeMatches.Count - 1].Groups[0].Value.Trim();
                return $"{first} - {last}";
            }


            return null;
        }
    }
}

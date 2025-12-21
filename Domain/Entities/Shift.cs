using Domain.Interfaces;

namespace Domain.Entities
{
    public class Shift : IHasIdBinary
    {
        private Guid _employeeGuid;
        private Guid _workdayGuid;
        private Guid _shiftCodeGuid;
        private Guid _shiftRemarkGuid;
        private Guid _filePathGuid;
        
        public byte[] IdBinary { get; set; }
        
        public byte[] EmployeeIdBinary { get; init; }
        public byte[] WorkdayIdBinary { get; init; }
        public byte[] ShiftCodeIdBinary { get; init; }
        public byte[] ShiftRemarkIdBinary { get; set; }
        public byte[] FilePathIdBinary { get; set; }

        public Guid EmployeeGuid
        {
            get
            {
                if (_employeeGuid != Guid.Empty) return _employeeGuid;
                _employeeGuid = new Guid(EmployeeIdBinary);
                return _employeeGuid;
            }
        }

        public Guid WorkdayGuid
        {
            get
            {
                if (_workdayGuid != Guid.Empty) return _workdayGuid;
                _workdayGuid = new Guid(WorkdayIdBinary);
                return _workdayGuid;
            }
        }

        public Guid ShiftCodeGuid
        {
            get
            {
                if (_shiftCodeGuid != Guid.Empty) return _shiftCodeGuid;
                _shiftCodeGuid = new Guid(ShiftCodeIdBinary);
                return _shiftCodeGuid;
            }
        }

        public Guid ShiftRemarkGuid
        {
            get
            {
                if (_shiftRemarkGuid != Guid.Empty) return _shiftRemarkGuid;
                _shiftRemarkGuid = new Guid(ShiftRemarkIdBinary);
                return _shiftRemarkGuid;
            }
        }

        public Guid FilePathGuid
        {
            get
            {
                if (_filePathGuid != Guid.Empty) return _filePathGuid;
                _filePathGuid = new Guid(FilePathIdBinary);
                return _filePathGuid;
            }
        }

        public string Time { get; init; }

        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }

        public DateTime CreatedAt { get; set; }
    }
}

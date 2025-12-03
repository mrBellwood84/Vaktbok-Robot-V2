namespace Domain.Entities
{
    public class Shift
    {
        public byte[] IdBinary { get; set; }
        public byte[] EmployeeId { get; set; }
        public byte[] WorkdayId { get; set; }
        public byte[] ShiftCodeId { get; set; }
        public byte[] ShiftRemarkId { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }

        public DateTime CreatedAt { get; set; }
    }
}

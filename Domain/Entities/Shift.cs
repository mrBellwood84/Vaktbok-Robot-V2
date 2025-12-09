using Domain.Interfaces;

namespace Domain.Entities
{
    public class Shift : IHasIdBinary
    {
        public byte[] IdBinary { get; set; }
        public byte[] EmployeeId { get; init; }
        public byte[] WorkdayId { get; init; }
        public byte[] ShiftCodeId { get; init; }
        public byte[] ShiftRemarkId { get; set; }
        
        public string Time { get; init; }

        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }

        public DateTime CreatedAt { get; set; }
    }
}

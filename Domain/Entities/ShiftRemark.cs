using Domain.Interfaces;

namespace Domain.Entities
{
    public class ShiftRemark : IHasIdBinary
    {
        public byte[] IdBinary { get; set; }
        public string Remark { get; set; }
     
        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }
        public DateTime CreatedAt { get; set; }
    }
}

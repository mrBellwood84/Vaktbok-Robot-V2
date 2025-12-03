namespace Domain.Entities
{
    public class ShiftCode
    {
        public byte[] IdBinary { get; set; }
        public string Code { get; set; }

        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }
        public DateTime CreatedAt { get; set; }
    }
}

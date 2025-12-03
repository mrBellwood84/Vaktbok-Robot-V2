namespace Domain.Entities
{
    public class Workday
    {
        public byte[] IdBinary { get; set; }
        public short Day { get; set; }
        public short Week { get; set; }
        public short Date { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

        public Guid Id
        {
            get => new Guid(IdBinary);
            set => IdBinary = value.ToByteArray();
        }
        public DateTime CreatedAt { get; set; }
    }
}

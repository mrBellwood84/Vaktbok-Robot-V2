namespace Domain.SourceModels
{
    public struct SourceWorkbookWeekHeader
    {
        public int Day { get; internal set; }
        public int Date { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public void GetDayInteger(string day)
        {
            Day = day.ToLower() switch
            {
                "mandag" => 1,
                "tirsdag" => 2,
                "onsdag" => 3,
                "torsdag" => 4,
                "fredag" => 5,
                "lørdag" => 6,
                "søndag" => 7,
                _ => 0
            };
        }
    }
}

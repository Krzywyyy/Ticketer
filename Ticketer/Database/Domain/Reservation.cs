namespace Ticketer.Database.Domain
{
    class Reservation
    {
        public int Id { get; set; }
        public Spectacle Spectacle { get; set; }
        public string Day { get; set; }
        public int Time { get; set; }
        public string Order { get; set; }
        public float Price { get; set; }
        public string Client { get; set; }
    }
}

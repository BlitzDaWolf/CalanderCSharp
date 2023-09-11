namespace CalanderCSharp.Entities
{
    public class CalanderEvent
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }


        public int UserId { get; set; }
        public User User { get; set; } = null;
    }
}

namespace CalanderCSharp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;

        public ICollection<CalanderEvent> Events { get; set; } = new List<CalanderEvent>();

        public int? UserGroupId { get; set; }
        public Group UserGroup { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace CalanderCSharp.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int CreatorId { get; set; }
        //public User Creator { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<CalanderEvent> Events { get; set; } = new List<CalanderEvent>();
    }
}

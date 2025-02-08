namespace Svistelka.Models
{
    public class Relation
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }

        public virtual User? Follower { get; set; }
        public virtual User? Followed { get; set; }
    }
}

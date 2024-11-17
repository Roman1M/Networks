namespace Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePublish { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}

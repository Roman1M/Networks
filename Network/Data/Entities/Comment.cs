﻿namespace Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePublish { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}

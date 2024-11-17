using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public User? Sender { get; set; }
        public string ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

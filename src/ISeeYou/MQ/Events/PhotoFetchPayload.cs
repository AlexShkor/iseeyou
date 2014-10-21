using System;

namespace ISeeYou.MQ.Events
{
    public class PhotoFetchPayload
    {
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public DateTime Published { get; set; }
        public DateTime? New { get; set; }
        public string DocId { get; set; }
    }
}
using System;

namespace ISeeYou.MQ.Events
{
    public class SourceFetchPayload
    {
        public int UserId { get; set; }
        public bool New { get; set; }
        public DateTime Published { get; set; }
    }
}
using System;

namespace CC98.Software.Data
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Time { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get;set; }
    }
}
using System;

namespace ConsoleApp.Models
{
    public class RPOperations
    {
        public long Id { get; set; }
        public short IdRPStatus { get; set; }
        public DateTime DateTimeRegistration { get; set; }
        public DateTime DateTimeExecution { get; set; }
        public string PartInformation { get; set; }
        public string Description { get; set; }

        public RPStatus Status { get; set; }
    }
}

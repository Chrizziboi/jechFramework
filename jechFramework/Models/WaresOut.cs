using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class WaresOut
    {
        public int OrderId { get; set; }
        public int InternalId { get; set; }
        public DateTime ScheduledTime { get; set; }
    }

    public class RecurringWaresOut
    {
        public int OrderId { get; set; }
        public int InternalId { get; set; }
        public DateTime StartTime { get; set; }
        public RecurrencePattern RecurrencePattern { get; set; }
    }

    public enum RecurrencePattern
    {
        Daily,
        Weekly
    }
}
using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    public class RecurringOrder
    {
        public int OrderId { get; set; }
        
        public DateTime StartTime { get; set; }

       public RecurrencePattern RecurrencePattern { get; set; }
        public List<Item> Items { get; set; }

        public RecurringOrder()
        {
            Items = new List<Item>();
        }

        
    }

   public enum RecurrencePattern
   {
        Daily,
        Weekly,
    }
}

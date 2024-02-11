using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    internal class RecurringOrder
    {
        public int OrderId { get; set; }
        public DateTime StartTime { get; set; }
        public RecurrencePattern RecurrencePattern { get; set; }
        public List<Item> Items { get; set; }

        public RecurringOrder()
        {
            Items = new List<Item>();
        }

        // ... eventuelle ekstra konstruktører og metoder ...
    }

    internal enum RecurrencePattern
    {
        Daily,
        Weekly,
        // ... andre gjentakelsesmønstre om nødvendig ...
    }
}
using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    internal class WaresIn  // Beholder WaresIn-klassen som internal
    {
        // Egenskaper for WaresIn-modellen
        public int orderId { get; set; }
        public DateTime scheduledTime { get; set; }
        public string location { get; set; }
        public List<Item> incomingItems { get; set; }

        // Konstruktør for WaresIn
        public WaresIn(int orderId, DateTime scheduledTime, string location, List<Item> items)
        {
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.location = location;
            this.incomingItems = incomingItems;
        }
        public WaresIn()
        {

        }
    }
}

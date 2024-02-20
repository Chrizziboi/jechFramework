using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    internal class WaresOut
    {
        // Properties for the WaresOut model
        public int OrderId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string Destination { get; set; }
        public List<Item> Items { get; set; }  // Assume there is a list of Item objects

        // Constructor for WaresOut
        public WaresOut(int orderId, DateTime scheduledTime, string destination, List<Item> items)
        {
            OrderId = orderId;
            ScheduledTime = scheduledTime;
            Destination = destination;
            Items = items;
        }
        public WaresOut()
        {
            
        }
    }
}

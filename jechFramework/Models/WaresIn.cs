using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Interfaces;
using jechFramework.Models;
using jechFramework.Services;


namespace jechFramework.Models
{
    internal class WaresIn
    {
        public int orderId { get; set; } // Unik ordre-ID
        public DateTime scheduledTime { get; set; } // Planlagt tidspunkt for mottak
        public string location { get; set; } // Plassering av de innkommende varene
        public TimeSpan processingTime { get; set; } // Tidsforbruk fra mottak til plassering
        public List<Item> incomingItems { get; set; } // Liste over innkommende varer

        public WaresIn()
        {
            incomingItems = new List<Item>();
        }

        public WaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.location = location;
            this.processingTime = processingTime;
            this.incomingItems = incomingItems ?? new List<Item>(); // Sikre at incomingItems ikke er null
        }
    }
}

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
        public Zone storageZone { get; set; } // Referanse til lagerets sone
        public TimeSpan processingTime { get; set; } // Tidsforbruk fra mottak til plassering
        public List<Item> Items { get; set; } // Liste over varer i innkomsten

        // Enkel konstruktør
        public WaresIn()
        {
            Items = new List<Item>();
        }

        // Konstruktør med parameter
        public WaresIn(int orderId, DateTime scheduledTime, Zone storageZone, TimeSpan processingTime)
        {
            orderId = orderId;
            scheduledTime = scheduledTime;
            storageZone = storageZone;
            processingTime = processingTime;
            Items = new List<Item>();
        }

        // Eventuelt andre konstruktører som initialiserer forskjellige kombinasjoner av datafeltene
    }
}
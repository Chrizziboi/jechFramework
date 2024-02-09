using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    public class WaresIn
    {
        public int OrderId { get; set; } // Unik ordre-ID
        public DateTime ScheduledTime { get; set; } // Planlagt tidspunkt for mottak
        public Zone StorageZone { get; set; } // Referanse til lagerets sone
        public TimeSpan ProcessingTime { get; set; } // Tidsforbruk fra mottak til plassering
        public List<Item> ItemList { get; set; } = new List<Item>(); // Liste over varer i ordren

        // Metode for å legge til en vare i varelisten
        public void AddItem(Item item)
        {
            if (item != null)
            {
                ItemList.Add(item);
            }
            else
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null");
            }
        }
    }

    public class RecurringOrder
    {
        public int OrderId { get; set; } // Unik ordre-ID
        public List<Item> Items { get; set; } // Liste over varer i den gjentagende ordren
        public DateTime StartTime { get; set; } // Starttidspunkt for den første gjentagelsen
        public RecurrencePattern RecurrencePattern { get; set; } // Mønster for gjentakelse

        public RecurringOrder()
        {
            Items = new List<Item>(); // Initialiser listen for å unngå NullReferenceException
        }

        // ... Eventuelle andre metoder og logikk som trengs for RecurringOrder
    }

    public enum RecurrencePattern
    {
        Daily,   // Mottak skjer hver dag.
        Weekly   // Mottak skjer ukentlig.
    }
}
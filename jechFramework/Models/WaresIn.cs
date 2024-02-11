using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Models;
using jechFramework.Services;

namespace jechFramework.Models
{
    public class WaresIn 

    {
        public int orderId { get; set; } 
        // Unik ordre-ID
        public DateTime scheduledTime { get; set; } 
        // Planlagt tidspunkt for mottak
        public Zone storageZone { get; set; } 
        // Referanse til lagerets sone
        public TimeSpan processingTime { get; set; } 
        // Tidsforbruk fra mottak til plassering

        private readonly ItemService _itemService;

        public WaresIn(ItemService itemService)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService), "ItemService cannot be null");
        }

        // Metode for å legge til en vare i ItemService sin liste
        public void AddItem(Item item)
        {
            _itemService.AddItem(item);
        }
    }

    public class RecurringOrder
    {
        public int OrderId { get; set; } 
        // Unik ordre-ID
        public List<Item> Items { get; set; } 
        // Liste over varer i den gjentagende ordren
        public DateTime StartTime { get; set; } 
        // Starttidspunkt for den første gjentagelsen
        public RecurrencePattern RecurrencePattern { get; set; } 
        // Mønster for gjentakelse

        public RecurringOrder()
        {
            Items = new List<Item>(); // Initialiser listen for å unngå NullReferenceException
        }

    }

    public enum RecurrencePattern
    {
        Daily,   // Mottak skjer hver dag.
        Weekly   // Mottak skjer ukentlig.
    }
}
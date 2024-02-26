using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    internal class WaresOut
    {
        // Egenskaper for WaresOut modellen
        public int OrderId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string Destination { get; set; }
        public List<Item> Items { get; set; } 

        /// <summary>
        /// Det er en konstruktør for "WaresOut"-klassen: 
        /// En konstruktør som tar fire parametere for å initialisere egenskapene "OrderId", "ScheduledTime", "Destination" og "Items".
        /// </summary>
        /// <param name="orderId"> Dette er en ordreId for ordre som skal ut fra et lager</param>
        /// <param name="scheduledTime"> Dette er noe som sier når en ordre skal ut av et lager</param>
        /// <param name="destination"> Dette er hvilken kunde ordren skal til</param>
        /// <param name="items"> Dette er en liste med alle gjenstander som skal ut med en gitt ordre</param> <summary>
         
        public WaresOut(int orderId, DateTime scheduledTime, string destination, List<Item> items)
        {
            OrderId = orderId;
            ScheduledTime = scheduledTime;
            Destination = destination;
            Items = items;
        }
        /// <summary>
        /// En standardkonstruktør uten parametere som ikke utfører noen spesifikk handling.
        /// Dette er for å kunne bruke waresout objekter i andre filer.
        /// </summary>
         
        public WaresOut()
        {
            
        }
    }
}

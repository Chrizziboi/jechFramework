using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    internal class WaresIn  // Beholder WaresIn-klassen som internal
    {

        /// <summary>
        /// WaresIn klassen er laget for å kunne få varer inn på lageret.
        /// </summary>

        // Egenskaper for WaresIn-modellen
        public int orderId { get; set; }
        public DateTime scheduledTime { get; set; }
        public int zoneId { get; set; }
        public List<Item> incomingItems { get; set; }

        /// <summary>
        /// Initialiserer en ny instans av WaresIn-klassen med 4 parametere.
        /// </summary>
        /// <param name="orderId"> Dette er en int som representerer ordrenummeret. </param>
        /// <param name="scheduledTime"> Dette er en DateTime-objekt som representerer planlagt tidspunkt for ordren. </param>
        /// <param name="zoneId"> En String som angir lokasjonen for ordren. </param>
        /// <param name="items"> Dette er en liste av elementer (Item-objekter) som representerer de varene som kommer inn. </param>
        public WaresIn(int orderId, DateTime scheduledTime, int zoneId, List<Item> items)
        {
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.zoneId = zoneId;
            this.incomingItems = incomingItems;
        }
        /// <summary>
        /// Initialiserer en ny instans av WaresIn-klassen uten parametere.
        /// </summary>
        public WaresIn()
        {

        }
    }
}

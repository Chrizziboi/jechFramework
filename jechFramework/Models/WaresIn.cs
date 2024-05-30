using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    /// <summary>
    /// WaresIn klassen er laget for å kunne få varer inn på lageret.
    /// </summary>
    internal class WaresIn  // Beholder WaresIn-klassen som internal
    {
        /// <summary>
        /// Henter eller setter ordre-ID-en for den innkommende ordren.
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// Henter eller setter det planlagte tidspunktet for den innkommende ordren.
        /// </summary>
        public DateTime scheduledTime { get; set; }

        /// <summary>
        /// Henter eller setter ID-en for sonen der varene skal plasseres.
        /// </summary>
        public int zoneId { get; set; }

        /// <summary>
        /// Henter eller setter listen over varer for den innkommende ordren.
        /// </summary>
        public List<Item> incomingItems { get; set; }

        /// <summary>
        /// Initialiserer en ny instans av WaresIn-klassen uten parametere.
        /// </summary>
        public WaresIn()
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av WaresIn-klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="orderId">En int som representerer ordrenummeret.</param>
        /// <param name="scheduledTime">En DateTime-objekt som representerer planlagt tidspunkt for ordren.</param>
        /// <param name="zoneId">En int som angir sonen for ordren.</param>
        /// <param name="items">En liste av elementer (Item-objekter) som representerer de varene som kommer inn.</param>
        public WaresIn(int orderId, DateTime scheduledTime, int zoneId, List<Item> items)
        {
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.zoneId = zoneId;
            this.incomingItems = incomingItems;
        }


    }
}

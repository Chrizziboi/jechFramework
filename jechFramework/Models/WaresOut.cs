using System;
using System.Collections.Generic;

namespace jechFramework.Models
{
    /// <summary>
    /// Representerer en utgående ordre fra lageret.
    /// </summary>
    internal class WaresOut
    {
        /// <summary>
        /// Henter eller setter ordre-ID-en for den utgående ordren.
        /// </summary>
        public int orderId { get; set; }

        /// <summary>
        /// Henter eller setter det planlagte tidspunktet for den utgående ordren.
        /// </summary>
        public DateTime scheduledTime { get; set; }

        /// <summary>
        /// Henter eller setter destinasjonen for den utgående ordren.
        /// </summary>
        public string destination { get; set; }

        /// <summary>
        /// Henter eller setter listen over varer for den utgående ordren.
        /// </summary>
        public List<Item> items { get; set; }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="WaresOut"/>-klassen.
        /// </summary>
        public WaresOut()
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="WaresOut"/>-klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="orderId">Ordre-ID-en for ordren som skal ut fra lageret.</param>
        /// <param name="scheduledTime">Det planlagte tidspunktet for når ordren skal ut fra lageret.</param>
        /// <param name="destination">Destinasjonen som ordren skal til.</param>
        /// <param name="items">Listen over alle varer som skal ut med den gitte ordren.</param>
        public WaresOut(int orderId, DateTime scheduledTime, string destination, List<Item> items)
        {
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.destination = destination;
            this.items = items;
        }
    }
}

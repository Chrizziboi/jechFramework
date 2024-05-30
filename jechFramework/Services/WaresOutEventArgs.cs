using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    /// <summary>
    /// Argumenter for WaresOut hendelser.
    /// </summary>
    public class WaresOutEventArgs
    {

        /// <summary>
        /// Henter ID-en til varehuset.
        /// </summary>
        public int warehouseId { get; private set; }

        /// <summary>
        /// Henter ID-en til ordren.
        /// </summary>
        public int orderId { get; private set; }

        /// <summary>
        /// Henter tidspunktet da varene er planlagt å sendes ut.
        /// </summary>
        public DateTime scheduledTime { get; private set; }

        /// <summary>
        /// Henter destinasjonen for varene som skal sendes ut.
        /// </summary>
        public string destination { get; private set; }

        /// <summary>
        /// Henter listen over utgående varer.
        /// </summary>
        public List<Item> outgoingItems { get; private set; }

        /// <summary>
        /// Henter det siste sendingnummeret.
        /// </summary>
        public int lastShipmentNumber { get; private set; }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="WaresOutEventArgs"/> klassen.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="scheduledTime">Tidspunktet da varene er planlagt å sendes ut.</param>
        /// <param name="destination">Destinasjonen for varene som skal sendes ut.</param>
        /// <param name="outgoingItems">Listen over utgående varer.</param>
        /// <param name="lastShipmentNumber">Det siste sendingnummeret.</param>
        public WaresOutEventArgs(
            int warehouseId, 
            int orderId, 
            DateTime scheduledTime, 
            string destination, 
            List<Item> outgoingItems, 
            int lastShipmentNumber)
        {
            this.warehouseId = warehouseId;
            this.orderId = orderId;
            this.scheduledTime = scheduledTime;
            this.destination = destination;
            this.outgoingItems = outgoingItems;
            this.lastShipmentNumber = lastShipmentNumber;
        }
    }
}
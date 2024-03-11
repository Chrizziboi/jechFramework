using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    public class WaresOutEventArgs
    {
        public int warehouseId { get; private set; }
        public int orderId { get; private set; }
        public DateTime scheduledTime { get; private set; }
        public string destination { get; private set; }
        public List<Item> outgoingItems { get; private set; }
        public int lastShipmentNumber { get; private set; }

        public WaresOutEventArgs(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems, int lastShipmentNumber)
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
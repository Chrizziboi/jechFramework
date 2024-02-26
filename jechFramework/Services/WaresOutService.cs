using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    public class WaresOutService : IWaresOutService
    {
        private readonly List<WaresOut> scheduledWaresOuts = new List<WaresOut>();
        private readonly ItemService itemService; // Assuming there is an ItemService to handle items in the warehouse

        // Constructor to inject ItemService
        public WaresOutService(ItemService itemService)
        {
            this.itemService = itemService;
        }


        public void ScheduleWaresOut(int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems)
        {
            if (outgoingItems == null) throw new ArgumentNullException(nameof(outgoingItems));

            // Check if a wares out with this orderId is already scheduled
            if (scheduledWaresOuts.Any(wo => wo.OrderId == orderId))
            {
                throw new InvalidOperationException("A wares out with this orderId is already scheduled.");
            }

            var waresOut = new WaresOut
            {
                OrderId = orderId,
                ScheduledTime = scheduledTime,
                Destination = destination,
                Items = outgoingItems // Set outgoing items to the WaresOut object
            };

            scheduledWaresOuts.Add(waresOut);

            // Remove the outgoing items from the warehouse
            foreach (var item in outgoingItems)
            {
                itemService.RemoveItem(item.internalId); // Assuming there is a method to remove an item
            }
        }


    }
}


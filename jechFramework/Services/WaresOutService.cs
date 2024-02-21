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

            if (scheduledWaresOuts.Any(wo => wo.OrderId == orderId))
            {
                throw new InvalidOperationException("A wares out with this orderId is already scheduled.");
            }

            // Sjekker lagerbeholdningen for hvert utgående vareelement
            foreach (var item in outgoingItems)
            {
                if (itemService.FindHowManyItemsInItemList(item.internalId) <= 0)
                {
                    throw new InvalidOperationException($"Item with internal ID {item.internalId} is unavailable.");
                }
            }

            var waresOut = new WaresOut
            {
                OrderId = orderId,
                ScheduledTime = scheduledTime,
                Destination = destination,
                Items = outgoingItems
            };

            scheduledWaresOuts.Add(waresOut);

            // Fjerner de utgående varene fra lageret
            foreach (var item in outgoingItems)
            {
                itemService.RemoveItem(item.internalId); // Antatt at dette reduserer antallet korrekt
            }
        }

    }
}


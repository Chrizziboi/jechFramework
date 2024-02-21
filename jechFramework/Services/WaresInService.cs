using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    public class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> scheduledWaresIns = new List<WaresIn>();
        private readonly ItemService itemService; // Beholder referansen til ItemService

        // Konstruktør for å injisere ItemService
        public WaresInService(ItemService itemService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        public void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            if (incomingItems == null) throw new ArgumentNullException(nameof(incomingItems));
            if (scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new InvalidOperationException("A wares in with this orderId is already scheduled.");
            }

            var waresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                location = location,
                incomingItems = incomingItems
            };

            scheduledWaresIns.Add(waresIn);

            foreach (var item in incomingItems)
            {
                // Sjekker om item allerede eksisterer i itemList
                if (!itemService.ItemExists(item.internalId))
                {
                    // Oppretter ny item kun hvis den ikke allerede eksisterer
                    itemService.CreateItem(item.internalId, item.externalId, item.name, item.type);
                }

                // Hent eksisterende lokasjon fra ItemService
                var existingLocation = itemService.GetLocationByInternalId(item.internalId);
                var itemLocation = !string.IsNullOrWhiteSpace(existingLocation) ? existingLocation : item.location;

                try
                {
                    itemService.AddItem(item.internalId, itemLocation, DateTime.Now);
                    Console.WriteLine($"Item {item.internalId} added with location {itemLocation}.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Could not add item with internal ID {item.internalId}: {ex.Message}");
                }
            }
        }
    }
}
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
        private readonly ItemService itemService; // Legger til en referanse til ItemService

        // Konstruktør for å injisere ItemService
        public WaresInService(ItemService itemService)
        {
            this.itemService = itemService;
        }
        public WaresInService()
        {
            this.itemService = itemService;
        }

        public void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            // Sjekker om det allerede finnes en planlagt vares inn med samme orderId
            if (scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new InvalidOperationException("A wares in with this orderId is already scheduled.");
            }

            var waresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                location = location,
                Items = incomingItems // Setter inngående varer til WaresIn-objektet
            };

            scheduledWaresIns.Add(waresIn);

            // Legger til de nye varene i varehuset
            foreach (var item in incomingItems)
            {
                itemService.AddItem(item.internalId); // Antatt metode for å legge til en vare
            }
        }
    }
}
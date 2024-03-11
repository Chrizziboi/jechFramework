using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner for WaresIn.cs
    /// </summary>
    public class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> scheduledWaresIns = new List<WaresIn>();
        private readonly ItemService itemService;
        private readonly WarehouseService warehouseService; // Ny avhengighet


        public WaresInService(ItemService itemService, WarehouseService warehouseService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
        }

        public void ScheduleWaresIn(int warehouseId, int orderId, DateTime scheduledTime, int zoneId, TimeSpan processingTime, List<Item> incomingItems)
        {
            try
            {
                // Sjekk for eksistensen av varehuset
                warehouseService.FindWarehouseInWarehouseList(warehouseId,false); // Bekreft varehuset eksisterer

                if (incomingItems == null) throw new ArgumentNullException(nameof(incomingItems));
                if (scheduledWaresIns.Any(wi => wi.orderId == orderId)) throw new InvalidOperationException("A wares in with this orderId is already scheduled.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to schedule wares in: {ex.Message}");
                return; // Returnerer tidlig hvis en feil oppstår
            }

            var waresIn = new WaresIn { orderId = orderId, scheduledTime = scheduledTime, zoneId = zoneId, incomingItems = incomingItems };
            scheduledWaresIns.Add(waresIn);

            foreach (var item in incomingItems)
            {
                try
                {
                    if (!itemService.ItemExists(warehouseId,item.internalId))
                    {
                        itemService.CreateItem(warehouseId, item.internalId, item.externalId, item.name, item.type);
                    }
                    var existingZoneId = itemService.GetLocationByInternalId(warehouseId,item.internalId);
                    var itemZoneId = existingZoneId ?? zoneId;
                    itemService.AddItem(item.internalId, itemZoneId, DateTime.Now, warehouseId); // Legger til item med spesifikk warehouseId
                }

                catch (Exception ex)

                {
                    Console.WriteLine($"Failed to process item {item.internalId}: {ex.Message}");
                    // Håndter feilen på en måte som tillater at simulasjonen fortsetter, f.eks. logge feilen
                }
            }
        }
    }
}
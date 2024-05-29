using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner for WaresIn.cs
    /// </summary>
    public class WaresInService
    {
        private List<WaresIn> WaresIns = new List<WaresIn>();
        private ItemService itemService;
        private WarehouseService warehouseService; // Ny avhengighet
        private WaresOutService waresOutService; // Ny avhengighet
        private PalletService palletService;
        private Warehouse warehouse;


        public WaresInService(ItemService itemService, WarehouseService warehouseService, PalletService palletService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            this.palletService = palletService ?? throw new ArgumentNullException(nameof(palletService));

        }



        public void WaresIn(int warehouseId, int orderId, List<Item> incomingItems, DateTime scheduledTime)

        {
            try
            {
                // Sjekk for eksistensen av varehuset
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null) throw new ServiceException("Warehouse not found.");

                // Sjekker at innkommende varer ikke er null og at ordreID ikke allerede er planlagt
                if (incomingItems == null) throw new ArgumentNullException(nameof(incomingItems));
                if (WaresIns.Any(wi => wi.orderId == orderId)) throw new ServiceException("Order ID already scheduled.");

                foreach (var item in incomingItems)
                {
                    // Finn en kompatibel sone for hver vare basert på storageType
                    Zone compatibleZone = null;
                    foreach (var zone in warehouse.zoneList)
                    {
                        if (warehouseService.IsStorageTypeCompatible(zone, item))
                        {
                            compatibleZone = zone;
                            break;
                        }
                    }

                    

                    if (compatibleZone == null)
                    {
                        Console.WriteLine($"No compatible zone found for item {item.internalId} with storage type {item.storageType}.");
                        continue; // Går til neste item hvis ingen kompatibel sone ble funnet
                    }

                    // Oppretter varen i lageret hvis den ikke eksisterer
                    if (!itemService.ItemExists(warehouseId, item.internalId))
                    {
                        itemService.CreateItem(warehouseId, item.internalId, item.externalId, item.name, item.storageType);
                    }
                    var existingZoneId = itemService.GetLocationByInternalId(warehouseId, item.internalId);
                    var itemZoneId = existingZoneId ?? compatibleZone.zoneId;
                    itemService.AddItem(warehouseId, compatibleZone.zoneId, item.internalId, scheduledTime, item.quantity); // Legger til item med spesifikk warehouseId

                    palletService.AddPallets(incomingItems);

                    // Legg til varen i den kompatible sonen
                    //itemService.AddItem(item.internalId, compatibleZone.zoneId, scheduledTime, warehouseId, item.quantity);

                    /*
                    if (!itemService.ItemExists(warehouseId, item.internalId))
                    {
                        itemService.CreateItem(warehouseId, item.internalId, item.externalId, item.name, item.storageType);
                    }
                    var existingZoneId = itemService.GetLocationByInternalId(warehouseId, item.internalId);
                    var itemZoneId = existingZoneId ?? compatibleZone.zoneId;
                    itemService.AddItem(item.internalId, itemZoneId, DateTime.Now, warehouseId); // Legger til item med spesifikk warehouseId
                    */
                }

            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Failed to process WaresIn: {ex.Message}");
                // Håndterer feilen på en måte som tillater at simulasjonen fortsetter, for eksempel logg feilen
            }
        }

        public void ScheduleWaresIn(int warehouseId, int orderId, List<Item> incomingItems, DateTime scheduledTime, RecurrencePattern frequency)
        {
            try
            {
                // Sjekk for eksistensen av varehuset
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null) throw new ServiceException("Warehouse not found.");

                // Sjekker at innkommende varer ikke er null og at ordreID ikke allerede er planlagt
                if (incomingItems == null) throw new ArgumentNullException(nameof(incomingItems));
                if (WaresIns.Any(wi => wi.orderId == orderId)) throw new ServiceException("Order ID already scheduled.");

                foreach (var item in incomingItems)
                {
                    // Finn en kompatibel sone for hver vare basert på storageType
                    Zone compatibleZone = null;
                    foreach (var zone in warehouse.zoneList)
                    {
                        if (warehouseService.IsStorageTypeCompatible(zone, item))
                        {
                            compatibleZone = zone;
                            break;
                        }
                    }

                    if (compatibleZone == null)
                    {
                        Console.WriteLine($"No compatible zone found for item {item.internalId} with storage type {item.storageType}.");
                        continue; // Går til neste item hvis ingen kompatibel sone ble funnet
                    }

                    // Oppretter varen i lageret hvis den ikke eksisterer
                    if (!itemService.ItemExists(warehouseId, item.internalId))
                    {
                        itemService.CreateItem(warehouseId, item.internalId, item.externalId, item.name, item.storageType);
                    }
                    var existingZoneId = itemService.GetLocationByInternalId(warehouseId, item.internalId);
                    var itemZoneId = existingZoneId ?? compatibleZone.zoneId;
                    itemService.AddItem(warehouseId, compatibleZone.zoneId, item.internalId, scheduledTime, item.quantity); // Legger til item med spesifikk warehouseId


                }

                palletService.AddPallets(incomingItems);

                // Planlegg neste forekomst basert på frekvensen
                switch (frequency)
                {
                    case RecurrencePattern.Daily:
                        ScheduleNextOccurrence(orderId, scheduledTime.AddDays(1));
                        break;
                    case RecurrencePattern.Weekly:
                        ScheduleNextOccurrence(orderId, scheduledTime.AddDays(7));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(frequency), "Invalid frequency type.");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            
        }
        private void ScheduleNextOccurrence(int orderId, DateTime nextScheduledTime)
                    {
                        // Logikk for å planlegge neste forekomst av ordreinnkommende varer
                        Console.WriteLine($"Next occurrence for order {orderId} scheduled at {nextScheduledTime}");
                        // Her kan du implementere logikken for å faktisk planlegge den neste forekomsten
                    }
    }
}
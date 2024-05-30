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
        private WarehouseService warehouseService;
        private WaresOutService waresOutService; 
        private PalletService palletService;
        private Warehouse warehouse;

        /// <summary>
        /// Initialisereren ny instanse av WaresInService klassen.
        /// </summary>
        /// <param name="itemService">Tjeneste for håndtering av varer.</param>
        /// <param name="warehouseService">Tjeneste for håndtering av varehus.</param>
        /// <param name="palletService">Tjeneste for håndtering av paller.</param>
        public WaresInService(
            ItemService itemService, 
            WarehouseService warehouseService, 
            PalletService palletService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            this.palletService = palletService ?? throw new ArgumentNullException(nameof(palletService));

        }

        /// <summary>
        /// Behandler innkommende varer til et spesifisert varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="orderId">Ordre ID for de innkommende varene.</param>
        /// <param name="incomingItems">Liste over innkommende varer.</param>
        /// <param name="scheduledTime">Planlagt tidspunkt for mottak av varene.</param>
        /// <exception cref="ServiceException">Kastes når det oppstår en tjenesterelatert feil.</exception>
        /// <exception cref="ArgumentNullException">Kastes når outgoingItems er null.</exception>
        public void WaresIn(
            int warehouseId,
            int orderId,
            List<Item> incomingItems,
            DateTime scheduledTime)
        {
            try
            {
                // Sjekk for eksistensen av varehuset
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);

                if (warehouse == null)
                {
                    throw new ServiceException("Warehouse not found.");
                }

                // Sjekker at innkommende varer ikke er null og at ordreID ikke allerede er planlagt
                if (incomingItems == null)
                {
                    throw new ArgumentNullException(nameof(incomingItems));
                }

                if (WaresIns.Any(wi => wi.orderId == orderId))
                {
                    throw new ServiceException("Order ID already scheduled.");
                }

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
                        throw new ServiceException($"No compatible zone found for item {item.internalId} with storage type {item.storageType}.");
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

            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Failed to process WaresIn: {ex.Message}");
                throw; // Kaster unntaket på nytt for å sikre at det blir håndtert høyere opp hvis nødvendig
            }
        }

        /// <summary>
        /// Planlegger innkommende varer til et spesifisert varehus med gjentakelser.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="orderId">Ordre ID for de innkommende varene.</param>
        /// <param name="incomingItems">Liste over innkommende varer.</param>
        /// <param name="scheduledTime">Planlagt tidspunkt for mottak av varene.</param>
        /// <param name="frequency">Gjentakelsesmønster for innkommende varer.</param>
        /// <exception cref="ServiceException">Kastes når det oppstår en tjenesterelatert feil.</exception>
        /// <exception cref="ArgumentNullException">Kastes når incomingItems er null.</exception>
        public void ScheduleWaresIn(
            int warehouseId,
            int orderId,
            List<Item> incomingItems,
            DateTime scheduledTime,
            ScheduleType frequency)
        {
            try
            {
                // Sjekk for eksistensen av varehuset
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);

                if (warehouse == null)
                {
                    throw new ServiceException("Warehouse not found.");
                }

                // Sjekker at innkommende varer ikke er null og at ordreID ikke allerede er planlagt
                if (incomingItems == null)
                {
                    throw new ArgumentNullException(nameof(incomingItems));
                }

                if (WaresIns.Any(wi => wi.orderId == orderId))
                {
                    throw new ServiceException("Order ID already scheduled.");
                }

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
                        throw new ServiceException($"No compatible zone found for item {item.internalId} with storage type {item.storageType}.");
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
                    case ScheduleType.Daily:
                        ScheduleNextOccurrence(orderId, scheduledTime.AddDays(1));
                        break;
                    case ScheduleType.Weekly:
                        ScheduleNextOccurrence(orderId, scheduledTime.AddDays(7));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(frequency), "Invalid frequency type.");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Kaster unntaket på nytt for å sikre at det blir håndtert høyere opp hvis nødvendig
            }
        }


        /// <summary>
        /// Planlegger neste forekomst av innkommende varer basert på ordreID og tidspunkt.
        /// </summary>
        /// <param name="orderId">Ordre ID for de innkommende varene.</param>
        /// <param name="nextScheduledTime">Neste planlagte tidspunkt for mottak av varene.</param>
        private void ScheduleNextOccurrence(
            int orderId, 
            DateTime nextScheduledTime)
                    {
                        // Logikk for å planlegge neste forekomst av ordreinnkommende varer
                        Console.WriteLine($"Next occurrence for order {orderId} scheduled at {nextScheduledTime}.");
                        // Her kan du implementere logikken for å faktisk planlegge den neste forekomsten
                    }
    }
}
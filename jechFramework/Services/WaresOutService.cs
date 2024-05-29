using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{

    /// <summary>
    /// Tjeneste for å håndtere utsending av varer fra lageret.
    /// </summary>
    public class WaresOutService
    {
        private List<WaresOut> scheduledWaresOuts = new List<WaresOut>();
        private readonly ItemService itemService; // Assuming there is an ItemService to handle items in the warehouse
        private readonly WarehouseService warehouseService; // Ny avhengighet
        private readonly Shelf shelf = new(); // Ny avhengighet
        private readonly PalletService palletService;

        public int lastShipmentNumber = 0;

        /// <summary>
        /// Delegat for hendelse når varer er planlagt for utsending.
        /// </summary>
        public delegate void WaresOutScheduledEventHandler(
            int warehouseId,
            int orderId,
            DateTime scheduledTime,
            string destination,
            List<Item> outgoingItems,
            int lastShipmentNumber);

        /// <summary>
        /// Hendelse som utløses når varer er planlagt for utsending.
        /// </summary>
        public event EventHandler<WaresOutEventArgs> WaresOutScheduledSentOut;

        /// <summary>
        /// Utløser hendelsen WaresOutScheduledSentOut.
        /// </summary>
        /// <param name="warehouseId">ID-en til lageret.</param>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="scheduledTime">Tidspunktet for planlagt utsending.</param>
        /// <param name="destination">Destinasjonen for varene.</param>
        /// <param name="outgoingItems">Liste over varer som skal sendes ut.</param>
        /// <param name="lastShipmentNumber">Siste sending nummer.</param>
        public void OnWaresOutScheduledSentOut(
            int warehouseId, 
            int orderId, 
            DateTime scheduledTime, 
            string destination, 
            List<Item> outgoingItems, 
            int lastShipmentNumber)
        {
            WaresOutScheduledSentOut?.Invoke(this, new WaresOutEventArgs(
                warehouseId, 
                orderId, 
                scheduledTime, 
                destination, 
                outgoingItems, 
                lastShipmentNumber));
        }

        /// <summary>
        /// Konstruktør for WaresOutService.
        /// </summary>
        /// <param name="itemService">Instans av ItemService.</param>
        /// <param name="palletService">Instans av PalletService.</param>
        public WaresOutService(ItemService itemService, PalletService palletService)
        {
            this.itemService = itemService;
            this.palletService = palletService ?? throw new ArgumentNullException(nameof(palletService));
        }

        /// <summary>
        /// Planlegger utsending av varer fra et lager til en destinasjon på et gitt tidspunkt.
        /// </summary>
        /// <param name="warehouseId">ID-en til lageret.</param>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="destination">Destinasjonen for varene.</param>
        /// <param name="outgoingItems">Liste over varer som skal sendes ut.</param>
        /// <param name="scheduledTime">Tidspunktet for planlagt utsending.</param>
        /// <exception cref="InvalidOperationException">Kastes hvis ItemService ikke er initialisert.</exception>
        /// <exception cref="ArgumentNullException">Kastes hvis listen over utgående varer er null.</exception>
        /// <exception cref="ServiceException">Kastes hvis ordren allerede er planlagt eller hvis ingen varer kunne behandles på grunn av lagerbegrensninger.</exception>
        public void WaresOut(
            int warehouseId, 
            int orderId, 
            string destination, 
            List<Item> outgoingItems, 
            DateTime scheduledTime)
        {

            if (itemService == null)
            {
                throw new InvalidOperationException("ItemService is not initialized.");
            }

            if (outgoingItems == null)
            {
                throw new ArgumentNullException(nameof(outgoingItems));
            }

            if (scheduledWaresOuts == null)
            {
                scheduledWaresOuts = new List<WaresOut>();
            }

            if (scheduledWaresOuts.Any(wo => wo.orderId == orderId))
            {
                throw new ServiceException($"Wares out with orderId {orderId} is already scheduled.");
            }

            List<Item> successfullyRemovedItems = new List<Item>();

            foreach (var item in outgoingItems)
            {
                var quantityAvailable = itemService.FindItemQuantityInWarehouse(warehouseId, item.internalId);

                if (quantityAvailable < item.quantity)
                {
                    Console.WriteLine($"Not enough stock for item {item.internalId}. Needed: {item.quantity}, Available: {quantityAvailable}.");
                    continue; // Skip this item but continue with others
                }

                itemService.RemoveItem(warehouseId, item.internalId, item.quantity);
                successfullyRemovedItems.Add(item); // Track successfully processed items
            }

            if (!successfullyRemovedItems.Any())
            {
                throw new ServiceException("No items could be processed for this order due to stock limitations.");
            }

            lastShipmentNumber++;
            palletService.RemovePallets(outgoingItems);

            var waresOut = new WaresOut
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                destination = destination,
                items = successfullyRemovedItems // Only include successfully processed items
            };

            scheduledWaresOuts.Add(waresOut);
            OnWaresOutScheduledSentOut(warehouseId, orderId, scheduledTime, destination, successfullyRemovedItems, lastShipmentNumber);
        }

        /// <summary>
        /// Planlegger utsending av varer med en gitt frekvens (daglig eller ukentlig).
        /// </summary>
        /// <param name="warehouseId">ID-en til lageret.</param>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="destination">Destinasjonen for varene.</param>
        /// <param name="outgoingItems">Liste over varer som skal sendes ut.</param>
        /// <param name="scheduledTime">Tidspunktet for planlagt utsending.</param>
        /// <param name="frequency">Frekvensen for utsending (daglig eller ukentlig).</param>
        /// <exception cref="InvalidOperationException">Kastes hvis ItemService ikke er initialisert.</exception>
        /// <exception cref="ArgumentNullException">Kastes hvis listen over utgående varer er null.</exception>
        /// <exception cref="ServiceException">Kastes hvis ordren allerede er planlagt eller hvis ingen varer kunne behandles på grunn av lagerbegrensninger.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Kastes hvis frekvensen for utsending er ugyldig.</exception>

        public void ScheduleWaresOut(
            int warehouseId, 
            int orderId, 
            string destination, 
            List<Item> outgoingItems, 
            DateTime scheduledTime, 
            ScheduleType frequency)
        {
            if (itemService == null)
            {
                throw new InvalidOperationException("ItemService is not initialized.");
            }

            if (outgoingItems == null)
            {
                throw new ArgumentNullException(nameof(outgoingItems));
            }

            if (scheduledWaresOuts == null)
            {
                scheduledWaresOuts = new List<WaresOut>();
            }

            if (scheduledWaresOuts.Any(wo => wo.orderId == orderId))
            {
                throw new ServiceException($"Wares out with orderId {orderId} is already scheduled.");
            }

            List<Item> successfullyRemovedItems = new List<Item>();

            foreach (var item in outgoingItems)
            {
                var quantityAvailable = itemService.FindItemQuantityInWarehouse(warehouseId, item.internalId);

                if (quantityAvailable < item.quantity)
                {
                    Console.WriteLine($"Not enough stock for item {item.internalId}. Needed: {item.quantity}, Available: {quantityAvailable}.");
                    continue; // Skip this item but continue with others
                }

                itemService.RemoveItem(warehouseId, item.internalId, item.quantity);
                successfullyRemovedItems.Add(item); // Track successfully processed items
            }

            if (!successfullyRemovedItems.Any())
            {
                throw new ServiceException("No items could be processed for this order due to stock limitations.");
            }

            lastShipmentNumber++;

            palletService.RemovePallets(outgoingItems);

            var waresOut = new WaresOut
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                destination = destination,
                items = successfullyRemovedItems // Only include successfully processed items
            };

            scheduledWaresOuts.Add(waresOut);
            ScheduledWaresOutPrint(warehouseId, orderId, scheduledTime, destination, successfullyRemovedItems, lastShipmentNumber);

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

        /// <summary>
        /// Planlegger neste forekomst av ordreutgående varer.
        /// </summary>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="nextScheduledTime">Tidspunktet for neste planlagte utsending.</param>
        private void ScheduleNextOccurrence(int orderId, DateTime nextScheduledTime)
        {
            // Logikk for å planlegge neste forekomst av ordreutgående varer
            Console.WriteLine($"Next occurrence for order {orderId} scheduled at {nextScheduledTime}.");
            // Her kan du implementere logikken for å faktisk planlegge den neste forekomsten
        }

        /// <summary>
        /// Skriver ut informasjon når varer er planlagt for utsending.
        /// </summary>
        /// <param name="warehouseId">ID-en til lageret.</param>
        /// <param name="orderId">ID-en til ordren.</param>
        /// <param name="scheduledTime">Tidspunktet for planlagt utsending.</param>
        /// <param name="destination">Destinasjonen for varene.</param>
        /// <param name="items">Liste over varer som skal sendes ut.</param>
        /// <param name="shipmentNumber">Forsendelsesnummeret.</param>
        private void ScheduledWaresOutPrint(
            int warehouseId, 
            int orderId, 
            DateTime scheduledTime, 
            string destination, 
            List<Item> items, 
            int shipmentNumber)
        {
            // Logikk for å håndtere når varer er planlagt for utsending
            Console.WriteLine($"Wares out scheduled for warehouse {warehouseId}, order {orderId}, destination {destination}, scheduled at {scheduledTime}, shipment number {shipmentNumber}.");
        }
    }
}
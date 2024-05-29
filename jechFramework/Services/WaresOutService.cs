using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    public class WaresOutService
    {
        private List<WaresOut> scheduledWaresOuts = new List<WaresOut>();
        private ItemService itemService; 
        private WarehouseService warehouseService; 
        private Shelf shelf = new(); 
        private PalletService palletService;

        public int lastShipmentNumber = 0;

        public delegate void WaresOutScheduledEventHandler(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems, int lastShipmentNumber);

        public event EventHandler<WaresOutEventArgs> WaresOutScheduledSentOut;
        
        public void OnWaresOutScheduledSentOut(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems, int lastShipmentNumber)
        {
            WaresOutScheduledSentOut?.Invoke(this, new WaresOutEventArgs(warehouseId, orderId, scheduledTime, destination, outgoingItems, lastShipmentNumber));
        }

        public WaresOutService(ItemService itemService, WarehouseService warehouseService, PalletService palletService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.warehouseService = warehouseService ?? throw new ArgumentNullException(nameof(warehouseService));
            this.palletService = palletService ?? throw new ArgumentNullException(nameof(palletService));
        }

        /// <summary>
        /// Metoden ScheduleWaresOut er designet for å håndtere planlegging av varer som skal sendes ut fra en lokasjon til 
        /// en bestemt destinasjon på et gitt tidspunkt. Den tar i bruk flere inputparametere for å utføre denne oppgaven og 
        /// utfører flere sjekker for å sikre at prosessen kan utføres korrekt. 
        /// </summary>
        /// <param name="orderId">En unik identifikator for ordren som skal sendes ut.</param>
        /// <param name="scheduledTime">Det spesifikke tidspunktet varene er planlagt å sendes ut på.</param>
        /// <param name="destination">Destinasjonen hvor varene skal sendes.</param>
        /// <param name="outgoingItems">En liste over Item-objekter som representerer de utgående varene.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        public void WaresOut(int warehouseId, int orderId, string destination, List<Item> outgoingItems, DateTime scheduledTime)
        {
            
            if (itemService == null)
            {
                throw new InvalidOperationException("ItemService is not initialized.");
            }
            
            if (outgoingItems == null) throw new ArgumentNullException(nameof(outgoingItems));
            if (scheduledWaresOuts == null) scheduledWaresOuts = new List<WaresOut>();

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
                    continue; 
                }

                itemService.RemoveItem(warehouseId, item.internalId, item.quantity);
                successfullyRemovedItems.Add(item); 
            }

            if (!successfullyRemovedItems.Any())
            {
                throw new ServiceException("No items could be processed for this order due to stock limitations.");
            }
            palletService.RemovePallets(successfullyRemovedItems);

            lastShipmentNumber++;

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

    }
}


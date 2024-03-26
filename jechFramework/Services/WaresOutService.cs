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
        private readonly WarehouseService warehouseService; // Ny avhengighet
        private readonly Shelf shelf = new(); // Ny avhengighet

        public int lastShipmentNumber = 0;

        public List<Pallet> palletList = new();

        public delegate void WaresOutScheduledEventHandler(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems, int lastShipmentNumber);

        public event EventHandler<WaresOutEventArgs> WaresOutScheduledSentOut;
        
        public void OnWaresOutScheduledSentOut(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems, int lastShipmentNumber)
        {
            WaresOutScheduledSentOut?.Invoke(this, new WaresOutEventArgs(warehouseId, orderId, scheduledTime, destination, outgoingItems, lastShipmentNumber));
        }

        public WaresOutService() 
        { }

        // Constructor to inject ItemService
        public WaresOutService(ItemService itemService)
        {
            this.itemService = itemService;
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
        public void WaresOut(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Models.Item> outgoingItems)
        {
            try
            {
                if (outgoingItems == null) throw new ArgumentNullException(nameof(outgoingItems));
                if (scheduledWaresOuts.Any(wo => wo.orderId == orderId))
                {
                    throw new ServiceException($"Wares out with orderId {orderId} is already scheduled.");
                }

                foreach (var item in outgoingItems)
                {
                    try
                    {
                        var quantityAvailable = itemService.FindHowManyItemQuantityByInternalId(warehouseId, item.internalId);
                        if (quantityAvailable <= 0)
                        {
                            throw new ServiceException($"Item with internal ID {item.internalId} is unavailable.");
                        }

                        // Antatt at RemoveItem nå krever warehouseId og internalId
                        //itemService.RemoveItem(warehouseId, item.internalId, item.quantity);

                        //if(item.quantity % 30 == 0)
                        //{
                        //    PalletService.removePallet(palletList);
                        //}
                    }
                    catch (ServiceException ex)
                    {
                        Console.WriteLine($"Failed to process item {item.internalId}: {ex.Message}");
                        // Hopper over fjerning av denne varen, men fortsetter prosessen for de andre varene
                        continue;
                    }
                }

                lastShipmentNumber++;

                var waresOut = new WaresOut
                {
                    orderId = orderId,
                    scheduledTime = scheduledTime,
                    destination = destination,
                    items = outgoingItems
                };

                scheduledWaresOuts.Add(waresOut);



                OnWaresOutScheduledSentOut(warehouseId, orderId, scheduledTime, destination, outgoingItems, lastShipmentNumber);

            }
            catch (ServiceException ex)
            {
                // Logger feil for hele ScheduleWaresOut operasjonen og fortsetter
                Console.WriteLine($"An error occurred while scheduling wares out:  {ex.Message}");
            }
        }
    }
}


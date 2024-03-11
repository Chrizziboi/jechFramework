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
        /// <exception cref="InvalidOperationException"></exception>
        public void ScheduleWaresOut(int warehouseId, int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems)
        {
            if (outgoingItems == null) throw new ArgumentNullException(nameof(outgoingItems));
            if (scheduledWaresOuts.Any(wo => wo.OrderId == orderId))
            {
                throw new InvalidOperationException("A wares out with this orderId is already scheduled.");
            }

            // Sjekker lagerbeholdningen for hvert utgående vareelement og forbereder for fjerning
            foreach (var item in outgoingItems)
            {
                var quantityAvailable = itemService.FindHowManyItemQuantityByInternalId(warehouseId, item.internalId);
                if (quantityAvailable <= 0)
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

            // Fjerner de utgående varene fra lageret basert på tilgjengelig mengde
            foreach (var item in outgoingItems)
            {
                // Antatt at RemoveItem nå krever warehouseId og internalId
                itemService.RemoveItem(warehouseId, item.internalId);
            }
        }

    }
}


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

        /// <summary>
        /// Metoden ScheduleWaresOut er designet for å håndtere planlegging av varer som skal sendes ut fra en lokasjon til 
        /// en bestemt destinasjon på et gitt tidspunkt. Den tar i bruk flere inputparametere for å utføre denne oppgaven og 
        /// utfører flere sjekker for å sikre at prosessen kan utføres korrekt. 
        /// </summary>
        /// <param name="orderId">En unik identifikator for ordren som skal sendes ut.</param>
        /// <param name="scheduledTime">Det spesifikke tidspunktet varene er planlagt å sendes ut på.</param>
        /// <param name="destination">Destinasjonen hvor varene skal sendes.</param>
        /// <param name="outgoingItems">En liste over Item-objekter som representerer de utgående varene.</param>
        /// <exception cref="ServiceException"></exception> //ArgnullException
        /// <exception cref="ServiceException"></exception>
        public void ScheduleWaresOut(int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems)
        {
            if (outgoingItems == null) throw new ServiceException(nameof(outgoingItems)); //ArgnullException

            if (scheduledWaresOuts.Any(wo => wo.OrderId == orderId))
            {
                throw new ServiceException("A wares out with this orderId is already scheduled.");
            }

            // Sjekker lagerbeholdningen for hvert utgående vareelement
            foreach (var item in outgoingItems)
            {
                if (itemService.FindHowManyItemQuantityByInternalId(item.internalId) <= 0)
                {
                    throw new ServiceException($"Item with internal ID {item.internalId} is unavailable.");
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


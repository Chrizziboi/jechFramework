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
        private readonly ItemService itemService; // Beholder referansen til ItemService
        private readonly Warehouse warehouse;

        // Konstruktør for å injisere ItemService
        /// <summary>
        /// Tar imot et objekt av typen "ItemService" som parameter. Om den er null kaster den en "ServiceException", hvis "ikke null", tilordnes den til "this.itemService"
        /// </summary>
        /// <param name="itemService"> Objektet som gir tjenester knyttet til elementer (varer).</param>
        /// <exception cref="ServiceException"> Kastes hvis 'itemService' er null. </exception> //ArgnullException
        public WaresInService(ItemService itemService)
        {
            this.itemService = itemService ?? throw new ServiceException(nameof(itemService)); //ArgNullException
        }
        /// <summary>
        /// Metoden ScheduleWaresIn planlegger ankomst av varer ved å ta inn ordre-ID, tidspunkt, lokasjon, behandlingstid, 
        /// og en vareliste. Den sjekker først for null-verdier og eksisterende ordre-ID for å forhindre duplikater. Om gyldig, 
        /// opprettes et WaresIn-objekt med informasjonen og legges til i en samling, og deretter legges hver vare til i 
        /// varelageret.
        /// </summary>
        /// <param name="orderId">En unik identifikator for ordren som skal planlegges for vareinnsendelse.</param>
        /// <param name="scheduledTime">Det spesifikke tidspunktet varene er planlagt å ankomme på.</param>
        /// <param name="location">Stedet hvor varene skal ankomme.</param>
        /// <param name="processingTime">Tiden det tar å behandle varene.</param>
        /// <param name="incomingItems">En liste over Item-objekter som representerer de inngående varene.</param>
        /// <exception cref="ServiceException">-</exception> //ArgnullException
        /// <exception cref="ServiceException">Forteller at en ordre med samme nummer er allerede laget.</exception>
        public void ScheduleWaresIn(int orderId, DateTime scheduledTime, int zoneId, TimeSpan processingTime, List<Item> incomingItems)
        {
            if (incomingItems == null) throw new ServiceException(nameof(incomingItems)); //ArgnullExceptions
            if (scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new ServiceException("A wares in with this orderId is already scheduled.");
            }
        
            var waresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                zoneId = zoneId,
                incomingItems = incomingItems
            };

            scheduledWaresIns.Add(waresIn);

            foreach (var item in incomingItems)
            {
                if (!itemService.ItemExists(item.internalId))
                {
                    itemService.CreateItem(item.internalId, item.externalId, item.name, item.type);
                }

                // Henter eksisterende zoneId for varen. Returnerer null hvis ingen sone er tildelt
                var existingZoneId = itemService.GetLocationByInternalId(item.internalId);

                // Bruker eksisterende zoneId hvis den ikke er null, ellers bruker den medfølgende zoneId
                var itemZoneId = existingZoneId ?? zoneId;

                try
                {
                    // Antar at du har tilgang til en gyldig warehouseId her
                    
                    itemService.AddItem(item.internalId, itemZoneId, DateTime.Now, warehouse.warehouseId);
                    Console.WriteLine($"Item {item.internalId} added to zone {itemZoneId} in warehouse {warehouse.warehouseId}.");
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Could not add item with internal ID {item.internalId}: {ex.Message}");
                }
            }
        }
    }
}
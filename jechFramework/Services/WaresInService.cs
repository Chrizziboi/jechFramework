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

        // Konstruktør for å injisere ItemService
        /// <summary>
        /// Tar imot et objekt av typen "ItemService" som parameter. Om den er null kaster den en "ArgumentNullException", hvis "ikke null", tilordnes den til "this.itemService"
        /// </summary>
        /// <param name="itemService"> Objektet som gir tjenester knyttet til elementer (varer).</param>
        /// <exception cref="ArgumentNullException"> Kastes hvis 'itemService' er null. </exception>
        public WaresInService(ItemService itemService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
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
        /// <exception cref="ArgumentNullException">-</exception>
        /// <exception cref="InvalidOperationException">Forteller at en ordre med samme nummer er allerede laget.</exception>
        public void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            if (incomingItems == null) throw new ArgumentNullException(nameof(incomingItems));
            if (scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new InvalidOperationException("A wares in with this orderId is already scheduled.");
            }
        
            var waresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                location = location,
                incomingItems = incomingItems
            };

            scheduledWaresIns.Add(waresIn);

            foreach (var item in incomingItems)
            {
                // Sjekker om item allerede eksisterer i itemList
                if (!itemService.ItemExists(item.internalId))
                {
                    // Oppretter ny item kun hvis den ikke allerede eksisterer
                    itemService.CreateItem(item.internalId, item.externalId, item.name, item.type);
                }

                // Hent eksisterende lokasjon fra ItemService
                var existingLocation = itemService.GetLocationByInternalId(item.internalId);
                var itemLocation = !string.IsNullOrWhiteSpace(existingLocation) ? existingLocation : item.location;

                try
                {
                    itemService.AddItem(item.internalId, itemLocation, DateTime.Now);
                    Console.WriteLine($"Item {item.internalId} added with location {itemLocation}.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Could not add item with internal ID {item.internalId}: {ex.Message}");
                }
            }
        }
    }
}
using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    public class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> scheduledWaresIns = new List<WaresIn>();
        private readonly ItemService itemService; // Beholder referansen til ItemService

        // Konstruktør for å injisere ItemService
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

            // Sjekker om det allerede finnes en planlagt vares inn med samme orderId
            if (scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new InvalidOperationException("A wares in with this orderId is already scheduled.");
            }

            var waresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                location = location,
                Items = incomingItems // Setter inngående varer til WaresIn-objektet
            };

            scheduledWaresIns.Add(waresIn);

            // Legger til de nye varene i varehuset
            foreach (var item in incomingItems)
            {
                itemService.AddItem(item); // Antatt metode for å legge til en vare
                
            }
        }
    }
}
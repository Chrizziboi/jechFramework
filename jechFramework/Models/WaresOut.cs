using jechFramework.Models;
using jechFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Models
{
    public class WaresOut
    {
        public int deliveryId { get; set; } 
        // Unik utlevering-ID
        public DateTime scheduledTime { get; set; } 
        // Planlagt tid for utlevering
        public Zone storageZone { get; set; } 
        // Referanse til lagerets sone
        public TimeSpan processingTime { get; set; } 
        // Tidsforbruk fra plassering til utlevering
        private ItemService _itemService; 
        // Tjeneste for håndtering av varer

        // Konstruktør som mottar ItemService
        public WaresOut(ItemService itemService)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        // Metode for å fjerne varen fra lageret ved hjelp av ItemService
        public void RemoveItemFromWarehouse(int internalId)
        {
            _itemService.RemoveItem(internalId);
        }
    }

    public class WareItem
    {
        public int externalId { get; set; }
        // Ekstern ID fra 'Item'-klassen
        public int internalId { get; set; }
        // Intern ID fra 'Item'-klassen
    }



    /// <summary>
    /// Representerer en plan for gjentagende hentinger av varer fra lageret.
    /// Inkluderer starttidspunkt og mønsteret for gjentakelse (f.eks., daglig eller ukentlig).
    /// </summary>
    public class RecurringWaresOut
    {
        public int deliveryId { get; set; }
        public int internalId { get; set; }
        public DateTime startTime { get; set; }
        public RecurrencePattern recurrencePattern { get; set; }
    }

    /// <summary>
    /// Definerer ulike mønstre for gjentakelse av hentinger.
    /// </summary>
    public enum RecurrencePattern
    {
        Daily,   // Henting skjer hver dag.
        Weekly   // Henting skjer ukentlig.
    }
}
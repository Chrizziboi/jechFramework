using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    /// <summary>
    /// Representerer en enkeltgangshenting av en vare fra lageret.
    /// Inneholder all nødvendig informasjon for å identifisere og planlegge hentingen.
    /// </summary>
    public class WaresOut
    {
        public int DeliveryId { get; set; }
        // Unik utlevering-ID

        public DateTime ScheduledTime { get; set; }
        // Planlagt tid for utlevering
        public Zone storageZone { get; set; }
        // Referanse til lagerets sone
        public TimeSpan ProcessingTime { get; set; }
        // Tidsforbruk fra plassering til utlevering

        public List<Item> itemList;
        // Liste over varer i utleveringen

        public void RemoveItemFromWarehouse(int internalId)
        {
            
            WareItem itemToRemove = Items.Find(item => item.InternalId == internalId);
            // Finn varen basert på internalId i Items-listen for varer i utleveringen

           
            if (itemToRemove != null)
            // Sjekk om varen er funnet
            {
                
                Items.Remove(itemToRemove);
                // Fjern varen fra listen

                // Oppdater lagerets kvantitet eller gjør andre nødvendige endringer
                // (avhengig av hvordan du har implementert lageret)
            }
            else
            {
                throw new ArgumentException($"Vare med InternalId {internalId} ble ikke funnet i utleveringen.");
            }
        }
    }

    public class WareItem
    {
        public int ExternalId { get; set; }
        // Ekstern ID fra 'Item'-klassen
        public int InternalId { get; set; }
        // Intern ID fra 'Item'-klassen
    }



    /// <summary>
    /// Representerer en plan for gjentagende hentinger av varer fra lageret.
    /// Inkluderer starttidspunkt og mønsteret for gjentakelse (f.eks., daglig eller ukentlig).
    /// </summary>
    public class RecurringWaresOut
    {
        public int DeliveryId { get; set; }
        public int InternalId { get; set; }
        public DateTime StartTime { get; set; }
        public RecurrencePattern RecurrencePattern { get; set; }
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
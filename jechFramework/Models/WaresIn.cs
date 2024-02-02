using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    /// <summary>
    /// Representerer en enkeltgangs innkomst av varer til lageret.
    /// Inneholder all nødvendig informasjon for å identifisere og planlegge mottak av varer.
    /// </summary>
    public class WaresIn
    {
        public int OrderId { get; set; } // Unik ordre-ID
        public List<WareItem> Items { get; set; } // Liste over varer i ordren
        public DateTime ScheduledTime { get; set; } // Planlagt tidspunkt for mottak
        public Zone StorageZone { get; set; } // Referanse til lagerets sone
        public TimeSpan ProcessingTime { get; set; } // Tidsforbruk fra mottak til plassering
    }

    public class WareItem
    {
        public int ExternalId { get; set; } // Ekstern ID fra 'Item'-klassen
        public int InternalId { get; set; } // Intern ID fra 'Item'-klassen
        // Du kan legge til flere egenskaper som er relevante for hver vare
    }

    /// <summary>
    /// Representerer en plan for gjentagende mottak av varer til lageret.
    /// Inkluderer starttidspunkt og mønsteret for gjentakelse (f.eks., daglig eller ukentlig).
    /// </summary>
    public class RecurringOrder
    {
        public int OrderId { get; set; }
        public int InternalId { get; set; }
        public DateTime StartTime { get; set; }
        public RecurrencePattern RecurrencePattern { get; set; }
    }

    /// <summary>
    /// Definerer ulike mønstre for gjentakelse av varemottak.
    /// </summary>
    public enum RecurrencePattern
    {
        Daily,   // Mottak skjer hver dag.
        Weekly   // Mottak skjer ukentlig.
    }
}
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
        public int OrderId { get; set; }
        public int InternalId { get; set; }
        public DateTime ScheduledTime { get; set; }
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
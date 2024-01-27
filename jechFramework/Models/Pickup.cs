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
    public class Pickup
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime ScheduledTime { get; set; }
    }

    /// <summary>
    /// Representerer en plan for gjentagende hentinger av varer fra lageret.
    /// Inkluderer starttidspunkt og mønsteret for gjentakelse (f.eks., daglig eller ukentlig).
    /// </summary>
    public class RecurringPickup
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
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

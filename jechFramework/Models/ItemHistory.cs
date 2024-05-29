using System;

namespace jechFramework.Models
{
    /// <summary>
    /// ItemHistory klassen er laget for å kunne opprette historikk for varer. 
    /// </summary>
    public class ItemHistory
    {
        /// <summary>
        /// Henter total tid for flyttingen.
        /// </summary>
        private TimeSpan totalTime;

        /// <summary>
        /// Henter den interne ID-en som identifiserer elementet.
        /// </summary>
        public int internalId { get; private set; }

        /// <summary>
        /// Henter den gamle plasseringen av elementet. Kan være null.
        /// </summary>
        public int? oldZone { get; private set; } = null;

        /// <summary>
        /// Henter den nye plasseringen av elementet.
        /// </summary>
        public int newZone { get; private set; }

        /// <summary>
        /// Henter tidspunktet for endringen.
        /// </summary>
        public DateTime dateTime { get; private set; }

        /// <summary>
        /// Konstruktør for å opprette et nytt objekt av typen ItemHistory.
        /// </summary>
        /// <param name="internalId">Intern ID som identifiserer elementet.</param>
        /// <param name="oldZone">Gammel plassering av elementet. Kan være null.</param>
        /// <param name="newZone">Ny plassering av elementet.</param>
        /// <param name="dateTime">Tidspunktet for endringen.</param>
        public ItemHistory(int internalId, int? oldZone, int newZone, DateTime dateTime)
        {
            this.internalId = internalId;
            this.oldZone = oldZone;
            this.newZone = newZone;
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Konstruktør for å opprette et nytt objekt av typen ItemHistory med total tid inkludert.
        /// </summary>
        /// <param name="internalId">Intern ID som identifiserer elementet.</param>
        /// <param name="oldZone">Gammel plassering av elementet. Kan være null.</param>
        /// <param name="newZone">Ny plassering av elementet.</param>
        /// <param name="dateTime">Tidspunktet for endringen.</param>
        /// <param name="totalTime">Total tid for flyttingen.</param>
        public ItemHistory(
            int internalId, 
            int? oldZone, 
            int newZone,
            DateTime dateTime, 
            TimeSpan totalTime
        ) : this(
            internalId, 
            oldZone, 
            newZone, 
            dateTime)
        {
            this.internalId = internalId;
            this.oldZone = oldZone;
            this.newZone = newZone;
            this.dateTime = dateTime;
            this.totalTime = totalTime;
        }
    }
}

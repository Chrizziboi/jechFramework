using System;

namespace jechFramework.Models
{
    /// <summary>
    /// ItemHistory klassen er laget for å kunne opprette historikk for varer. 
    /// </summary>
    public class ItemHistory
    {

        public int internalId { get; private set; }
        public int? oldZone { get; private set; } = null;
        public int newZone { get; private set; }
        public DateTime dateTime { get; private set; }

        // Beholder kun denne konstruktøren for å inkludere både gamle og nye lokasjoner
        /// <summary>
        /// Konstruktør for å opprette et nytt objekt av typen ItemHistory.
        /// </summary>
        /// <param name="internalId">Intern ID som identifiserer elementet.</param>
        /// <param name="oldLocation">Gammel plassering av elementet.</param>
        /// <param name="newLocation">Ny plassering av elementet.</param>
        /// <param name="dateTime">Tidspunktet for endringen.</param>
        public ItemHistory(int internalId, int? oldZone, int newZone, DateTime dateTime)
        {
            this.internalId = internalId;
            this.oldZone = oldZone;
            this.newZone = newZone;
            this.dateTime = dateTime;
        }
    }
}

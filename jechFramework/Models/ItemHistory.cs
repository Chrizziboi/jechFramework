using System;

namespace jechFramework.Models
{
    /// <summary>
    /// ItemHistory klassen er laget for å kunne opprette historikk for varer. 
    /// </summary>
    public class ItemHistory
    {

        public int internalId { get; private set; }
        public string oldLocation { get; private set; }
        public string newLocation { get; private set; }
        public DateTime dateTime { get; private set; }

        // Beholder kun denne konstruktøren for å inkludere både gamle og nye lokasjoner
        /// <summary>
        /// Konstruktør for å opprette et nytt objekt av typen ItemHistory.
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="oldLocation"></param>
        /// <param name="newLocation"></param>
        /// <param name="dateTime"></param>
        public ItemHistory(int internalId, string oldLocation, string newLocation, DateTime dateTime)
        {
            this.internalId = internalId;
            this.oldLocation = oldLocation;
            this.newLocation = newLocation;
            this.dateTime = dateTime;
        }
    }
}

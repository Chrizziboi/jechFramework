using System;

namespace jechFramework.Models
{
    public class ItemHistory
    {
        public int internalId { get; private set; }
        public string oldLocation { get; private set; }
        public string newLocation { get; private set; }
        public DateTime dateTime { get; private set; }

        // Beholder kun denne konstruktøren for å inkludere både gamle og nye lokasjoner
        public ItemHistory(int internalId, string oldLocation, string newLocation, DateTime dateTime)
        {
            internalId = internalId;
            oldLocation = oldLocation;
            newLocation = newLocation;
            dateTime = dateTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Models
{
    /// <summary>
    /// Klasse for soner.
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// Henter eller setter ID-en til sonen.
        /// </summary>
        public int zoneId { get; set; } = 0;

        /// <summary>
        /// Henter eller setter navnet til sonen.
        /// </summary>
        public string zoneName { get; set; }

        /// <summary>
        /// Henter eller setter beskrivelsen av sonen.
        /// </summary>
        public string zoneDescription { get; set; }

        /// <summary>
        /// Henter eller setter reolkapasiteten til sonen.
        /// </summary>
        public int shelfCapacity { get; set; } = 5; // Maks antall reoler i sonen

        /// <summary>
        /// Henter eller setter lagringstypen til sonen.
        /// </summary>
        public StorageType storageType { get; set; }

        /// <summary>
        /// Henter eller setter tiden det tar å plassere en vare i sonen.
        /// </summary>
        public TimeSpan itemPlacementTime { get; set; } // Tid for å plassere en vare

        /// <summary>
        /// Henter eller setter tiden det tar å hente en vare fra sonen.
        /// </summary>
        public TimeSpan itemRetrievalTime { get; set; } // Tid for å flytte en vare til vareforsendelse

        /// <summary>
        /// Henter eller setter listen over varer i sonen.
        /// </summary>
        public List<Item> itemsInZoneList { get; set; } = new List<Item>(); // nye items in warehouse list

        /// <summary>
        /// Henter eller setter listen over reoler i sonen.
        /// </summary>
        public List<Shelf> shelves { get; set; } = new List<Shelf>();

        /// <summary>
        /// Henter eller setter listen over lagringstyper som er tillatt i sonen.
        /// </summary>
        public List<StorageType> zonePacketList { get; set; } = new List<StorageType>();

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Zone"/>-klassen.
        /// </summary>
        public Zone()
        {
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Zone"/>-klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="zoneId">ID-en til sonen.</param>
        /// <param name="zoneName">Navnet til sonen.</param>
        /// <param name="zoneCapacity">Reolkapasiteten til sonen.</param>
        /// <param name="itemPlacementTime">Tiden det tar å plassere en vare i sonen.</param>
        /// <param name="itemRetrievalTime">Tiden det tar å hente en vare fra sonen.</param>
        /// <param name="zonePacketList">Listen over lagringstyper som er tillatt i sonen.</param>
        public Zone(
            int zoneId,
            string zoneName,
            int zoneCapacity,
            TimeSpan itemPlacementTime,
            TimeSpan itemRetrievalTime,
            List<StorageType> zonePacketList)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.itemPlacementTime = itemPlacementTime;
            this.itemRetrievalTime = itemRetrievalTime;
            this.zonePacketList = zonePacketList ?? new List<StorageType>();
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Zone"/>-klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="zoneId">ID-en til sonen.</param>
        /// <param name="zoneName">Navnet til sonen.</param>
        /// <param name="zoneCapacity">Reolkapasiteten til sonen.</param>
        /// <param name="itemPlacementTime">Tiden det tar å plassere en vare i sonen.</param>
        /// <param name="itemRetrievalTime">Tiden det tar å hente en vare fra sonen.</param>
        /// <param name="storageType">Lagringstypen til sonen.</param>
        public Zone(
            int zoneId,
            string zoneName,
            int zoneCapacity,
            TimeSpan itemPlacementTime,
            TimeSpan itemRetrievalTime,
            StorageType storageType)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.itemPlacementTime = itemPlacementTime;
            this.itemRetrievalTime = itemRetrievalTime;
            this.storageType = storageType;
        }

        /// <summary>
        /// Returnerer en strengrepresentasjon av sonen.
        /// </summary>
        /// <returns>En streng som representerer sonen.</returns>
        public override string ToString()
        {
            var storageTypes = zonePacketList.Any()
                ? string.Join(", ", zonePacketList)
                : storageType.ToString();

            return $"Zone ID: {zoneId}, Name: {zoneName}, Capacity: {shelfCapacity}, Placement Time: {itemPlacementTime.TotalSeconds}s, Retrieval Time: {itemRetrievalTime.TotalSeconds}s, Storage Type(s): {storageTypes}.";
        }

    }
}

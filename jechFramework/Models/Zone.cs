using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class Zone
    {
        ///<summary>
        /// Initialiserer variabler med get og set metoder
        ///</summary>
        public int zoneId { get; set; } = 0;
        public string zoneName { get; set; }
        public string zoneDescription { get; set; }
        public int shelfCapacity { get; set; } = 5; // Maks antall reoler i sonen
        public StorageType storageType { get; set; }
        public TimeSpan itemPlacementTime { get; set; } // Tid for å plassere en vare
        public TimeSpan itemRetrievalTime { get; set; } // Tid for å flytte en vare til vareforsendelse
        public List<Item> itemsInZoneList { get; set; } = new List<Item>(); // nye items in warehouse list

        public List<Shelf> shelves { get; set; }

        public List<StorageType> zonePacketList { get; set; } = new List<StorageType>();


        public Zone()
        {
            shelves = new List<Shelf>();
        }

        public Zone(int zoneId, string zoneName, int zoneCapacity, List<StorageType> zonePacketList)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.zonePacketList = zonePacketList;
        }

        public Zone(int zoneId, string zoneName, int zoneCapacity, StorageType storageType)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.storageType = storageType;
        }

        public Zone(int zoneId, string zoneName, string zoneDescription, int zoneCapacity, StorageType storageType)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.zoneDescription = zoneDescription;
            this.shelfCapacity = zoneCapacity;
            this.itemsInZoneList = itemsInZoneList;
            this.storageType = storageType;
        }
        public Zone(int zoneId, string zoneName, int zoneCapacity, TimeSpan itemPlacementTime, TimeSpan itemRetrievalTime, StorageType storageType)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.itemPlacementTime = itemPlacementTime;
            this.itemRetrievalTime = itemRetrievalTime;
            itemsInZoneList = new List<Item>();
            shelves = new List<Shelf>();
            this.storageType = storageType;
        }

        public Zone(int zoneId, string zoneName, int zoneCapacity, TimeSpan itemPlacementTime, TimeSpan itemRetrievalTime, List<StorageType> zonePacketList)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.shelfCapacity = zoneCapacity;
            this.itemPlacementTime = itemPlacementTime;
            this.itemRetrievalTime = itemRetrievalTime;
            itemsInZoneList = new List<Item>();
            shelves = new List<Shelf>();
            this.zonePacketList = zonePacketList;
        }


        /// <summary>
        /// Konstruktører (med og uten zoneDescription)
        /// </summary>
        /// <param name = "zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner</param>
        /// <param name = "zoneName">Dette er en variabel så hver sone kan ha et navn etter ønske</param>
        /// <param name = "zoneDescription">Dette er en beskrivelse av sonen, f.eks. hva som er i sonen</param>
        /// <param name = "zoneCapacity">Dette brukes for å ha en maks verdi på hvor mye det er plass til i en sone</param>
        public Zone(int zoneId, string zoneName, string zoneDescription, int shelfCapacity, TimeSpan itemPlacementTime, TimeSpan itemRetrievalTime, StorageType storageType)
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.zoneDescription = zoneDescription;
            this.shelfCapacity = shelfCapacity;
            this.itemPlacementTime = itemPlacementTime;
            this.itemRetrievalTime = itemRetrievalTime;
            itemsInZoneList = new List<Item>();
            shelves = new List<Shelf>();
            this.storageType = storageType;
        }
    }
} 

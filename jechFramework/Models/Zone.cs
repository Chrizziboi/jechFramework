using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Zone : Warehouse
    {
        ///<summary>
        /// Initialiserer variabler med get og set metoder
        ///</summary>
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public string zoneDescription { get; set; }
        public int zoneCapacity { get; set; }

        /// <summary>
        /// Konstruktører (med og uten zoneDescription)
        /// </summary>
        /// <param name = "zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner</param>
        /// <param name = "zoneName">Dette er en variabel så hver sone kan ha et navn etter ønske</param>
        /// <param name = "zoneDescription">Dette er en beskrivelse av sonen, f.eks. hva som er i sonen</param>
        /// <param name = "zoneCapacity">Dette brukes for å ha en maks verdi på hvor mye det er plass til i en sone</param>
        public Zone(int zoneId, string zoneName, string zoneDescription, int zoneCapacity) 
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.zoneDescription = zoneDescription;
            this.zoneCapacity = zoneCapacity;
        }
        public Zone(int zoneId, string zoneName, int zoneCapacity) 
        {
            this.zoneId = zoneId;
            this.zoneName = zoneName;
            this.zoneCapacity = zoneCapacity;
        }
    }
} // 

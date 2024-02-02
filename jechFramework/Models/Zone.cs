using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Zone : Warehouse
    {
        //Variabler
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public string zoneDescription { get; set; }
        public int zoneCapacity { get; set; }

        //Konstruktører
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

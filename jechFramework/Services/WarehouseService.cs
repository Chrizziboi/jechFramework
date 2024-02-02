using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    internal class WarehouseService
    {
        public int zoneId = 0;
        public void creatingZones(string zoneName, int zoneCapacity) 
        {
            zoneId += 1;
            Zone _ = new Zone (zoneId, zoneName, zoneCapacity);
        }

        Zone Sone1 = new Zone(1, "Sone 1 - Tørrvare", 15);
        Zone Sone2 = new Zone(2, "Sone 2 - Tørrvare", 15);
    }
}

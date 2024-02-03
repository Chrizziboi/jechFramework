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
        /// <summary>
        /// Her er det laget en funksjon for å kunne lage en sone hvor man kan lage navn og velge kapasiteten til en ny sone.
        /// </summary>
        int zoneId = 0;
        public void creatingZones(string zoneName, int zoneCapacity) 
        {
            zoneId += 1;
            Zone _ = new Zone (zoneId, zoneName, zoneCapacity);
        }

        /// <summary>
        /// Her er det en funksjon som gir en mulighet til å kunne fjerne en sone hvis man har gjort feil eller ikke vil ha en sone lenger
        /// </summary>
        /// <param name="zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner</param>
        /// <param name="listToRemove">Dette er listen over alle sonene i et varehus
        /// </param>
        public void removeZone(int zoneId, List<Zone> listToRemove) 
        {
            listToRemove.RemoveAt(zoneId);
        }
        
        //Zone sone1 = new Zone(1, "Sone 1 - Tørrvare", 15);
        //Zone sone2 = new Zone(2, "Sone 2 - Tørrvare", 15);
        
    }
}

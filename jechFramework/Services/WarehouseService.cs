using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    public class WarehouseService //Service klasse for Warehouse.cs, Employee.cs og Zone.cs
    {


        private List<Warehouse> warehouseList = new List<Warehouse>();
        // Liste med varehus



        /// 
        ///Service funksjoner for Warehouse.cs

        /// <summary>
        /// Funksjon for å opprette et varehus.
        /// </summary>
        public void CreateWarehouse(int warehouseId, string warehouseName, int warehouseCapacity)
        {
            var newWarehouse = new Warehouse(warehouseId, warehouseName, warehouseCapacity);

            warehouseList.Add(newWarehouse);


        }


        public void FindWareHouseInWarehouseList(int warehouseId)
        {
            var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

            if (warehouse == null)
            {
                Console.WriteLine($"A warehouse with id: {warehouseId} could not be found.");
                throw new InvalidOperationException($"A warehouse with id{warehouseId} could not be found.");

            }

            Console.WriteLine($"warehouse Id: {warehouse.warehouseId}\n" +
                              $"warehouse Name: {warehouse.warehouseName}\n" +
                              $"warehouse Capacity: {warehouse.warehouseCapacity}\n");


        }

        /// <summary>
        /// Funksjon for å ta vekk opprettede varehus fra varehuslisten.
        /// </summary>
        /// <param name="warehouseId"></param>
        public void RemoveWarehouse(int warehouseId)
        {
            var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

            if (warehouse == null)
            {
                throw new InvalidOperationException($"A warehouse with id: {warehouseId} could not be found.");
            }

            warehouseList.Remove(warehouse);

        }

        ///Service funksjoner for Zone.cs


        /// <summary>
        /// Her er det laget en funksjon for å kunne lage en sone hvor man kan lage navn og velge kapasiteten til en ny sone.
        /// </summary>
        int zoneId = 0;
        //public void CreateZone(int warehouseId, Zone zone) 
        public void CreateZone(int warehouseId, int zoneId, string zoneName, int? zoneCapacity)
        {
            

            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new InvalidOperationException($"Warehouse with id: {warehouseId} does not exist.");
                }
                var existingZone = warehouse.zoneList.FirstOrDefault(existingZone => existingZone.zoneId == zoneId);
                if (existingZone != null)
                {
                    throw new InvalidOperationException($"Zone with id: {zoneId} already exists in Warehouse id {warehouseId}.");
                }

                // Check if adding this zone would exceed warehouse capacity
                int totalZoneCapacity = warehouse.zoneList.Count() + 1;
                
                if (totalZoneCapacity > warehouse.warehouseCapacity)
                {
                    throw new InvalidOperationException($"Adding zone with id: {zoneId} would exceed warehouse capacity.");
                }

                Zone zone = new Zone(zoneId, zoneName, zoneCapacity);
                warehouse.zoneList.Add(zone);
                Console.WriteLine($"Successfully created Zone: {zoneId} in warehouse: {warehouseId}.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally handle the exception here if needed
            }
        }



        public void RemoveZoneInWarehouse(int warehouseId, int zoneId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new InvalidOperationException($"Warehouse with id: {warehouseId} does not exist.");
                }

                var zoneToRemove = warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId);
                if (zoneToRemove == null)
                {
                    throw new InvalidOperationException($"Zone with id: {zoneId} does not exist in Warehouse id {warehouseId}. Therefore zone could not be removed.");
                }

                warehouse.zoneList.Remove(zoneToRemove);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally handle the exception here if needed
            }
        }

        public void GetAllZonesInWarehouse(int warehouseId) 
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new InvalidOperationException($"Warehouse with id: {warehouseId} does not exist.");
                }

                Console.WriteLine($"Zones in Warehouse '{warehouse.warehouseName}':");

                foreach (var zone in warehouse.zoneList)
                {
                    Console.WriteLine($"Zone ID: {zone.zoneId}, Name: {zone.zoneName}, Capacity: {zone.zoneCapacity}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally handle the exception here if needed
            }
        }


        /// <summary>
        /// Her er det en funksjon som gir en mulighet til å kunne fjerne en sone hvis man har gjort feil eller ikke vil ha en sone lenger
        /// </summary>
        /// <param name="zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner</param>
        /// <param name="listToRemove">Dette er listen over alle sonene i et varehus
        /// </param>
        //public void removeZone(int zoneId, List<Zone> listToRemove) 
        //{
        //    listToRemove.RemoveAt(zoneId);
        //}

        //Zone sone1 = new Zone(1, "Sone 1 - Tørrvare", 15);
        //Zone sone2 = new Zone(2, "Sone 2 - Tørrvare", 15);


        ///Service funksjoner for Employee.cs
        /// <summary>
        /// Funksjon for å se alle ansatte for et gitt varehus
        /// </summary>
        public void CreateEmployee()
        {

            // var findEmployee = Warehouse.EmployeeList.FirstOrDefault(i => i.warehouseId == warehouseId);
            // 
            // for each(  );

        }

        public void RemoveEmployee()
        {

        }


    }
}

using jechFramework.Models;
using jechFramework.Services;

namespace FunctionsTester

{
    internal class FunctionsTest
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Testing JECH Frameworks Functions.\n");



            Console.WriteLine("\nWarehouseService Functions" +
                              "\n--------------------------\n");

            WarehouseService WService = new();
            //WService.WarehouseCreated += Service_WarehouseCreated;

            WService.CreateWarehouse(1, "Varehus 1", 5);
            WService.CreateWarehouse(2, "Varehus 2", 5);
            WService.CreateWarehouse(3, "Varehus 3", 5);
            WService.CreateWarehouse(4, "Varehus 4", 5);

            WService.FindWareHouseInWarehouseList(1);
            WService.FindWareHouseInWarehouseList(2);
            WService.FindWareHouseInWarehouseList(3);
            WService.FindWareHouseInWarehouseList(4);


            
            //WService.RemoveEmployee(1, 1);
            //service.CreateWarehouse(1, "Godeste Varehus", 5);
            //service.FindWareHouseInWarehouseList(1);
            //
            //service.CreateZone(1, 1, "Emirs P-Plass", 5);
            //service.CreateZone(1, 2, "Chris P-Plass", 4);
            //service.CreateZone(1, 3, "Joakim P-Plass", 3);
            //service.CreateZone(1, 4, "Hannan P-Plass", 2);
            //service.CreateZone(1, 5, "Edgar P-Plass", 2);
            ////service.CreateZone(1, 6, "Jesus P-Plass", 2);
            //
            //service.CreateZone(2, 7, "Jesus P-Plass", 2);
            //service.CreateZone(1, 1, "Jesus P-Plass", 2);
            //service.CreateZone(1, 7, "Jesus P-Plass", 2);
            //
            //
            //
            //service.GetAllZonesInWarehouse(1);
            //
            //service.RemoveZoneInWarehouse(1, 6);
            //
            //service.GetAllZonesInWarehouse(2);
            //
            //service.CreateEmployee(1, 1, "Emir");
            //service.CreateEmployee(1, 2, "Joakim");
            //service.CreateEmployee(1, 3, "Chris");
            //service.CreateEmployee(1, 4, "Hannan");
            //
            //service.GetAllEmployeesInWarehouse(1);
            //service.RemoveEmployee(1, 1);
            //
            //service.GetAllEmployeesInWarehouse(1);
            //
            //service.FindZoneById(1, 1);
            //
            //service.GetAllItemsInZone(1,1);





            Console.WriteLine($"\nItemService Functions" +
                              $"\n--------------------------\n");
            //ItemService IService = new(WService);
            //IService.CreateItem(1, 1, "FLAKSELOGG", "lodd");
            //IService.AddItem(1, 1, DateTime.Now, 1);
            //
            //
            //Item item = new(1, 1, "itemdummy", "dummytest");
            //


            Console.WriteLine("To finish testing, press the any key.");
            Console.ReadKey();

        }
    }
}

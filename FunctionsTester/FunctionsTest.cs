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


            WService.WarehouseCreated += Service_OnWarehouseCreated;
            WService.WarehouseRemoved += Service_OnWarehouseRemoved;
            WService.ZoneCreated += Service_OnZoneCreated;
            WService.ZoneRemoved += Service_OnZoneRemoved;
            WService.EmployeeCreated += Service_OnEmployeeCreated;
            WService.EmployeeRemoved += Service_OnEmployeeRemoved;


            WService.CreateWarehouse(1, "Varehus 1", 5);
            WService.CreateWarehouse(1, "Varehus 2", 3);

            WService.CreateZone(1, 1, "Zone 1", 10);
            WService.CreateZone(1, 2, "Zone 2", 10);
            WService.CreateZone(1, 3, "Zone 3", 10);
            WService.CreateZone(1, 4, "Zone 4", 10);
            WService.CreateZone(1, 5, "Zone 5", 10);
            WService.CreateZone(1, 6, "Zone 6", 10);

            WService.GetAllZonesInWarehouse(1);

            WService.CreateEmployee(1, 1, "Employee 1");
            WService.RemoveEmployee(1, 1);
            
            


            WService.WarehouseCreated -= Service_OnWarehouseCreated;


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

        private static void Service_OnWarehouseCreated(object sender, WarehouseEventArgs e)
        {
            Console.WriteLine($"Warehouse Created: {e.Warehouse.warehouseName} and ID: {e.Warehouse.warehouseId}");
        }

        private static void Service_OnWarehouseRemoved(object sender, WarehouseEventArgs e)
        {
            Console.WriteLine($"Warehouse Removed: {e.Warehouse.warehouseName} and ID: {e.Warehouse.warehouseId}");
        }

        private static void Service_OnZoneCreated(object sender, ZoneEventArgs e)
        {
            Console.WriteLine($"Zone Created: {e.Zone.zoneName} and ID: {e.Zone.zoneId}");
        }
        
        private static void Service_OnZoneRemoved(object sender, ZoneEventArgs e)
        {
            Console.WriteLine($"Zone Removed: {e.Zone.zoneName} and ID: {e.Zone.zoneId}");
        }

        private static void Service_OnEmployeeCreated(object sender, EmployeeEventArgs e)
        {
            Console.WriteLine($"Employee Created: {e.Employee.employeeName} and ID: {e.Employee.employeeId}");

        }
        
        private static void Service_OnEmployeeRemoved(object sender, EmployeeEventArgs e)
        {
            Console.WriteLine($"Employee Removed: {e.Employee.employeeName} and ID: {e.Employee.employeeId}");

        }



    }
}

using jechFramework.Models;
using jechFramework.Services;

namespace Program
{
    class CustomerWarehouse
    {
        static void Main(string[] args)
        {
            WarehouseService WService = new();
            PalletService PService = new();
            ItemService IService = new(WService);
            ItemHistoryService IHService = new();
            //Item Item = new();
            WaresInService waresInService = new WaresInService(IService, WService);
            WaresOutService waresOutService = new WaresOutService();
            PalletService palletService = new();


          //            Subscriptions
           WService.WarehouseCreated += Service_OnWarehouseCreated;
           WService.WarehouseRemoved += Service_OnWarehouseRemoved;
           WService.ZoneCreated += Service_OnZoneCreated;
           WService.ZoneRemoved += Service_OnZoneRemoved;
           WService.EmployeeCreated += Service_OnEmployeeCreated;
           WService.EmployeeRemoved += Service_OnEmployeeRemoved;
           
           IService.ItemCreated += Service_OnItemCreated;
           IService.ItemAdded += Service_OnItemAdded;
           IService.ItemRemoved += Service_OnItemRemoved;
           IService.ItemMoved += Service_OnItemMoved;

            Console.WriteLine("----- Create Warehouse -----");
            WService.CreateWarehouse(1, "Testhouse", 400);
            WService.CreateWarehouse(2, "Your mom's house", 500);
            
            Console.WriteLine("\n----- Creating Zone -----");
            WService.CreateZone(1, 1, "Cold Zone", 50, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.ClimateControlled);
            WService.CreateZone(1, 3, "Cold Zone 2", 50, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.ClimateControlled);
            WService.CreateZone(1, 2, "High Value Zone", 20, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.HighValue);

            Console.WriteLine("\n----- Create Employee -----");
            WService.CreateEmployee(1, 1, "Ola Nordmann");
            WService.CreateEmployee(1, 2, "Arne Roger");

            Console.WriteLine("\n----- Create Shelf -----");
            WService.AddShelfToZone(1, 1, 20, 3, 30);
            WService.AddShelfToZone(1, 1, 20, 3, 40);
            WService.AddShelfToZone(1, 2, 30, 3, 40, 8);

            Console.WriteLine("\n----- Create Item -----");
            IService.CreateItem(1, 1, 1, "Snickers", StorageType.ClimateControlled);
            IService.CreateItem(1, 2, 2, "Rolex", StorageType.HighValue);

           Console.WriteLine("\n----- Add Item -----");
           IService.AddItem(1, 1, 1, DateTime.Now);
           IService.AddItem(1, 2, 2, DateTime.Now, 3);
           IService.AddItem(1, 1, 2, DateTime.Now, 3);

            Console.WriteLine("\n----- Wares In -----");
            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 8, name = "Cheese", storageType = StorageType.ClimateControlled, quantity = 31 },
                new Item() { internalId = 7, name = "Dressing", storageType = StorageType.ClimateControlled, quantity = 40 }
            };
            waresInService.WaresIn(1, 1, incomingItems, DateTime.Now);

            Console.WriteLine("\n----- Pallets testing -----");

            List<Item> shitlist = new List<Item>() {
                new Item() { internalId = 8, name = "Cheese", storageType = StorageType.ClimateControlled, quantity = 31 },
                new Item() { internalId = 7, name = "Dressing", storageType = StorageType.ClimateControlled, quantity = 1 }
            };

            int totalQuantity = shitlist.Sum(x => x.quantity);

            Console.WriteLine(totalQuantity);

            var rest = (totalQuantity % 30 != 0);

            Console.WriteLine(rest);

            Console.WriteLine("\n----- Wares Out -----");
            IService.CreateItem(1, 10, null, "Soda", StorageType.ClimateControlled);
            IService.AddItem(1, 1, 10, DateTime.Now, 50); // Legger til 50 Soda i zone 1
            
            IService.CreateItem(1, 11, null, "Water", StorageType.ClimateControlled);
            IService.AddItem(1, 3, 11, DateTime.Now, 30); // Legger til 30 Water i zone 1
            
            // Planlegger en WaresOut som krever mer av en vare enn hva som er tilgjengelig
            List<Item> outgoingItems = new List<Item>()
            {
                new Item() { internalId = 10, quantity = 40 }, // Prøver å sende ut mer Soda enn tilgjengelig
                new Item() { internalId = 11, quantity = 20 }  // Dette antallet er tilgjengelig
            };
            waresOutService.WaresOut(1, 12, "Downtown Hub", outgoingItems, DateTime.Now);
            
            Console.WriteLine("\n");
            

             //             Unsubscriptions

            WService.WarehouseCreated -= Service_OnWarehouseCreated;
            WService.WarehouseRemoved -= Service_OnWarehouseRemoved;
            WService.ZoneCreated -= Service_OnZoneCreated;
            WService.ZoneRemoved -= Service_OnZoneRemoved;
            WService.EmployeeCreated -= Service_OnEmployeeCreated;
            WService.EmployeeRemoved -= Service_OnEmployeeRemoved;
            
            IService.ItemCreated -= Service_OnItemCreated;
            IService.ItemAdded -= Service_OnItemAdded;
            IService.ItemRemoved -= Service_OnItemRemoved;
            IService.ItemMoved -= Service_OnItemMoved;
            


            IService.ClearWarehouseData();
            IHService.ClearHistoryLog(); // Dette vil slette loggfilen

            Console.WriteLine("Simulation complete. Data has been cleared.");
            Console.WriteLine("Press any key to close this window...");
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

        private static void Service_OnWaresOutScheduledSentOut(object sender, WaresOutEventArgs e)
        {
            Console.WriteLine($"The Order with id: {e.orderId} has been sent out and is going to be " +
                              $"delivered to {e.destination} with a Shipment number of {e.lastShipmentNumber}.");

        }

        private static void Service_OnItemCreated(object sender, ItemEventArgs e)
        {
            Console.WriteLine($"Item: {e.name} and ID: {e.internalId} has been successfully " +
                              $"created in Warehouse: {e.warehouseId}.");

        }

        private static void Service_OnItemAdded(object sender, ItemEventArgs e)
        {
            Console.WriteLine($"{e.quantity} of item: {e.internalId} has been successfully " +
                              $"added to zone: {e.zoneId} in Warehouse: {e.warehouseId}.");

        }

        private static void Service_OnItemRemoved(object sender, ItemEventArgs e)
        {
            Console.WriteLine($"Item: {e.internalId} has been successfully " +
                              $"removed from zone: {e.zoneId} in Warehouse: {e.warehouseId}.");
        }

        private static void Service_OnItemMoved(object sender, ItemEventArgs e)
        {
            Console.WriteLine($"Item: {e.internalId} has been successfully " +
                              $"moved from zone: {e.oldZone} to zone: {e.newZone} at {DateTime.Now}" +
                              $" in Warehouse: {e.warehouseId}.");
        }
    }
}
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

            //              Subscriptions
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

            WService.CreateWarehouse(1, "Varehus 1", 6); // Anta at varehuset har kapasitet til 5 soner
                                                         // Anta at metoden CreateZone nå tar TimeSpan-objekter for tidsestimater
            WService.CreateEmployee(1, 1, "Hannan #ÅretsAnsatt");
            WService.CheckEmployeeAccessStatus(1, 1);
            WService.SetAccessToHighValueGoods(1, 1, true);
            WService.CheckEmployeeAccessStatus(1, 1);

            List<StorageType> zoneStorageTypes = new List<StorageType> { StorageType.Small, StorageType.Medium, StorageType.Large };

            Console.WriteLine("\n----- Creating Zones -----");
            WService.CreateZone(1, 1, "High Value Goods", 5, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.HighValue);
            WService.CreateZone(1, 2, "Climate Controlled Storage Area", 5, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.ClimateControlled);
            WService.CreateZone(1, 3, "Pallet Racking", 5, TimeSpan.FromMinutes(4), TimeSpan.FromMinutes(4), StorageType.Standard);
            WService.CreateZoneWithMultipleType(1, 4, "Small Item Shelving", 30, TimeSpan.FromSeconds(110), TimeSpan.FromSeconds(70), zoneStorageTypes);
            WService.CreateZone(1, 5, "Packing/Stacking", 150, TimeSpan.FromSeconds(50), TimeSpan.FromSeconds(50), StorageType.None);
            WService.CreateZone(1, 6, "High Value Goods", 5, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210), StorageType.HighValue);

            Console.WriteLine("\n----- Get All Zones in Warehouse -----");
            //WService.GetAllWarehouses();
            WService.GetAllZonesInWarehouse(1);

            Console.WriteLine("\n----- Adding Shelf to Zone -----");
            //Shelf newShelf = new Shelf(200, 40, 100, 2);
            WService.AddShelfToZone(1, 200, 40, 100);
            WService.AddShelfToZone(1, 200, 40, 100);
            WService.AddShelfToZone(1, 200, 40, 100);
            WService.AddShelfToZone(1, 200, 80, 100);

            WService.AddShelfToZone(2, 150, 40, 100);
            WService.AddShelfToZone(2, 150, 40, 100);
            WService.AddShelfToZone(2, 150, 40, 100);
            WService.AddShelfToZone(2, 150, 80, 100);

            WService.AddShelfToZone(3, 200, 40, 100, 8);
            WService.AddShelfToZone(3, 200, 40, 100, 8);
            WService.AddShelfToZone(3, 200, 40, 100, 8);

            //WService.AddShelfToZone(4, 720, 40, 100, 7); // 3 (7*3=21) 720/7
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 seksjon 1
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 seksjon 2
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 sseksjon 3
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 sseksjon 4
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 sseksjon 5
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 sseksjon 6
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 1 sseksjon 7
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 seksjon 1
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 seksjon 2
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 sseksjon 3
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 sseksjon 4
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 sseksjon 5
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 sseksjon 6
            WService.AddShelfToZone(4, 102, 40, 1, 0);// reol 2 sseksjon 7

            WService.AddShelfToZone(6, 200, 40, 100);
            WService.AddShelfToZone(6, 200, 40, 100);
            WService.AddShelfToZone(6, 200, 40, 100);
            WService.AddShelfToZone(6, 200, 80, 100);

            Console.WriteLine("\n----- Get all Shelves in Zone -----");
            WService.GetAllShelvesInZone(1, 1);
            WService.GetAllShelvesInZone(1, 2);
            WService.GetAllShelvesInZone(1, 3);
            WService.GetAllShelvesInZone(1, 4);



            Console.WriteLine("\n----- Create Item -----");
            IService.CreateItem(1, 1, null, "Cheese", StorageType.HighValue);
            IService.CreateItem(1, 2, null, "Ball'o'Cheese", StorageType.HighValue);
            Console.WriteLine("\n----- Add Item -----");
            IService.AddItem(1, 1, DateTime.Now, 1, 101);
            IService.AddItem(1, 1, DateTime.Now, 1, 1);
            IService.AddItem(2, 1, DateTime.Now, 1, 1);

            Console.WriteLine("\n----- Get all info on Items in Warehouse -----");
            IService.GetItemAllInfo(1, 2);
            IService.GetItemAllInfo(1, 1);
            
            Console.WriteLine("\n----- Get all Items in Zone -----");
            WService.GetAllItemsInZone(1, 1);

            Console.WriteLine("\n----- Move Item To Location -----");
            IService.MoveItemToLocation(1, 1, 6);
            IService.MoveItemToLocation(1, 2, 6);
            IService.MoveItemToLocation(1, 2, 1);
            IService.MoveItemToLocation(1, 2, 6);


            
            Console.WriteLine("\n----- Get Item History By Id -----");
            IHService.GetItemHistoryById(1,1);
            IHService.GetItemHistoryById(1,2);

            Console.WriteLine("\n----- Create Item -----");
            IService.CreateItem(1, 3, null, "Kebab", StorageType.HighValue );
            IService.CreateItem(1, 4, null, "T-Shirt",StorageType.ClimateControlled );
            IService.CreateItem(1, 5, null, "Pizza", StorageType.ClimateControlled);
            IService.CreateItem(1, 6, null, "Cola", StorageType.ClimateControlled);

            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 8, name = "Cheese", storageType = StorageType.ClimateControlled, quantity = 10 },
                new Item() { internalId = 7, name = "Dressing", storageType = StorageType.Standard }
            };

            Console.WriteLine("\n----- Wares In -----");
            waresInService.WaresIn(1, 1, DateTime.Now, incomingItems);

            Pallet pallet = new Pallet(10, "Hannan's pall");
            //WService.PlaceItemOnShelf(1, 1, 1, 1);
            Console.WriteLine("\n----- Count Pallets -----");
            //palletService.addPallet(waresOutService.palletList);
            //palletService.addPallet(waresOutService.palletList);

            PService.countPalletInWarehouse(1, WService, waresOutService);
            PService.countPalletInWarehouse(3, WService, waresOutService);
            Console.WriteLine(waresOutService.palletList.Any());


            //              Unsubscriptions

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
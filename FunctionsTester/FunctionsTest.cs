using jechFramework.Models;
using jechFramework.Services;

namespace FunctionsTester

{
    internal class FunctionsTest
    {
        /*
        static void Main(string[] args)
        {

            Console.WriteLine("Testing JECH Frameworks Functions.\n");
            
            Console.WriteLine("\nWarehouseService Functions" +
                              "\n--------------------------\n");


            WarehouseService WService = new();

            //              Subscriptions

            WService.WarehouseCreated += Service_OnWarehouseCreated;
            WService.WarehouseRemoved += Service_OnWarehouseRemoved;
            WService.ZoneCreated += Service_OnZoneCreated;
            WService.ZoneRemoved += Service_OnZoneRemoved;
            WService.EmployeeCreated += Service_OnEmployeeCreated;
            WService.EmployeeRemoved += Service_OnEmployeeRemoved;

            //              funksjons-kall             

            WService.CreateWarehouse(1, "Varehus 1", 1);
            WService.CreateWarehouse(1, "Varehus 2", 3);
            WService.CreateZone(1, 1, "Zone 1", 50);
            WService.CreateEmployee(1, 1, "Employee 1");

            WService.GetAllZonesInWarehouse(1);

            WService.CreateWarehouse(2, "Varehus 2", 2);
            WService.CreateZone(2, 1, "Zone 1", 10);
            WService.CreateZone(2, 2, "Zone 2", 10);
            WService.CreateZone(2, 3, "Zone 6", 10);

            WService.CreateEmployee(2, 1, "Employee 1");

            WService.RemoveZone(2, 1);
            WService.RemoveEmployee(2, 1);
            WService.RemoveWarehouse(2);



            //              Unsubscriptions

            WService.WarehouseCreated -= Service_OnWarehouseCreated;
            WService.WarehouseRemoved -= Service_OnWarehouseRemoved;
            WService.ZoneCreated -= Service_OnZoneCreated;
            WService.ZoneRemoved -= Service_OnZoneRemoved;
            WService.EmployeeCreated -= Service_OnEmployeeCreated;
            WService.EmployeeRemoved -= Service_OnEmployeeRemoved;


            Console.WriteLine($"\nItemService Functions" +
                              $"\n--------------------------\n");

            ItemService IService = new(WService);

            //              Subscriptions

            IService.ItemCreated += Service_OnItemCreated;
            IService.ItemAdded += Service_OnItemAdded;
            IService.ItemRemoved += Service_OnItemRemoved;
            IService.ItemMoved += Service_OnItemMoved;

            //              funksjons-kall  

            IService.CreateItem(1, 1, null, "Item 1", "Item");
            IService.AddItem(1, 1, DateTime.Now, 1);
            IService.MoveItemToLocation(1, 1, 1);
            IService.RemoveItem(1, 1);
            IService.RemoveItem(1, 1);
            IService.RemoveItem(1, 1);
            IService.RemoveItem(1, 1);

            IService.AddItem(1, 1, DateTime.Now, 1, 50);
            


            //              Unsubscriptions

            IService.ItemCreated -= Service_OnItemCreated;
            IService.ItemAdded -= Service_OnItemAdded;
            IService.ItemRemoved -= Service_OnItemRemoved;
            IService.ItemMoved -= Service_OnItemMoved;


            Console.WriteLine($"\n WaresInService Functions" +
                              $"\n--------------------------\n");

            WaresInService WINService = new(IService, WService);

            //              Subscriptions

            // ikke implementert

            //              funksjons-kall  

            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 2, name = "Kebab", type = "Food" },
                new Item() { internalId = 3, name = "Ananas", type = "Fruit" }
            };

            WINService.WaresIn(1, 1, DateTime.Now, 1, TimeSpan.FromMinutes(30), incomingItems);

            //              Unsubscriptions

            // ikke implementert

            Console.WriteLine($"\nWaresOutService Functions" +
                              $"\n--------------------------\n");


            
            WaresOutService WOUTService = new(IService);


            //              Subscriptions

            WOUTService.WaresOutScheduledSentOut += Service_OnWaresOutScheduledSentOut;

            //              funksjons-kall 


            List<Item> outgoingItems1 = new List<Item>() {
            new Item() { internalId = 1, name = "T-Shirt", type = "Clothes" },
            new Item() { internalId = 2, name = "Cola", type = "Soda" }
            };

            List<Item> outgoingItems2 = new List<Item>() {
            new Item() { internalId = 3, name = "Apple", type = "Fruit" },
            new Item() { internalId = 4, name = "Pomegranate", type = "Fruit" }
            };

            List<Item> outgoingItems3 = new List<Item>() {
            new Item() { internalId = 1, name = "T-Shirt", type = "Clothes" },
            new Item() { internalId = 2, name = "Cola", type = "Soda" }
            };

            WOUTService.WaresOut(1, 2, DateTime.Now.AddHours(1), "Ikea Moss", outgoingItems1);
            WOUTService.WaresOut(1, 3, DateTime.Now.AddHours(1), "Johnsen's Boutique", outgoingItems2);
            WOUTService.WaresOut(1, 4, DateTime.Now.AddHours(1), "Ikea Moss", outgoingItems3);
            //
            WOUTService.WaresOut(3, 2, DateTime.Now.AddHours(1), "Ikea Moss", outgoingItems3);


            //              Unsubscriptions

            WOUTService.WaresOutScheduledSentOut -= Service_OnWaresOutScheduledSentOut;




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
        */
    }
}

using jechFramework.Models;
using jechFramework.Services;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to JECH Warehouse Simulation");

            // Opprettelse av tjenesteinstanser
            Warehouse warehouse = new Warehouse(); // Initialiserer Warehouse
            WarehouseService warehouseService = new WarehouseService();
            ItemService itemService = new ItemService(warehouseService);
            ItemHistoryService itemHistoryService = new ItemHistoryService();
            WaresInService waresInService = new WaresInService(itemService, warehouseService);
            WaresOutService waresOutService = new WaresOutService();

            warehouseService.CreateWarehouse(1, "Warehouse 1", 5);
            warehouseService.FindWarehouseInWarehouseListWithPrint(1);

            warehouseService.CreateZone(1, 1, "Emirs P-Plass", 40);
            warehouseService.CreateZone(1, 2, "Chris P-Plass", 40);
            warehouseService.CreateZone(1, 3, "Joakim P-Plass", 3);
            warehouseService.CreateZone(1, 4, "Hannan P-Plass", 2);
            warehouseService.CreateZone(1, 5, "Edgar P-Plass", 2);
            warehouseService.CreateZone(1, 6, "Jesus P-Plass", 40);

            // Opprettelse og legging til varer
            itemService.CreateItem(1,6, 6, "Kebab", "Food");
            itemService.CreateItem(1,3, 3, "T-Shirt", "Clothes");
            itemService.CreateItem(1,4, null, "Pizza", "Food");
            itemService.CreateItem(1,5, null, "Cola", "Soda");

            // Legger til varer med riktig zoneId og warehouseId
            itemService.AddItem(4, 1, DateTime.Now, 1, 35);
            itemService.AddItem(3, 1, DateTime.Now, 1, 5);
            itemService.AddItem(6, 1, DateTime.Now, 1, 5);
            itemService.AddItem(5, 1, DateTime.Now, 1, 1);

            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 6, name = "Kebab", storageType = StorageType.HighValue },
                new Item() { internalId = 2, name = "Ananas", storageType = StorageType.HighValue }
            };

            waresInService.WaresIn(1,1, DateTime.Now, 1, TimeSpan.FromMinutes(30), incomingItems);

            itemService.FindHowManyItemsInZone(1,1);

            itemService.FindItemQuantityInWarehouse(1,1);

            // Flytting av varen til ulike lokasjoner og sporing av varehistorikk
            itemService.MoveItemToLocation(1,6, 2);
            itemService.MoveItemToLocation(1,2, 2);
            //itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(1,6, 3);
            //itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(1,6, 4);
            itemHistoryService.GetItemHistoryById(6);
            itemHistoryService.GetItemHistoryById(2);
            itemHistoryService.GetItemHistoryById(3);
            // itemHistoryService.GetItemHistoryById(4);
            // itemHistoryService.GetItemHistoryById(5);

            // Planlegging av varer som skal sendes ut
            List<Item> outgoingItems = new List<Item>() {
            new Item() { internalId = 4, name = "T-Shirt", type = "Clothes" },
            new Item() { internalId = 5, name = "Cola", type = "Soda" }
            };
            
            // Simulerer en ordre som behandles og varer som sendes ut
            waresOutService.WaresOut(1,2, DateTime.Now.AddHours(1), "Customer Location", outgoingItems);
            try
            {
                List<Item> nonExistingItems = new List<Item>() {
            new Item() { internalId = 999, name = "Non-Existing Item", type = "Ghost" }
            };
                waresOutService.WaresOut(1,3, DateTime.Now.AddHours(2), "Ghost Location", nonExistingItems);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Failed to schedule wares out for non-existing items: " + ex.Message);
            }
            
            //waresOutService.ScheduleWaresOut(1,);

            itemService.ClearWarehouseData();
            itemHistoryService.ClearHistoryLog(); // Dette vil slette loggfilen

            Console.WriteLine("Simulation complete. Data has been cleared.");
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();

        }
    }
}
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
            ItemService itemService = new ItemService();
            ItemHistoryService itemHistoryService = new ItemHistoryService();
            WaresInService waresInService = new WaresInService(itemService);
            WaresOutService waresOutService = new WaresOutService(itemService);


            // Opprettelse av nye varer
            // Item newItem = new Item { internalId = 6, location = "H0", dateTime = DateTime.Now };
            // Item newItem2 = new Item { internalId = 1, location = "H0", dateTime = DateTime.Now };
            itemService.CreateItem(6, 6, "Kebab", "Food");
            // itemService.CreateItem(2, 2, "Ananas", "Fruit");
            itemService.CreateItem(3, 3, "T-Shirt", "Clothes");
            itemService.CreateItem(4, null, "Pizza", "Food");
            itemService.CreateItem(5, null, "Cola", "Soda");

            //waresInService.ScheduleWaresIn(1, DateTime.Now, "H0", (hour:1,minutes:00,seconds:00), new Item(1,"Hairspray", "Hair products") ;

            // Legger til nye varer i warehouseItemList
            itemService.AddItem(4, "H0", DateTime.Now);
            itemService.AddItem(6, "H0", DateTime.Now);
            itemService.AddItem(5, "H0", DateTime.Now);
            // itemService.AddItem(5, "H0", DateTime.Now);
            // itemService.AddItem(4, "H0", DateTime.Now);


            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 6, name = "Kebab", type = "Food" },
                // new Item() { internalId = 6, name = "Kebab", type = "Food" },
                new Item() { internalId = 2, name = "Ananas", type = "Fruit" }
            };

            waresInService.ScheduleWaresIn(1, DateTime.Now, "H0", TimeSpan.FromMinutes(30), incomingItems);

            itemService.FindHowManyItemsInItemList();

            itemService.FindHowManyItemQuantityByInternalId(1);
         

            // Flytting av varen til ulike lokasjoner og sporing av varehistorikk
            itemService.MoveItemToLocation(6, "H2");
            itemService.MoveItemToLocation(2, "H2");
            //itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(6, "H3");
            //itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(6, "H4");
            itemHistoryService.GetItemHistoryById(6);
            itemHistoryService.GetItemHistoryById(2);
            itemHistoryService.GetItemHistoryById(3);
            // itemHistoryService.GetItemHistoryById(4);
            // itemHistoryService.GetItemHistoryById(5);

            // Planlegging av varer som skal sendes ut
            List<Item> outgoingItems = new List<Item>() {
            new Item() { internalId = 4, name = "Pizza", type = "Food" },
            new Item() { internalId = 5, name = "Cola", type = "Soda" }
            };

            // Simulerer en ordre som behandles og varer som sendes ut
            waresOutService.ScheduleWaresOut(2, DateTime.Now.AddHours(1), "Customer Location", outgoingItems);
            try
            {
                List<Item> nonExistingItems = new List<Item>() {
            new Item() { internalId = 999, name = "Non-Existing Item", type = "Ghost" }
            };
                waresOutService.ScheduleWaresOut(3, DateTime.Now.AddHours(2), "Ghost Location", nonExistingItems);
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
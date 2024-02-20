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
            itemService.CreateItem(6, 6,"Kebab","Food");
            itemService.CreateItem(2,2,"Ananas", "Fruit");
            itemService.CreateItem(3, 03, "T-sjorte", "Klær");

            // Legger til nye varer i warehouseItemList
            itemService.AddItem(2);
            itemService.AddItem(6);


            // Flytting av varen til ulike lokasjoner og sporing av varehistorikk
            itemService.MoveItemToLocation(6, "H2");
            itemService.MoveItemToLocation(2, "H2");
            itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(6, "H3");
            itemHistoryService.GetItemHistoryById(6);


            itemService.MoveItemToLocation(6, "H4");
            itemHistoryService.GetItemHistoryById(6);

            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();
        }
    }
}

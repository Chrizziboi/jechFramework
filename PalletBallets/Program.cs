using jechFramework.Models;
using jechFramework.Services;

namespace PalletBallet
{
    public class PalletBallet
    {
        static void Main(string[] args)
        {
            WarehouseService WService = new();
            PalletService PService = new();
            ItemService IService = new(WService);
            ItemHistoryService IHService = new();
            WaresInService waresInService = new(IService, WService, PService);
            WaresOutService waresOutService = new(IService, PService);
            PalletService palletService = new();


            //            Subscriptions
            //WService.WarehouseCreated += Service_OnWarehouseCreated;
            //WService.WarehouseRemoved += Service_OnWarehouseRemoved;
            //WService.ZoneCreated += Service_OnZoneCreated;
            //WService.ZoneRemoved += Service_OnZoneRemoved;
            //WService.EmployeeCreated += Service_OnEmployeeCreated;
            //WService.EmployeeRemoved += Service_OnEmployeeRemoved;
            //
            //IService.ItemCreated += Service_OnItemCreated;
            //IService.ItemAdded += Service_OnItemAdded;
            //IService.ItemRemoved += Service_OnItemRemoved;
            //IService.ItemMoved += Service_OnItemMoved;

            Console.WriteLine("----- Create Warehouse -----");
            WService.CreateWarehouse(1, "Testhouse", 400);
            WService.CreateWarehouse(2, "Testhouse 2", 500);

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

            IService.CreateItem(1, 10, null, "Soda", StorageType.ClimateControlled);
            IService.AddItem(1, 1, 10, DateTime.Now, 50); // Legger til 50 Soda i zone 1

            IService.CreateItem(1, 11, null, "Water", StorageType.ClimateControlled);
            IService.AddItem(1, 3, 11, DateTime.Now, 30); // Legger til 30 Water i zone 1

            Console.WriteLine("\n----- Add Item -----");
            IService.AddItem(1, 1, 1, DateTime.Now);
            IService.AddItem(1, 2, 2, DateTime.Now, 3);
            IService.AddItem(1, 1, 2, DateTime.Now, 3);

            Console.WriteLine("\n----- Pallets testing -----\n");
            PService.CountPallets();

            Console.WriteLine("\n----- Wares In -----");
            List<Item> incomingItems = new List<Item>() {
                new Item() { internalId = 8, name = "Cheese", storageType = StorageType.ClimateControlled, quantity = 30 },
                new Item() { internalId = 7, name = "Dressing", storageType = StorageType.ClimateControlled, quantity = 30 }
            };

            waresInService.WaresIn(1, 1, incomingItems, DateTime.Now);
            Console.WriteLine("TEST_Incoming");
            List<Item> scheduledIncomingItems = new List<Item>()
            {
                new Item() { internalId = 1, quantity = 20, storageType = StorageType.ClimateControlled }, // Prøver å sende ut mer Soda enn tilgjengelig
                new Item() { internalId = 2, quantity = 40, storageType = StorageType.HighValue }  // Dette antallet er tilgjengelig
            };
            waresInService.ScheduleWaresIn(1, 2, scheduledIncomingItems, DateTime.Now, RecurrencePattern.Weekly);

            Console.WriteLine("\n----- Pallets testing after WaresIn -----\n");
            PService.CountPallets();

            Console.WriteLine("\n----- Wares Out -----");
            

            // Planlegger en WaresOut som krever mer av en vare enn hva som er tilgjengelig
            List<Item> outgoingItems = new List<Item>()
            {
                new Item() { internalId = 10, quantity = 40 }, // Prøver å sende ut mer Soda enn tilgjengelig
                new Item() { internalId = 11, quantity = 20 }  // Dette antallet er tilgjengelig
            };

            waresOutService.WaresOut(1, 12, "Downtown Hub", outgoingItems, DateTime.Now);

            PService.CountPallets();

            List<Item> ScheduledOutgoingItems = new List<Item>()
            {
                new Item() { internalId = 1, quantity = 10 }, // Prøver å sende ut mer Soda enn tilgjengelig
                new Item() { internalId = 2, quantity = 5 }  // Dette antallet er tilgjengelig
            };
            waresOutService.ScheduleWaresOut(1, 3, "Partytown", ScheduledOutgoingItems, DateTime.Now, RecurrencePattern.Weekly);
            PService.CountPallets();

            //IService.GetItemAllInfo(1, 10); // Skal vise at Soda fortsatt har 50 enheter, ingen ble fjernet
            //IService.GetItemAllInfo(1, 11); // Skal vise at Water er redusert til 10 enheter (30 - 20)


            Console.WriteLine("\n----- Pallets testing after WaresOut -----\n");
            PService.CountPallets();

            Console.WriteLine("\n----- Pallets testing end -----\n");

            Console.WriteLine(WService.GetAllWarehouses());

            Zone zone = new Zone();
            Item item = new Item();
            item.storageType = StorageType.ClimateControlled;
            zone.storageType = StorageType.HighValue;
            WService.IsStorageTypeCompatible(zone, item);



            Console.WriteLine("\n");


            //             Unsubscriptions

            //WService.WarehouseCreated -= Service_OnWarehouseCreated;
            //WService.WarehouseRemoved -= Service_OnWarehouseRemoved;
            //WService.ZoneCreated -= Service_OnZoneCreated;
            //WService.ZoneRemoved -= Service_OnZoneRemoved;
            //WService.EmployeeCreated -= Service_OnEmployeeCreated;
            //WService.EmployeeRemoved -= Service_OnEmployeeRemoved;
            //
            //IService.ItemCreated -= Service_OnItemCreated;
            //IService.ItemAdded -= Service_OnItemAdded;
            //IService.ItemRemoved -= Service_OnItemRemoved;
            //IService.ItemMoved -= Service_OnItemMoved;



            IService.ClearWarehouseData();
            IHService.ClearHistoryLog(); // Dette vil slette loggfilen

            Console.WriteLine("Simulation complete. Data has been cleared.");
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();


        }
    }
}

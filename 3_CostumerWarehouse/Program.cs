using jechFramework.Interfaces;
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
            Item Item = new();

            WService.CreateWarehouse(1, "Varehus 1", 5); // Anta at varehuset har kapasitet til 5 soner
                                                         // Anta at metoden CreateZone nå tar TimeSpan-objekter for tidsestimater
            WService.CreateEmployee(1, 1, "Hannan #ÅretsAnsatt");
            WService.CheckEmployeeAccessStatus(1, 1);
            WService.SetAccessToHighValueGoods(1, 1, true);
            WService.CheckEmployeeAccessStatus(1, 1);


            WService.CreateZone(1, 1, "High Value Goods", 5, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210));
            WService.CreateZone(1, 2, "Climate Controlled Storage Area", 5, TimeSpan.FromSeconds(70), TimeSpan.FromSeconds(210));
            WService.CreateZone(1, 3, "Pallet Racking", 5, TimeSpan.FromMinutes(4), TimeSpan.FromMinutes(4));
            WService.CreateZone(1, 4, "Small Item Shelving", 30, TimeSpan.FromSeconds(110), TimeSpan.FromSeconds(70));
            WService.CreateZone(1, 5, "Packing/Stacking", 150, TimeSpan.FromSeconds(50), TimeSpan.FromSeconds(50));

            WService.GetAllWarehouses();
            WService.GetAllZonesInWarehouse(1);

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

            WService.GetAllShelvesInZone(1, 1);
            WService.GetAllShelvesInZone(1, 2);
            WService.GetAllShelvesInZone(1, 3);
            WService.GetAllShelvesInZone(1, 4);




            IService.CreateItem(1, 1, null, "Cheese", Item.StorageType.Small);
            IService.CreateItem(1, 2, null, "Ball'o'Cheese", Item.StorageType.Medium);
            IService.AddItem(1, 1, DateTime.Now, 1, 101);
            IService.AddItem(1, 1, DateTime.Now, 1, 1);
            IService.AddItem(2, 1, DateTime.Now, 1, 1);

            WService.GetAllItemsInZone(1, 1);

            IService.MoveItemToLocation(1, 1, 2);
            IService.MoveItemToLocation(1, 2, 2);
            IService.MoveItemToLocation(1, 2, 3);
            IService.MoveItemToLocation(1, 2, 4);

            IHService.GetItemHistoryById(1);
            IHService.GetItemHistoryById(2);

            Pallet pallet = new Pallet(10, "palle");
            WService.PlaceItemOnShelf(1, 1, 1, 1);
            Console.WriteLine("----- Count Pallets -----");
            PService.countPalletInWarehouse(1, WService);


            IService.ClearWarehouseData();
            IHService.ClearHistoryLog(); // Dette vil slette loggfilen

            Console.WriteLine("Simulation complete. Data has been cleared.");
            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();

        }
    }
}
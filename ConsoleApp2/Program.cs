using jechFramework.Interfaces;
using jechFramework.Models;
using jechFramework.Services;
using System;
using System.Collections.Generic;

namespace program
{
    internal class program1
    {
        static void Main(string[] args)
        {
            // Opprettelse av tjenesteinstanser
            WaresInService waresInService = new WaresInService();
            WaresOutService waresOutService = new WaresOutService();
            ItemService itemService = new ItemService();
            ItemHistoryService itemHistory = new ItemHistoryService();
            

            // Opprettelse av en liste over vareobjekter
            List<Item> warehouseItemList = new List<Item>
            {
                new Item { internalId = 1, location = "H1", dateTime = DateTime.Now },
                new Item { internalId = 2, location = "H1", dateTime = DateTime.Now },
                // Legg til flere vareobjekter etter behov
            };

            // Opprettelse av en ny vare
            itemService.CreateItem(internalId: 1, location: "H1", dateTime: DateTime.Now);

            // Planlegging av innkommende varer
            waresInService.ScheduleWaresIn(0001, DateTime.Now, "H1", TimeSpan.FromMinutes(10), warehouseItemList);

            // Flytting av varen til ulike lokasjoner og sporing av varehistorikk
            itemService.MoveItemToLocation(1, "H2");
            itemHistory.GetItemHistoryById(1);

            itemService.MoveItemToLocation(1, "H3");
            itemHistory.GetItemHistoryById(1);

            itemService.MoveItemToLocation(1, "H4");
            itemHistory.GetItemHistoryById(1);

            waresOutService.ScheduleWaresOut(0001, DateTime.Now, "H2", warehouseItemList);


        }
    }
}

using jechFramework.Interfaces;
using jechFramework.Models;
using jechFramework.Services;
using System;
using System.Collections.Generic;

static void Main(string[] args)
{
    Console.WriteLine("Welcome to JECH Warehouse Simulation");


    // Opprettelse av tjenesteinstanser
    ItemService itemService = new ItemService();
    ItemHistoryService itemHistoryService = new ItemHistoryService();
    WaresInService waresInService = new WaresInService(itemService);
    WaresOutService waresOutService = new WaresOutService(itemService);


    // Opprettelse av nye varer
    Item newItem = new Item { internalId = 6, location = "H0", dateTime = DateTime.Now };
    Item newItem2 = new Item { internalId = 1, location = "H0", dateTime = DateTime.Now };


    // Legger til nye varer i warehouseItemList
    itemService.AddItem(newItem);
    itemService.AddItem(newItem2);


    // Flytting av varen til ulike lokasjoner og sporing av varehistorikk
    itemService.MoveItemToLocation(6, "H2");
    itemHistoryService.GetItemHistoryById(6);


    itemService.MoveItemToLocation(6, "H3");
    itemHistoryService.GetItemHistoryById(6);


    itemService.MoveItemToLocation(6, "H4");
    itemHistoryService.GetItemHistoryById(6);
    
}

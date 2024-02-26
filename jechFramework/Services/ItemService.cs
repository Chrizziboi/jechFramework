using jechFramework.Interfaces;
using jechFramework.Models;
using System.IO;

namespace jechFramework.Services
{

    public class ItemService : IItemService
    //implementerer itemservice på interfacet IItemService
    {
        /// <summary>
        /// Funksjoner for Item.cs
        /// </summary>
        /// 
        // public ItemService()
        // {
        //     itemList = new List<Item>();
        // 
        // }

        private static List<Item> warehouseItemList = new List<Item>();
        private static List<Item> createdItemsList = new List<Item>();


        public void CreateItem(int internalId, int? externalId, string name, string type)
        {
            if (createdItemsList.Any(i => i.internalId == internalId))
            {
                throw new InvalidOperationException("Item with this internal ID already exists in the creation list.");
            }

            var newItem = new Item
            {
                internalId = internalId,
                externalId = externalId,
                name = name,
                type = type
            };

            createdItemsList.Add(newItem);
        }


        public void AddItem(int internalId, string location, DateTime dateTime)
        {
            var itemToAdd = createdItemsList.FirstOrDefault(i => i.internalId == internalId);
           
            try
            {
                if (itemToAdd == null)
                {
                    throw new InvalidOperationException("No item with this internal ID exists in the creation list.");
                }

            }

            finally
            {
                if (itemToAdd != null)
                {

                    if (warehouseItemList.Any(i => i.internalId == internalId))
                    {
                        //legger til 1 i kvantitet for item når når det kommer inn nye varer som allerede eksisterer
                        itemToAdd.quantity += 1;
                        Console.WriteLine($"Item with this internal ID already exists in the warehouse, " +
                                          $"updated the {internalId} with a new quantity: {itemToAdd.quantity}");

                    }


                    // Setter lokasjon og dato før du legger til i warehouseItemList
                    itemToAdd.location = location;
                    itemToAdd.dateTime = dateTime;

                    warehouseItemList.Add(itemToAdd);

                    // Logger denne første plasseringen av varen
                    LogItemMovement(new ItemHistory(internalId, null, location, dateTime));

                    Console.WriteLine($"Item {internalId} has been added to the warehouse at location {location}.");

                }

            }
        }

        public void RemoveItem(int internalId)
        {
            var item = warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
            if (item != null)
            {
                if (item.quantity > 1)
                {
                    // Reduserer antallet med én hvis det er mer enn én på lager.
                    item.quantity -= 1;
                }
                else
                {
                    // Fjerner varen helt hvis dette var den siste.
                    warehouseItemList.Remove(item);
                    Console.WriteLine($"Item with internal ID {internalId} is now out of stock and has been removed from the warehouse list.");
                    // Her kan du implementere ytterligere logikk for varsling eller håndtering av utsolgte varer.
                }
            }
            else
            {
                throw new InvalidOperationException("Item not found.");
            }
        }


        public void MoveItemToLocation(int internalId, string newLocation)
            {
             var item = warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
             if (item != null)
             {
                var oldLocation = item.location; // Lagrer den gamle lokasjonen før oppdatering
                item.location = newLocation; // Oppdaterer lokasjonen
                item.dateTime = DateTime.Now; // Oppdaterer tidsstempel

                // Oppretter en ny ItemHistory oppføring
                var itemHistory = new ItemHistory(internalId, oldLocation, newLocation, item.dateTime);

                // Anta at vi har en metode for å logge denne historikken til en fil
                LogItemMovement(itemHistory);

                Console.WriteLine($"Item {internalId} has been moved from {oldLocation} to {newLocation} at {item.dateTime}");
                }
                else
                {
                    Console.WriteLine($"Item with internal ID {internalId} not found. No action taken.");
                }
            }

        // En hjelpemetode for å logge bevegelsen til en fil
        private void LogItemMovement(ItemHistory itemHistory)
        {
            // Format for logginnlegget
            var logEntry = $"{itemHistory.internalId},{itemHistory.oldLocation},{itemHistory.newLocation},{itemHistory.dateTime}\n";

            // Spesifiser stien til loggfilen
            var logFilePath = "ItemMovements.log";

            // Skriver logginnlegget til filen
            File.AppendAllText(logFilePath, logEntry);
        }

        public int FindHowManyItemsInItemList(int internalId)
        {

            int countedItems = warehouseItemList.Count(item => item.internalId == internalId);

            // var returnedCountedItems = $"The counted items for item: {internalId} is: {countedItems}.";

            return countedItems;

        }
        // Inne i ItemService-klassen
        public void FindItemByInternalId(int internalId)
        {

            warehouseItemList.FirstOrDefault(i => i.internalId == internalId);

        }
        public void ClearWarehouseData()
        {
            warehouseItemList.Clear(); // Tømmer listen over varer i varehuset
            createdItemsList.Clear();          // Eventuelt tøm andre lister eller ressurser om nødvendig
        }

        public string GetLocationByInternalId(int internalId)
        {
            var item = warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
            return item?.location ?? "Unallocated"; // Returnerer "Unallocated" hvis varen ikke finnes
        }


        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}
        public bool ItemExists(int internalId)
        {
            return createdItemsList.Any(i => i.internalId == internalId);
        }

    }
       
}
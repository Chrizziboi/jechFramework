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
        private static List<Item> itemList = new List<Item>();


        public void CreateItem(int internalId, int? externalId, string name, string type)
        {
            if (itemList.Any(i => i.internalId == internalId))
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

            itemList.Add(newItem);
        }

        public void AddItem(int internalId, string location, DateTime dateTime)
        {
            var itemToAdd = itemList.FirstOrDefault(i => i.internalId == internalId);
            if (itemToAdd == null)
            {
                throw new InvalidOperationException("No item with this internal ID exists in the creation list.");
            }

            if (warehouseItemList.Any(i => i.internalId == internalId))
            {

                throw new InvalidOperationException("Item with this internal ID already exists in the warehouse.");
            }

            // Setter lokasjon og dato før du legger til i warehouseItemList
            
            itemToAdd.location = location;
            itemToAdd.dateTime = dateTime;

            warehouseItemList.Add(itemToAdd);

            // Logger denne første plasseringen av varen
            LogItemMovement(new ItemHistory(internalId, null, location, dateTime));

            Console.WriteLine($"Item {internalId} has been added to the warehouse at location {location}.");
        }


        public void RemoveItem(int internalId)
        {
            var item = warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
            if (item != null)
            {
                warehouseItemList.Remove(item);
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
                throw new InvalidOperationException("Item not found.");
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
            warehouseItemList.Count(item => item.internalId == internalId);
            return warehouseItemList.Count;

        }
        // Inne i ItemService-klassen
        public void FindItemByInternalId(int internalId)
        {

            warehouseItemList.FirstOrDefault(i => i.internalId == internalId);

        }
        public void ClearWarehouseData()
        {
            warehouseItemList.Clear(); // Tømmer listen over varer i varehuset
            itemList.Clear();          // Eventuelt tøm andre lister eller ressurser om nødvendig
        }


        public void AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}

    }
}

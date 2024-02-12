using jechFramework.Interfaces;
using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{

    internal class ItemService : IItemService
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
            
        public void AddItem(int internalId)
        {
            var item = warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(internalId), "Item cannot be null");
            }
            if (!warehouseItemList.Any(i => i.internalId == internalId))
            {
                warehouseItemList.Add(item);
            }
            else
            {
                throw new InvalidOperationException("Item with the same internal ID already exists.");
            }
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
                //string oldLocation = item.location
                item.location = newLocation;
                item.dateTime = DateTime.Now;

                var itemHistory = new ItemHistory(internalId, newLocation, item.dateTime);
                //Lager historikk for items ved å opprette et objekt i ItemHistory ved endring av lokasjon.
                Console.WriteLine($"Item has been successfully updated to: {item.location} at: {item.dateTime}");

            }
            else
            {
                throw new InvalidOperationException("Item not found.");
            }
        }

        public int FindHowManyItemsInItemList(int internalId)
        {
            warehouseItemList.Count(item => item.internalId == internalId);
            return warehouseItemList.Count;

        }
        // Inne i ItemService-klassen
        public Item FindItemByInternalId(int internalId)
        {

            return warehouseItemList.FirstOrDefault(i => i.internalId == internalId);
        
        }

        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}

    }
}

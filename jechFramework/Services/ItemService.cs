using jechFramework.Interfaces;
using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    internal class ItemService : IItemService //implementerer itemservice på interfacet IItemService
    {
        /// <summary>
        /// Funksjoner for Item.cs
        /// </summary>
        /// 
        public ItemService()
        {
            itemList = new List<Item>();
        
        }

        public List<Item> itemList;

        public void AddItem(int internalId)
        {
            var item = itemList.FirstOrDefault(item => item.internalId == internalId);
            itemList.Add(item);
         
        }

        public void RemoveItem(int internalId) 
        {
            var item = itemList.FirstOrDefault(item => item.internalId == internalId);
            itemList.Remove(item);

        }


        public void MoveItemToLocation(int internalId, string newLocation) 
        {
            var item = itemList.FirstOrDefault(item => item.internalId == internalId);

            if (internalId != null)
            {
                item.location = newLocation;
                
            }

        }


        public int FindHowManyItemsInItemList(int internalId) 
        {
            itemList.Count(item => item.internalId == internalId);
            return itemList.Count;

        }


    }
}

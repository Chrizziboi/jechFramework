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

        private static readonly List<Item> _itemList = new List<Item>();

        public void AddItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null");
            }
            if (!_itemList.Any(i => i.InternalId == item.InternalId))
            {
                _itemList.Add(item);
            }
            else
            {
                throw new InvalidOperationException("Item with the same internal ID already exists.");
            }
        }

        public void RemoveItem(int internalId)
        {
            var item = _itemList.FirstOrDefault(i => i.InternalId == internalId);
            if (item != null)
            {
                _itemList.Remove(item);
            }
            else
            {
                throw new InvalidOperationException("Item not found.");
            }
        }

        public void MoveItemToLocation(int internalId, string newLocation)
        {
            var item = _itemList.FirstOrDefault(i => i.InternalId == internalId);
            if (item != null)
            {
                item.Location = newLocation;
            }
            else
            {
                throw new InvalidOperationException("Item not found.");
            }
        }

        public int FindHowManyItems(int internalId)
        {
            return _itemList.Count(i => i.InternalId == internalId);
        }

    }
}

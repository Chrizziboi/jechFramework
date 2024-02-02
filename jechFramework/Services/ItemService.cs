using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    internal class ItemService
    {
        /// <summary>
        /// Funksjoner for Item.cs
        /// </summary>
        /// 
        private List<Item> itemList;

        //public ItemService()
        //{
        //    itemList = new List<Item>();
        //
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
                
        
        public void AddItem(Item item)
        { 
            itemList.Add(item);
         
        }

        public void RemoveItem(Item item) 
        {
            itemList.Remove(item);

        }


        ///
        /// public void MoveItemToDifferentZone(Item internalId, Zone zoneId)
        /// {
        ///     var internalId = Item.internalId;
        ///     
        /// }

        public void MoveItemToLocation(String Location) 
        {

        }


        public void FindHowManyItemsInItemList(string itemname) 
        {
            itemList.Count(item => item.name == itemname);

        }


    }
}

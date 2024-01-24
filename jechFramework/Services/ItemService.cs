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
        private List<Item> itemList;
        
        public ItemService() 
        {
            itemList = new List<Item>();
        }

        public void AddItem(Item item)
        { 
            itemList.Add(item);
        }

        public void RemoveItem(Item item) 
        {
            itemList.Remove(item);
        }

        public void MoveItemInWarehouse(Item item, Warehouse warehouse)
        {
            
        }

        public void MoveItemFromWarehouse(Item item, Warehouse warehouse1, Warehouse warehouse2)
        {

        }

        public void MoveItemList() 
        {
            foreach (Item item in itemList) 
            {  }
        }




    }
}

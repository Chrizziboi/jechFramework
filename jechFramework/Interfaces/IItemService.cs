using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Models;

namespace jechFramework.Interfaces
{
    public interface IItemService 
    {
        /// <summary>
        /// Her er det interface for ItemService.cs
        /// </summary>
        /// 
        void CreateItem(int internalId, int? externalId, string name, string type);

        void AddItem(int internalId, string location, DateTime dateTime);

        void RemoveItem(int internalId);

        void MoveItemToLocation(int internalId, string newLocation);

        int FindHowManyItemsInItemList(int internalId);

        void FindItemByInternalId(int internalId);

 
    }
}

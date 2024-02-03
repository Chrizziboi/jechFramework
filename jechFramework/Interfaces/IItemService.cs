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

        void AddItem(int internalId);

        void RemoveItem(int internalId);

        void MoveItemToLocation(int internalId, string newLocation);

        int FindHowManyItemsInItemList(int internalId);

    }
}

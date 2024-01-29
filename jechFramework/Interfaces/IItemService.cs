using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Models;

namespace jechFramework.Interfaces
{
    internal interface IItemService 
    {
        /// <summary>
        /// Her er det interface for ItemService.cs
        /// </summary>
        void CalculateWeightForItem();

        void AddItem();

        void RemoveItem();

        void UpdateWeightForItem();
           

    }
}

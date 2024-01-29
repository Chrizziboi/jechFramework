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
        void CalculateWeightForItem();

        void AddItem();

        void RemoveItem();

        void UpdateWeightForItem();
           

    }
}

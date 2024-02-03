using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Interfaces
{
    /// <summary>
    /// Interface for pall funksjoner
    /// </summary>
    public interface IPalletService
    {
        int CalculateWeightForPallet(int internalPalletId);

        void MovePalletToLocation(int internalPalletId, string newLocation);

    }
}

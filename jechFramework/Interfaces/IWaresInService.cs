using jechFramework.Models;
using System;
using System.Collections.Generic;

namespace jechFramework.Interfaces
{
    public interface IWaresInService  // Endrer grensesnittet til public
    {
        // Metoder som forventes å bli implementert av tjenesten for inngående varer
        void WaresIn(int warehouseId, int orderId, DateTime scheduledTime, TimeSpan processingTime, List<Item> incomingItems);
    }
}

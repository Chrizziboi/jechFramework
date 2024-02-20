using jechFramework.Models;
using System;
using System.Collections.Generic;

namespace jechFramework.Interfaces
{
    public interface IWaresInService  // Endrer grensesnittet til public
    {
        // Metoder som forventes å bli implementert av tjenesten for inngående varer
        void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems);
    }
}

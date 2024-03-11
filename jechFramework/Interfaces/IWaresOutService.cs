using jechFramework.Models;
using System;
using System.Collections.Generic;

namespace jechFramework.Interfaces
{
    public interface IWaresOutService
    {
        // Methods expected to be implemented by the service for outgoing wares
        void WaresOut(int warehouseId,int orderId, DateTime scheduledTime, string destination, List<Item> outgoingItems);
    }
}
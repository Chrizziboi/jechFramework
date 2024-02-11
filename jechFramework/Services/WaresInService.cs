using jechFramework.Interfaces;
using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    public class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> _scheduledWaresIns = new List<WaresIn>();
        private readonly List<RecurringOrder> _scheduledRecurringOrders = new List<RecurringOrder>();

        public void ScheduleWaresIn(WaresIn waresIn)
        {
            if (waresIn == null) throw new ArgumentNullException(nameof(waresIn));
            if (_scheduledWaresIns.Any(wi => wi.orderId == waresIn.orderId))
                throw new InvalidOperationException($"A WaresIn with OrderId {waresIn.orderId} is already scheduled.");
            _scheduledWaresIns.Add(waresIn);
        }

        public void ScheduleRecurringOrder(RecurringOrder recurringOrder)
        {
            if (recurringOrder == null) throw new ArgumentNullException(nameof(recurringOrder));
            _scheduledRecurringOrders.Add(recurringOrder);
        }

        public void UpdateWaresIn(WaresIn waresIn)
        {
            var existingWaresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == waresIn.orderId);
            if (existingWaresIn == null) throw new InvalidOperationException("WaresIn not found.");
            _scheduledWaresIns.Remove(existingWaresIn);
            _scheduledWaresIns.Add(waresIn);
        }

        public WaresIn GetWaresIn(int orderId)
        {
            var waresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == orderId);
            if (waresIn == null) throw new InvalidOperationException("WaresIn not found.");
            return waresIn;
        }

        public void DeleteWaresIn(int orderId)
        {
            var waresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == orderId);
            if (waresIn == null) throw new InvalidOperationException("WaresIn not found.");
            _scheduledWaresIns.Remove(waresIn);
        }

        public IEnumerable<WaresIn> GetAllScheduledWaresIn()
        {
            return _scheduledWaresIns.AsReadOnly();
        }
    }
}

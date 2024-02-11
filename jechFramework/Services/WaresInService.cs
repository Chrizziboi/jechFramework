using jechFramework.Interfaces;
using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace jechFramework.Services
{
    internal class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> _scheduledWaresIns = new List<WaresIn>();

        public void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            if (_scheduledWaresIns.Any(wi => wi.orderId == orderId))
            {
                throw new InvalidOperationException($"A WaresIn with OrderId {orderId} is already scheduled.");
            }

            var newWaresIn = new WaresIn
            {
                orderId = orderId,
                scheduledTime = scheduledTime,
                location = location,
                processingTime = processingTime,
                incomingItems = incomingItems
            };

            _scheduledWaresIns.Add(newWaresIn);
        }

        public void UpdateWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<Item> incomingItems)
        {
            var existingWaresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == orderId);
            if (existingWaresIn == null)
            {
                throw new InvalidOperationException("WaresIn not found.");
            }

            existingWaresIn.scheduledTime = scheduledTime;
            existingWaresIn.location = location;
            existingWaresIn.processingTime = processingTime;
            existingWaresIn.incomingItems = incomingItems;
        }

        public WaresIn GetWaresIn(int orderId)
        {
            var waresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == orderId);
            if (waresIn == null)
            {
                throw new InvalidOperationException("WaresIn not found.");
            }

            return waresIn;
        }

        public void DeleteWaresIn(int orderId)
        {
            var waresIn = _scheduledWaresIns.FirstOrDefault(wi => wi.orderId == orderId);
            if (waresIn == null)
            {
                throw new InvalidOperationException("WaresIn not found.");
            }

            _scheduledWaresIns.Remove(waresIn);
        }

        public IEnumerable<WaresIn> GetAllScheduledWaresIn()
        {
            return _scheduledWaresIns.AsReadOnly();
        }
    }
}

using jechFramework.Models;
using jechFramework.Interfaces;
using System.Collections.Generic;

namespace jechFramework.Services
{
    public class WaresOutService : IWaresOutService
    {
        private readonly List<WaresOut> _scheduledWaresOuts = new List<WaresOut>();
        private readonly List<RecurringWaresOut> _scheduledRecurringWaresOuts = new List<RecurringWaresOut>();

        public void ScheduleWaresOut(WaresOut waresOut)
        {
            _scheduledWaresOuts.Add(waresOut);
            // Ytterligere logikk for planlegging av vareuttak
        }

        public void ScheduleRecurringWaresOut(RecurringWaresOut recurringWaresOut)
        {
            _scheduledRecurringWaresOuts.Add(recurringWaresOut);
            // Ytterligere logikk for planlegging av gjentagende vareuttak
        }
    }
}

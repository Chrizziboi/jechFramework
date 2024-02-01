using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using jechFramework.Models;

namespace jechFramework.Interfaces
{
    public interface IWaresOutService
    {
        void ScheduleWaresOut(WaresOut waresOut);
        void ScheduleRecurringWaresOut(RecurringWaresOut recurringWaresOut);
    }
}
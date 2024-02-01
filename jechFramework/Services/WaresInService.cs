using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace jechFramework.Services
{
    public class WaresInService : IWaresInService
    {
        private readonly List<WaresIn> _scheduledWaresIns = new List<WaresIn>();
        private readonly List<RecurringOrder> _scheduledRecurringOrders = new List<RecurringOrder>();

        /// <summary>
        /// Planlegger en enkeltgangs innkomst av varer til lageret.
        /// </summary>
        /// <param name="waresIn">Inneholder detaljer om vareinnkomsten som skal planlegges.</param>
        public void ScheduleWaresIn(WaresIn waresIn)
        {
            // Her kan du legge til logikk for å validere waresIn-detaljer.
            _scheduledWaresIns.Add(waresIn);
            // Implementer funksjonalitet for å faktisk planlegge vareinnkomsten.
        }

        /// <summary>
        /// Planlegger gjentagende innkomster av varer til lageret basert på et definert mønster (f.eks. daglig eller ukentlig).
        /// </summary>
        /// <param name="recurringOrder">Inneholder detaljer om den gjentagende vareinnkomsten som skal planlegges.</param>
        public void ScheduleRecurringOrder(RecurringOrder recurringOrder)
        {
            // Her kan du legge til logikk for å validere recurringOrder-detaljer.
            _scheduledRecurringOrders.Add(recurringOrder);
            // Implementer funksjonalitet for å håndtere gjentagende vareinnkomster.
        }
    }
}

using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace jechFramework.Services
{
    public class PickupService : IPickupService
    {
        private readonly List<Pickup> _scheduledPickups = new List<Pickup>();
        private readonly List<RecurringPickup> _scheduledRecurringPickups = new List<RecurringPickup>();

        /// <summary>
        /// Planlegger en enkeltgangshenting av varer fra lageret på et spesifisert tidspunkt.
        /// </summary>
        /// <param name="pickup">Inneholder detaljer om hentingen som skal planlegges.</param>
        public void SchedulePickup(Pickup pickup)
        {
            // Her kan du legge til logikk for å validere pickup-detaljer.
            _scheduledPickups.Add(pickup);
            // Implementer funksjonalitet for å faktisk planlegge hentingen.
        }

        /// <summary>
        /// Planlegger gjentagende hentinger av varer fra lageret basert på et definert mønster (f.eks. daglig eller ukentlig).
        /// </summary>
        /// <param name="recurringPickup">Inneholder detaljer om den gjentagende hentingen som skal planlegges.</param>
        public void ScheduleRecurringPickup(RecurringPickup recurringPickup)
        {
            // Her kan du legge til logikk for å validere recurringPickup-detaljer.
            _scheduledRecurringPickups.Add(recurringPickup);
            // Implementer funksjonalitet for å håndtere gjentagende hentinger.
        }
    }
}

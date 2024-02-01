using jechFramework.Models;
using jechFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    public class PickupService : IPickupService
    {
        /// <summary>
        /// Planlegger en enkeltgangshenting av varer fra lageret på et spesifisert tidspunkt.
        /// Denne metoden vil, når den implementeres, registrere detaljene for en planlagt henting,
        /// inkludert hvilke varer som skal hentes og når.
        /// </summary>
        /// <param name="pickup">Inneholder detaljer om hentingen som skal planlegges.</param>
        public void SchedulePickup(Pickup pickup)
        {
            // Tom implementasjon for nå.
        }

        /// <summary>
        /// Planlegger gjentagende hentinger av varer fra lageret basert på et definert mønster (f.eks. daglig eller ukentlig).
        /// Denne metoden vil håndtere oppsettet for regelmessige hentinger,
        /// og sikre at disse hentinger skjer i henhold til det spesifiserte mønsteret.
        /// </summary>
        /// <param name="recurringPickup">Inneholder detaljer om den gjentagende hentingen som skal planlegges.</param>
        public void ScheduleRecurringPickup(RecurringPickup recurringPickup)
        {
            // Tom implementasjon for nå.
        }
    }
}

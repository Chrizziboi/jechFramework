using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Interfaces
{
    /// <summary>
    /// Definerer tjenester for planlegging av henting av varer fra lageret.
    /// </summary>
    public interface IPickupService
    {
        /// <summary>
        /// Planlegger en enkeltgangshenting av en vare fra lageret.
        /// Denne metoden skal, når den implementeres, håndtere logikken for å registrere
        /// og organisere henting av en vare på et spesifikt tidspunkt.
        /// </summary>
        /// <param name="pickup">Detaljer om den enkelte hentingen som skal skje.</param>
        void SchedulePickup(Pickup pickup);

        /// <summary>
        /// Planlegger gjentagende hentinger av varer fra lageret.
        /// Denne metoden skal, når implementert, håndtere oppsettet av regelmessige hentinger
        /// basert på et definert mønster (som daglig eller ukentlig).
        /// </summary>
        /// <param name="recurringPickup">Detaljer om den gjentagende hentingen som skal planlegges.</param>
        void ScheduleRecurringPickup(RecurringPickup recurringPickup);

        // Eventuelle andre nødvendige metoder relatert til henting av varer...
    }
}

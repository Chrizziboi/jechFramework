using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Interfaces
{
    /// <summary>
    /// Definerer tjenester for planlegging av mottak av varer til lageret.
    /// </summary>
    public interface IWaresInService
    {
        /// <summary>
        /// Planlegger en enkeltgangs mottak av varer til lageret.
        /// Denne metoden skal, når den implementeres, håndtere logikken for å registrere
        /// og organisere mottak av en vare på et spesifikt tidspunkt.
        /// </summary>
        /// <param name="waresIn">Detaljer om varemottaket som skal skje.</param>
        void ScheduleWaresIn(WaresIn waresIn);

        /// <summary>
        /// Planlegger gjentagende mottak av varer til lageret.
        /// Denne metoden skal, når implementert, håndtere oppsettet av regelmessige mottak
        /// basert på et definert mønster (som daglig eller ukentlig).
        /// </summary>
        /// <param name="recurringOrder">Detaljer om den gjentagende varemottaket som skal planlegges.</param>
        void ScheduleRecurringOrder(RecurringOrder recurringOrder);

        // Eventuelle andre nødvendige metoder relatert til mottak av varer...
    }
}
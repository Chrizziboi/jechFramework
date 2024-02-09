using jechFramework.Models;
using System;

namespace jechFramework.Interfaces
{
    /// <summary>
    /// Definerer tjenester for planlegging og håndtering av mottak av varer til lageret.
    /// </summary>
    public interface IWaresInService
    {
        /// <summary>
        /// Planlegger en enkeltgangs mottak av varer til lageret.
        /// </summary>
        /// <param name="waresIn">Detaljer om varemottaket som skal skje.</param>
        void ScheduleWaresIn(WaresIn waresIn);

        /// <summary>
        /// Planlegger gjentagende mottak av varer til lageret.
        /// </summary>
        /// <param name="recurringOrder">Detaljer om den gjentagende varemottaket som skal planlegges.</param>
        void ScheduleRecurringOrder(RecurringOrder recurringOrder);

        // Bruker kan vurdere å legge til metoder for å oppdatere, hente, eller slette planlagte ordre,
        // avhengig av systemets behov.

        // Eksempel:
        // void UpdateWaresIn(WaresIn waresIn);
        // WaresIn GetWaresIn(int orderId);
        // void DeleteWaresIn(int orderId);
    }
}

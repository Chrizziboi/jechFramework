using jechFramework.Models;
using jechFramework.Interfaces;
using jechFramework.Services;
using System;
using System.Collections.Generic;

namespace jechFramework.Interfaces
{
    public interface IWaresInService
    {
        // Planlegger mottak av en ny ordre basert på en liste av item IDs.
        // Denne metoden antar at du allerede har en måte å oversette internalId til faktiske Item objekter via IItemService.
        void ScheduleWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<int> itemInternalIds);

        // Oppdaterer en eksisterende ordre.
        void UpdateWaresIn(int orderId, DateTime scheduledTime, string location, TimeSpan processingTime, List<int> itemInternalIds);

        // Henter en spesifikk ordre basert på dens ID.
        WaresIn GetWaresIn(int orderId);

        // Sletter en spesifikk ordre basert på dens ID.
        void DeleteWaresIn(int orderId);

        // Henter en liste over alle planlagte ordre.
        IEnumerable<WaresIn> GetAllScheduledWaresIn();
    }
}
using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using jechFramework.Services;

/// <summary>
/// klasse for Håndtering av historikk.
/// </summary>
public class ItemHistoryService
{
   
    private static List<ItemHistory> itemHistoryList = new List<ItemHistory>();
    private static string logFilePath = "ItemMovements.log"; // Stien til loggfilen
    private WarehouseService warehouseService = new();


    /// <summary>
    /// Tom konstruktør for å gjøre kobling mellom klasser enklere.
    /// </summary>
    public ItemHistoryService()
    {
    }


    /// <summary>
    /// Oppdaterer historikken ved å lese loggfilen med den nye strukturen.
    /// </summary>
    public static void UpdateHistoryFromLog()
    {
        try
        {
            itemHistoryList.Clear();
            var logEntries = File.ReadAllLines(logFilePath);

            foreach (var entry in logEntries)
            {
                var fields = entry.Split(',');
                if (fields.Length == 4)
                {
                    try
                    {
                        int internalId = int.Parse(fields[0]);
                        int? oldZone = fields[1] != "NULL" ? int.Parse(fields[1]) : null;
                        int newZone = int.Parse(fields[2]);
                        DateTime dateTime = DateTime.Parse(fields[3]);

                        itemHistoryList.Add(new ItemHistory(internalId, oldZone, newZone, dateTime));
                    }
                    catch (ServiceException ex)
                    {
                        Console.WriteLine($"Unable to parse log entry: {entry}. Error: {ex.Message}");
                        // Fortsetter til neste logginnslag
                    }
                }
            }
        }
        catch (ServiceException ex)
        {
            throw new ServiceException("Error reading from log file.", ex);
            throw new ServiceException("An unexpected error occurred while updating history from log.", ex);
        }
    }


    /// <summary>
    /// Funksjon for å hente en liste over alle elementhistorier.
    /// </summary>
    /// <returns> En liste over alle elementhistorier.</returns>
    public static List<ItemHistory> GetAll()
    {
        UpdateHistoryFromLog(); // Oppdaterer listen hver gang denne metoden kalles
        return itemHistoryList;
    }

    public void GetItemHistoryById(int warehouseId,int internalId)
    {
        try
        {
            UpdateHistoryFromLog(); 
            var singleItemHistory = itemHistoryList.Where(itemHistory => itemHistory.internalId == internalId).ToList();

            if (!singleItemHistory.Any())
            {
                Console.WriteLine($"No history found for the item with internal ID: {internalId}.");
                return;
            }

            
            Console.WriteLine($"(Internal ID: {internalId}):");
            Console.WriteLine($"{"Date",-20} | {"Old Location",-20} | {"New Location",-20}");
            Console.WriteLine(new string('-', 60)); 

            foreach (var itemHistory in singleItemHistory)
            {
                var oldZoneDisplay = itemHistory.oldZone.HasValue ? $"Zone {itemHistory.oldZone.Value}" : "None";
                var newZoneDisplay = $"Zone {itemHistory.newZone}";
                Console.WriteLine($"{itemHistory.dateTime,-20:dd.MM.yyyy HH:mm} | {oldZoneDisplay,-20} | {newZoneDisplay,-20}");
            }
        }
        catch (ServiceException ex)
        {
            Console.WriteLine($"Error retrieving item history for internal ID {internalId}: {ex.Message}");
        }
    }


    /// <summary>
    /// Tømmer historikkloggen ved å slette loggfilen hvis den eksisterer.
    /// </summary>
    public void ClearHistoryLog()
    {
        // Slett loggfilen hvis den eksisterer
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }
}

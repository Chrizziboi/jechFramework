using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using jechFramework.Services;

public class ItemHistoryService
{
    /// <summary>
    /// Funksjoner for itemHistoryService.cs
    /// </summary>
    private static List<ItemHistory> itemHistoryList = new List<ItemHistory>();
    private static string logFilePath = "ItemMovements.log"; // Stien til loggfilen

    /// <summary>
    /// Tom konstruktør for å gjøre kobling mellom klasser enklere.
    /// </summary>
    public ItemHistoryService()
    {
    }

    // Oppdaterer for å lese loggfilen med den nye strukturen
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
    /// Returnerer en liste over alle elementhistorier.
    /// </summary>
    /// <returns> En liste over alle elementhistorier. </returns>
    public static List<ItemHistory> GetAll()
    {
        UpdateHistoryFromLog(); // Oppdaterer listen hver gang denne metoden kalles
        return itemHistoryList;
    }

    public void GetItemHistoryById(int internalId)
    {
        try
        {
            UpdateHistoryFromLog(); // Ensure the list is updated based on the log file
            var singleItemHistory = itemHistoryList.Where(itemHistory => itemHistory.internalId == internalId).ToList();

            if (!singleItemHistory.Any())
            {
                Console.WriteLine($"No history was found for item with internalId: {internalId}.");
                return;
            }

            foreach (var itemHistory in singleItemHistory)
            {
                var oldZoneDisplay = itemHistory.oldZone.HasValue ? itemHistory.oldZone.Value.ToString() : "None";
                Console.WriteLine($"--------------\n - DateTime: {itemHistory.dateTime}.\n - Old Location: {oldZoneDisplay}.\n - New Location: {itemHistory.newZone}.\n");
            }
        }
        catch (ServiceException ex)
        {
            Console.WriteLine($"Error retrieving item history for internalId {internalId}: {ex.Message}");
            // Depending on your application's needs, you might want to log this error or notify someone.
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

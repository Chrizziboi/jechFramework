using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        itemHistoryList.Clear();
        var logEntries = File.ReadAllLines(logFilePath);

        foreach (var entry in logEntries)
        {
            var fields = entry.Split(',');
            if (fields.Length == 4)
            {
                var internalId = int.Parse(fields[0]);
                var oldZone = int.Parse(fields[1]); // Endret til int.Parse
                var newZone = int.Parse(fields[2]); // Endret til int.Parse
                var dateTime = DateTime.Parse(fields[3]);

                itemHistoryList.Add(new ItemHistory(internalId, oldZone, newZone, dateTime));
            }
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
        UpdateHistoryFromLog(); // Sørg for at listen er oppdatert basert på loggfilen
        List<ItemHistory> singleItemHistory = itemHistoryList.Where(itemHistory => itemHistory.internalId == internalId).ToList();

        if (!singleItemHistory.Any())
        {
            Console.WriteLine($"No history was found for the internalId that you were looking for.");
            return;
        }

        Console.WriteLine($"History for item with internalId: {internalId}");
        
        foreach (ItemHistory itemHistory in singleItemHistory)
        {
            Console.WriteLine($"--------------\n" +
                              $" - DateTime: {itemHistory.dateTime}.\n" +
                              $" - Old Location: {itemHistory.oldZone}.\n" +
                              $" - New Location: {itemHistory.newZone}.\n");

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

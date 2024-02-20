using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class ItemHistoryService
{
    private static List<ItemHistory> itemHistoryList = new List<ItemHistory>();
    private static string logFilePath = "ItemMovements.log"; // Stien til loggfilen

    public ItemHistoryService()
    {
    }

    // Oppdaterer for å lese loggfilen med den nye strukturen
    public static void UpdateHistoryFromLog()
    {
        itemHistoryList.Clear(); // Tømmer listen for å unngå duplikater
        var logEntries = File.ReadAllLines(logFilePath);

        foreach (var entry in logEntries)
        {
            var fields = entry.Split(',');
            if (fields.Length == 4) // Antar format: internalId,oldLocation,newLocation,dateTime
            {
                var internalId = int.Parse(fields[0]);
                var oldLocation = fields[1];
                var newLocation = fields[2];
                var dateTime = DateTime.Parse(fields[3]);

                itemHistoryList.Add(new ItemHistory(internalId, oldLocation, newLocation, dateTime));
            }
        }
    }

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
            Console.WriteLine("No history was found for the internalId you were looking for.");
            return;
        }

        Console.WriteLine($"History for item with internalId: {internalId}");
        
        foreach (ItemHistory itemHistory in singleItemHistory)
        {
            Console.WriteLine($"--------------\n" +
                              $" - DateTime: {itemHistory.dateTime}.\n" +
                              $" - Old Location: {itemHistory.oldLocation}.\n" +
                              $" - New Location: {itemHistory.newLocation}.\n");

        }
    }
    public void ClearHistoryLog()
    {
        // Slett loggfilen hvis den eksisterer
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }

}

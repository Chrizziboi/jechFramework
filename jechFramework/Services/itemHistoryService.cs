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
    public static string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ItemMovements.log");
    private WarehouseService warehouseService = new();


    /// <summary>
    /// Tom konstruktør for å gjøre kobling mellom klasser enklere.
    /// </summary>
    public ItemHistoryService()
    {
    }


    public static void EnsureLogfileExists()
    {
        Console.WriteLine("Checking log file at: " + logFilePath);
        if (!File.Exists(logFilePath))
        {
            using (var stream = File.Create(logFilePath))
            {
                // Kun opprette filen, ingen skriving av tekst som "Log File Created"
                stream.Close(); // Lukker strømmen etter opprettelse
            }
            Console.WriteLine("Log file created: " + logFilePath);
        }
        else
        {
            Console.WriteLine("Log file already exists.");
        }
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
            if (!File.Exists(logFilePath))
            {
                Console.WriteLine($"Log file not found: {logFilePath}");
                return;
            }

            var logEntries = File.ReadAllLines(logFilePath);
            foreach (var entry in logEntries)
            {
                var fields = entry.Split(',');
                if (fields.Length == 4 && fields[0].All(char.IsDigit)) // Sjekker om første felt er et tall
                {
                    try
                    {
                        int internalId = int.Parse(fields[0]);
                        int? oldZone = fields[1] != "NULL" ? int.Parse(fields[1]) : null;
                        int newZone = int.Parse(fields[2]);
                        DateTime dateTime = DateTime.Parse(fields[3]);
                        itemHistoryList.Add(new ItemHistory(internalId, oldZone, newZone, dateTime));
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Unable to parse log entry: {entry}. Error: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from log file: {ex.Message}");
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

    public List<ItemHistory> GetItemHistoryById(int warehouseId, int internalId)
    {
        List<ItemHistory> results = new List<ItemHistory>();

        try
        {
            UpdateHistoryFromLog(); // Oppdaterer historikklisten fra loggfilen

            // Sjekker først om listen har noen elementer i det hele tatt
            if (itemHistoryList == null || !itemHistoryList.Any())
            {
                Console.WriteLine("The item history list is empty or null.");
                return results; // Returnerer tom liste hvis ingen historikk er tilgjengelig
            }

            // Filtrer historikken basert på internID
            var singleItemHistory = itemHistoryList.Where(itemHistory => itemHistory.internalId == internalId).ToList();

            // Sjekker om filtreringen resulterte i noen treff
            if (!singleItemHistory.Any())
            {
                Console.WriteLine($"No history found for the item with internal ID: {internalId}.");
                return results; // Returner tom liste hvis ingen historikk er funnet
            }

            // Legger til gyldige historikkelementer til resultatlisten
            foreach (var itemHistory in singleItemHistory)
            {
                results.Add(itemHistory);
                Console.WriteLine($"History added for item ID {internalId}: Zone {itemHistory.oldZone} to {itemHistory.newZone} at {itemHistory.dateTime}.");
            }
        }
        catch (Exception ex) // Endret til Exception for å fange alle typer unntak, ikke bare ServiceException
        {
            Console.WriteLine($"An error occurred while retrieving item history for internal ID {internalId}: {ex.Message}");
        }

        // Returnerer listen av historikkobjekter, som nå kan være tom eller fylt basert på funnene
        return results;
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

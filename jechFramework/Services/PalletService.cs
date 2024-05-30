using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Models;

namespace jechFramework.Services
{
    /// <summary>
    /// Klasse for håndtering av Palle-objekter.
    /// </summary>
    public class PalletService
    {

        public int totalPallets = 0;

        public PalletService()
        {

        }

        /// <summary>
        /// Funksjon for å legge til paller i varehuset. Denne brukes i WaresInService automatisk.
        /// </summary>
        /// <param name="incomingItems">En liste over innkommende varer til kunder etc.</param>
        public void AddPallets(List<Item> incomingItems) 
        {
            try
            {
                
                int totalQuantity = incomingItems.Sum(item => item.quantity);

                if (totalQuantity > 0)
                {
                    int numberOfPallets = totalQuantity / 30; // Beregner antallet paller

                    Console.WriteLine(numberOfPallets);

                    if (totalQuantity % 30 != 0) // Sjekker om det er en rest etter deling
                    {
                        
                        numberOfPallets++;
                        
                        Console.WriteLine($"Added {numberOfPallets} pallets to Warehouse.");
                        Console.WriteLine($"Current total pallets: {totalPallets}.");
                       
                    }
                        
                        totalPallets += numberOfPallets;

                }

                else 
                {   
                    Console.WriteLine("No new pallets from incoming delivery.");
                }

                    
            }
            catch (ServiceException ex) 
            {
                Console.WriteLine(ex.Message);
                
            }
            
        }

        /// <summary>
        /// Funksjon for å fjerne paller fra lageret som brukes ved WaresOutService automatisk.
        /// </summary>
        /// <param name="outgoingItems">En liste over utgående varer til kunder etc.</param>
       public void RemovePallets(List<Item> outgoingItems)
       {
            try
            {

                int totalQuantity = outgoingItems.Sum(item => item.quantity);

                if (totalQuantity > 0)
                {
                    int numberOfPallets = totalQuantity / 30; // Beregner antallet paller

                    if (totalQuantity % 30 != 0) // Sjekker om det er en rest etter deling
                    {

                        numberOfPallets++;
                    }

                    // Sørger for at antallet paller ikke blir negativt
                    if (numberOfPallets <= 0)
                    {
                        numberOfPallets = 0;
                        Console.WriteLine("No pallets removed from outgoing delivery.");
                    }

                    if(numberOfPallets > totalPallets) 
                    {
                        int palletQuantity = 20;
                        OrderPallets(palletQuantity);
                    }                

                    totalPallets -= numberOfPallets;

                    Console.WriteLine($"Removed {numberOfPallets} pallets from Warehouse.");
                    Console.WriteLine($"Current total pallets: {totalPallets}.");
                }

                else
                {
                    Console.WriteLine("No pallets removed from outgoing delivery.");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
        /// <summary>
        /// Funskjon for å telle antall paller i varehuset.
        /// </summary>
        /// <returns> Returnerer antall paller i varehuset. </returns>
       public int CountPallets()
       {
            Console.WriteLine(totalPallets);
            return totalPallets;
       }

        /// <summary>
        /// Funksjon for å bestille nye paller hvis det ikke er nok på lageret.
        /// </summary>
        /// <param name="palletQuantity">Kvantitet for antall paller som skal bestilles.</param>
        public void OrderPallets(int palletQuantity)
        {
            //int palletQuantity = 0;
            totalPallets += palletQuantity;
            Console.WriteLine($"New Pallets have been ordered for the warehouse with amount: {palletQuantity}.");
                
        }
    }  
}

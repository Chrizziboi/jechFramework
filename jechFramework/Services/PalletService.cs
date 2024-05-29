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

        private WarehouseService warehouseService = new();

        private Warehouse warehouse = new();

        private ItemService itemService;

        private WaresOutService waresOutService = new();

        private WaresInService waresInService;

        public int totalPallets = 0;

        public PalletService()
        {


        }

        public void AddPallets(List<Item> incomingItems) 
        {
            try
            {
                
                int totalQuantity = incomingItems.Sum(item => item.quantity);

                if (totalQuantity > 0)
                {
                    int numberOfPallets = totalQuantity / 30; // Beregner antallet paller

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

                        numberOfPallets--;

                        Console.WriteLine($"Removed {numberOfPallets} pallets from Warehouse.");
                        Console.WriteLine($"Current total pallets: {totalPallets}.");

                    }

                    totalPallets -= numberOfPallets;

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
       
       //public int CountPallets()
       //{
       //
       //}
    }  //
}

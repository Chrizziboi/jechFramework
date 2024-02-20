using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    public class ItemHistoryService
    {

        private static List<ItemHistory> itemHistoryList = new List<ItemHistory>();

        //ItemHistoryService newItemHistoryService = new ItemHistoryService();

        public ItemHistoryService()
        {

        }     
            

        public static List<ItemHistory> GetAll()
        {  
            return itemHistoryList; 

        }
                
        
        public void GetItemHistoryById(int internalId)
        {
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
                             $" - Location: {itemHistory.location}. \n" +
                             $" - DateTime: {itemHistory.dateTime}. \n");

            }


        }

       //public string PrintHistory(int internalId)
       //{
       //
       //    Console.WriteLine($"");
       //}

    }
}

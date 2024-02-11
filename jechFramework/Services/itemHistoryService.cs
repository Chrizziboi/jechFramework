using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    internal class itemHistoryService
    {

        private static List<ItemHistory> itemHistoryList = new List<ItemHistory>();

        itemHistoryService newItemHistoryService = new itemHistoryService();

        public itemHistoryService()
        {

        }

        
            

        public static List<ItemHistory> GetAll()
        {  
            return itemHistoryList; 

        }
                
        
        public void GetItemHistoryById(int internalId)
        {
            List<ItemHistory> singleItemHistory = new List<ItemHistory> ();

            foreach (ItemHistory itemHistory in itemHistoryList)
                singleItemHistory.Add(itemHistory);
            Console.WriteLine($"History of all Items with internalId:\n" +
                              $" - {internalId}. \n" +
                              $" - {singleItemHistory}. \n");

        }

    

    }
}

using jechFramework.Interfaces;
using jechFramework.Models;
using jechFramework.Services;

namespace program
{
    internal class program1
    {
        static void Main(string[] args)
        {
           
            ItemService itemService = new ItemService();
            ItemHistoryService itemHistory = new ItemHistoryService();

            //WaresInService waresService = new WaresInService();

            //ItemService.newItem = new ItemService(internalId: 1, name: "Sample Item", weight: 10, Type: "TypeA", storageType, "Warehouse", DateTime.Now);
            itemService.CreateItem(internalId: 1, location: "H1", dateTime: DateTime.Now);

            itemService.MoveItemToLocation(newItem.FindItemByInternalId, newLocation);
            List<int> itemInternalIds = new List<int>() { newItem.internalId };

            ItemHistoryService.GetItemHistoryById(newItem);
            WaresInService.ScheduleWaresIn(orderId: 1, scheduledTime: DateTime.Now.AddHours(1), location: newLocation, processingTime: TimeSpan.FromHours(2), itemInternalIds);

            



        }
    }
}

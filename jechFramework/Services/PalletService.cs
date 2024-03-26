using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Interfaces;
using jechFramework.Models;

namespace jechFramework.Services
{
    public class PalletService
    {

        //var warehouseServiceInstance = new WarehouseService();
        //var shelfInstance = new Shelf();

        private readonly WarehouseService warehouseServiceInstance = new();

        private readonly Warehouse warehouseInstance = new();

        private readonly Shelf shelfInstance = new();

       //private int lastPalletId = 0;
       //private List<Pallet> pallets = new List<Pallet>();
       //
       //public void AddPallet(int itemId, int warehouseId)
       //{
       //    lastPalletId++;
       //
       //    string itemIdString = itemId.ToString();
       //    string warehouseIdString = warehouseId.ToString();
       //    string zoneIdString = "5";
       //
       //    string combinedIdString = warehouseIdString + zoneIdString + lastPalletId.ToString() + itemIdString;
       //
       //    int combinedId = int.Parse(combinedIdString);
       //
       //    Pallet newPallet = new Pallet
       //    {
       //        internalPalletId = combinedId
       //    };
       //
       //    pallets.Add(newPallet);
       //}
        // internalPalletId + Item.internalId
        //private static List<Pallet> palletList = new List<Pallet>();
        //
        // int x = 5;
        // int y = 10;
        // int sum;
        // sum = Convert.ToInt32("" + x + y);
        //
        /// <summary>
        ///  en funksjon for å kunne kalkulere hvor mye en pall vil veie uten at man skulle trenge å veie hver pall.
        ///  det er en forventing til hva slags pall som skal brukes, denne forventningen er satt til europaller som veier fra 20 til 25 kilo.
        ///  det er derfor vi setter 25 før kalkulasjonen.
        /// </summary>
        /// <returns></returns>
        //public int CalculateWeightForPallet(int internalPalletId)
        //{
        //    int totalWeight = 25;
        //
        //    var shelf = 
        //
        //    var pallet = shelfInstance.palletList.FirstOrDefault(pallet => pallet.internalPalletId == internalPalletId);
        //
        //    if (pallet != null)
        //    {
        //        foreach (var item in pallet.itemList)
        //        {
        //            totalWeight += item.weight;
        //        }
        //                                       
        //    }
        //       
        //
        //    
        //    return totalWeight;
        //}
        //
        //public void MovePalletToLocation(int internalPalletId, string newLocation)
        //{
        //    var pallet = shelfInstance.palletList.FirstOrDefault(pallet => pallet.internalPalletId == internalPalletId);
        //
        //    if (internalPalletId != null)
        //    {
        //        pallet.palletLocation = newLocation;
        //
        //    }
        //
        //}
        //
        public void countPalletInWarehouse(int warehouseId, WarehouseService warehouseService)
        {
            //var warehouse = WarehouseService.warehouseList.FirstOrDefault(Warehouse => Warehouse.warehouseId == warehouseId);

            try
            { 
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                //var zone = warehouseInstance.zoneList;
                //var shelf = shelfInstance.palletList;
                
                if (warehouse == null) 
                {
                    throw new ServiceException($"A warehouse with id {warehouseId} could not be found.");   
                }
                int palletCount = 0;
                foreach(var zone in warehouse.zoneList)
                {
                    foreach(var shelf in zone.shelves)
                    {
                        palletCount += shelf.palletList.Count;
                    }
                }

                Console.WriteLine($"Number of pallets in warehouse: {palletCount}");
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);     
            }
        }
    } 
}

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

        //var warehouseServiceInstance = new WarehouseService();
        //var shelfInstance = new Shelf();

        private readonly WarehouseService warehouseServiceInstance = new();

        private readonly Warehouse warehouseInstance = new();

        private readonly WaresOutService WaresOutServiceInstance = new();

        public List<Pallet> palletList = new();

        private readonly PalletService palletService;

        /// <summary>
        /// Metoden for å fjerne en palle fra palleList
        /// </summary>
        /// <param name="palletList">Liste over pallet</param>
        public void addPallet(List<Pallet> palletList)
        {
            try
            { 
                if(palletList == null)
                {
                    throw new ServiceException($"palletList not found or does not exist");
                }

                Pallet newPallet = new Pallet();
                palletList.Add(newPallet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Meotden for å fjerne en palle fra palleList
        /// </summary>
        /// <param name="palletList">Liste over paller</param>
        public void removePallet(List<Pallet> palletList)
        {
            try
            {
                if (palletList == null)
                {
                    //håndtere tilfelle det palletList er null
                    throw new ServiceException($"palletList not found or does not exist");
                }

                if (palletList.Any())
                {
                    Pallet palletToRemove = palletList.First();
                    palletList.Remove(palletToRemove);
                }
                else
                {
                    throw new ServiceException($"PalletList is empty");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private int lastPalletId = 0;
        //
        //public void AddPallet(int warehouseId, int internalId)
        //{
        //    lastPalletId++;
        //  
        //    string internalIdString = internalId.ToString();
        //    string warehouseIdString = warehouseId.ToString();
        //    string zoneIdString = "5";
        //
        //    string combinedIdString = warehouseIdString + zoneIdString + lastPalletId.ToString() + internalIdString;
        //
        //    int combinedId = int.Parse(combinedIdString);
        //
        //    Pallet newPallet = new Pallet
        //    {
        //        internalPalletId = combinedId
        //    };
        //
        //    shelfInstance.palletList.Add(newPallet);
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

  
        
        public void countPalletInWarehouse(int warehouseId, List<Pallet> palletList, List<Warehouse> warehouseList)
        {
            //var warehouse = WarehouseService.warehouseList.FirstOrDefault(Warehouse => Warehouse.warehouseId == warehouseId);

            try
            {
                if (warehouseList.Any(w => w.warehouseId == warehouseId))
                {
                    var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                    //var zone = warehouseInstance.zoneList;
                    //var shelf = shelfInstance.palletList;
                
                   
                    int palletCount = palletList.Count;
                    Console.WriteLine($"Number of pallets in warehouse: {palletCount}");
                }
                else
                {
                    throw new ServiceException($"Warehouse with Id {warehouseId} not found in warehouseList");
                }

                
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);     
            }
        }
    } 
}

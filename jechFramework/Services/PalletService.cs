using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jechFramework.Interfaces;
using jechFramework.Models;

namespace jechFramework.Services
{
    internal class PalletService : IPalletService
    {

        public PalletService()
        {
            List<Pallet> palletList = new List<Pallet>();
        }

        public List<Pallet> palletList;

        /// <summary>
        ///  en funksjon for å kunne kalkulere hvor mye en pall vil veie uten at man skulle trenge å veie hver pall.
        ///  det er en forventing til hva slags pall som skal brukes, denne forventningen er satt til europaller som veier fra 20 til 25 kilo.
        ///  det er derfor vi setter 25 før kalkulasjonen.
        /// </summary>
        /// <returns></returns>
        public int CalculateWeightForPallet(int internalPalletId)
        {
            int totalWeight = 25;

            Pallet pallet = palletList.FirstOrDefault(pallet => pallet.internalPalletId == internalPalletId);

            if (pallet != null)
            {
                foreach (var item in pallet.itemList)
                {
                    totalWeight += item.weight;
                }
                                               
            }
               

            
            return totalWeight;
        }

        public void MovePalletToLocation(int internalPalletId, string newLocation)
        {
            Pallet pallet = palletList.FirstOrDefault(pallet => pallet.internalPalletId == internalPalletId);

            if (internalPalletId != null)
            {
                pallet.palletLocation = newLocation;

            }

        }

    } 
}

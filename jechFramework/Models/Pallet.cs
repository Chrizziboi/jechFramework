namespace jechFramework.Models
{
    internal class Pallet
    {
        public int internalPalletId  //item + pallet id
        { get; private set; }

        public string palletName
        { get; private set; }

        public string palletDescription
        { get; private set; }

        public int palletWeight
        { get; private set; }

        public string palletLocation
        { get; private set; }

        public int palletQuantity
        { get; private set; }

        public List<Item> itemList 
        { get; private set; }

        private Pallet(int internalPalletId, string palletName)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;

        }
        private Pallet(int internalPalletId, string palletName, string palletDescription, string palletLocation, List<Item> itemList) 
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletLocation = palletLocation;
            this.itemList = itemList;

        }

        public Pallet(int internalPalletId, string palletName, string palletDescription, int palletWeight, string palletLocation, int palletQuantity, List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletWeight = palletWeight;
            this.palletLocation = palletLocation;
            this.palletQuantity = palletQuantity;
            this.itemList = itemList;

        }

        public int CalculateWeightForPallet()
        {
            int totalWeight = 0;
            foreach (var I in itemList)
            {
                totalWeight += I.weight;

            }
            return totalWeight;
        }



    }
}

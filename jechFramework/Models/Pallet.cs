using static jechFramework.Models.Pallet;

namespace jechFramework.Models
{
    /// <summary>
    /// Pallet klassen er laget for masse-samlinger av en gitt Item for å kunne lagre paller av en vare
    /// som kommer inn, i stedet for å skrive 500 nye varer av prduktid 1 så kan man opprette en pall av produktid 1.
    /// </summary>
    public class Pallet
    {
        /// <summary>
        /// Henter eller setter den interne ID-en for pallen.
        /// </summary>
        public int internalPalletId { get; set; } = 10;
        //Pallet.internalPalletId + Item.internalId

        /// <summary>
        /// Henter eller setter navnet på pallen.
        /// </summary>
        public string? palletName { get; set; } = null;

        /// <summary>
        /// Henter eller setter beskrivelsen av pallen.
        /// </summary>
        public string? palletDescription { get; set; } = null;

        /// <summary>
        /// Henter eller setter vekten på pallen.
        /// </summary>
        public int? palletWeight { get; set; }

        /// <summary>
        /// Henter eller setter lagringstypen for pallen.
        /// </summary>
        public PalletStorageType palletStorageType { get; set; } = PalletStorageType.Standard;

        /// <summary>
        /// Henter eller setter lokasjonen for pallen.
        /// </summary>
        public string? palletLocation { get; set; } = null;

        /// <summary>
        /// Henter eller setter listen over varer på pallen.
        /// </summary>
        public List<Item> itemList { get; set; }

        /// <summary>
        /// Henter eller setter kvantiteten av varer på pallen.
        /// </summary>
        public int itemQuantity { get; set; } = 0;

        /// <summary>
        /// Representerer forskjellige lagringstyper for paller.
        /// </summary>
        public enum PalletStorageType
        {
            Standard,
            ClimateControlled,
            HighValue,
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen.
        /// </summary>
        public Pallet() 
        {
        
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen med spesifisert ID og navn.
        /// </summary>
        /// <param name="internalPalletId">Den interne ID-en for pallen.</param>
        /// <param name="palletName">Navnet på pallen.</param>
        public Pallet(int internalPalletId, string palletName)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen med spesifisert ID, navn og liste over varer.
        /// </summary>
        /// <param name="internalPalletId">Den interne ID-en for pallen.</param>
        /// <param name="palletName">Navnet på pallen.</param>
        /// <param name="itemList">Listen over varer på pallen.</param>
        public Pallet(int internalPalletId, string palletName, List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="internalPalletId">Den interne ID-en for pallen.</param>
        /// <param name="palletWeight">Vekten på pallen.</param>
        /// <param name="palletStorageType">Lagringstypen for pallen.</param>
        /// <param name="palletLocation">Lokasjonen for pallen.</param>
        /// <param name="itemQuantity">Kvantiteten av varer på pallen.</param>
        /// <param name="itemList">Listen over varer på pallen.</param>
        public Pallet(
            int internalPalletId, 
            int palletWeight, 
            PalletStorageType palletStorageType, 
            string palletLocation, 
            int itemQuantity, 
            List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletWeight = palletWeight;
            this.palletStorageType = palletStorageType;
            this.palletLocation = palletLocation;
            this.itemQuantity = itemQuantity;
            this.itemList = itemList;

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="internalPalletId">Den interne ID-en for pallen.</param>
        /// <param name="palletName">Navnet på pallen.</param>
        /// <param name="palletDescription">Beskrivelsen av pallen.</param>
        /// <param name="palletLocation">Lokasjonen for pallen.</param>
        /// <param name="itemList">Listen over varer på pallen.</param>
        private Pallet(
            int internalPalletId, 
            string palletName, 
            string palletDescription, 
            string palletLocation, 
            List<Item> itemList) 
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletLocation = palletLocation;
            this.itemList = itemList;

        }


        /// <summary>
        /// Initialiserer en ny instans av <see cref="Pallet"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="internalPalletId">Den interne ID-en for pallen.</param>
        /// <param name="palletName">Navnet på pallen.</param>
        /// <param name="palletDescription">Beskrivelsen av pallen.</param>
        /// <param name="palletWeight">Vekten på pallen.</param>
        /// <param name="palletStorageType">Lagringstypen for pallen.</param>
        /// <param name="palletLocation">Lokasjonen for pallen.</param>
        /// <param name="itemQuantity">Kvantiteten av varer på pallen.</param>
        /// <param name="itemList">Listen over varer på pallen.</param>
        public Pallet(
            int internalPalletId, 
            string palletName, 
            string palletDescription, 
            int palletWeight, 
            PalletStorageType palletStorageType, 
            string palletLocation, 
            int itemQuantity, 
            List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletWeight = palletWeight;
            this.palletStorageType = palletStorageType;
            this.palletLocation = palletLocation;
            this.itemQuantity = itemQuantity;
            this.itemList = itemList;

        }

           
    }
}

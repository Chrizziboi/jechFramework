using static jechFramework.Models.Pallet;

namespace jechFramework.Models
{
    public class Pallet
    {
        /// <summary>
        /// Pallet klassen er laget for masse-samlinger av en gitt Item for å kunne lagre paller av en vare
        /// som kommer inn, i stedet for å skrive 500 nye varer av prduktid 1 så kan man opprette en pall av produktid 1.
        /// </summary>
        public int internalPalletId { get; set; } = 10;
        //Pallet.internalPalletId + Item.internalId

        public string? palletName { get; set; } = null;

        public string? palletDescription { get; set; } = null;

        public int? palletWeight { get; set; }

        public PalletStorageType palletStorageType { get; set; } = PalletStorageType.Standard;

        public string? palletLocation { get; set; } = null;

        public List<Item> itemList { get; set; }

        public int itemQuantity { get; set; } = 0;

        public enum PalletStorageType
        {
            Standard,
            ClimateControlled,
            HighValue
        }

        public Pallet() 
        {
        
        }

        public Pallet(int internalPalletId, string palletName)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;

        }

        public Pallet(int internalPalletId, string palletName, List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;

        }

        public Pallet(int internalPalletId, int palletWeight, PalletStorageType palletStorageType, string palletLocation, int itemQuantity, List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletWeight = palletWeight;
            this.palletStorageType = palletStorageType;
            this.palletLocation = palletLocation;
            this.itemQuantity = itemQuantity;
            this.itemList = itemList;

        }


        private Pallet(int internalPalletId, string palletName, string palletDescription, string palletLocation, List<Item> itemList) 
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletLocation = palletLocation;
            this.itemList = itemList;

        }


        /// <summary>
        /// Konstruktør for pallet.
        /// </summary>
        /// <param name="internalPalletId">Her har tenkes det å bruke først Item sin interne id og deretter legge til en pall id for en pall med gitt vare(Item).</param> 
        /// <param name="palletName">Navn for en gitt pall.</param> 
        /// <param name="palletDescription">beskrivelse for pallen.</param> 
        /// <param name="palletWeight">Vekt for den gitte pallen. denne vekten pleier ikke å endre seg mye med tiden så hvis en pall med vareid 1 veier 750 så gjør den noe
        /// det i senere tid også.</param> 
        /// <param name="palletStorageType">storageType er for informasjon om hvilket type lager det burde/må stå på.</param> 
        /// <param name="palletLocation">Location for lokasjon på pallen i lageret.</param> 
        /// <param name="palletQuantity">Quantity for kvantitet av pallen. vurderes tatt vekk siden det opprettes en ny id for hver pall som blir
        /// opprettet.</param> 
        /// <param name="itemList">En liste med varer som er på pallen, denne må vurderes videre om den faktisk trengs i senere utvikling.</param> 
        public Pallet(int internalPalletId, string palletName, string palletDescription, int palletWeight, PalletStorageType palletStorageType, string palletLocation, int itemQuantity, List<Item> itemList)
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

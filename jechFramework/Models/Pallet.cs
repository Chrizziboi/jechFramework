namespace jechFramework.Models
{
    internal class Pallet
    {
        /// <summary>
        /// Pallet klassen er laget for masse-samlinger av en gitt Item for å kunne lagre paller av en vare
        /// som kommer inn, i stedet for å skrive 500 nye varer av prduktid 1 så kan man opprette en pall av produktid 1.
        /// </summary>
        public int internalPalletId  //item + pallet id
        { get; set; }

        public string? palletName
        { get; set; }

        public string? palletDescription
        { get; set; }

        public int palletWeight
        { get; set; }

        public string palletStorageType
        { get; set; }

        public string? palletLocation
        { get; set; }

        public int palletQuantity
        { get; set; }

        public List<Item> itemList 
        { get; set; }

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

        /// <summary>
        /// Kontruktør for pallet.
        /// </summary>
        /// <param name="internalPalletId">Her har tenkes det å bruke først Item sin interne id og derretter legge til en pall id for en pall med gitt vare(Item).</param> 
        /// <param name="palletName">Navn for en gitt pall.</param> 
        /// <param name="palletDescription">beskrivelse for pallen.</param> 
        /// <param name="palletWeight">Vekt for den gitte pallen. denne vekten pleier ikke å endre seg mye med tiden så hvis en pall med vareid 1 veier 750 så gjør den noe
        /// det i senere tid også.</param> 
        /// <param name="palletStorageType">storageType er for informasjon om hvilket type lager det burde/må stå på.</param> 
        /// <param name="palletLocation">Location for lokasjon på pallen i lageret.</param> 
        /// <param name="palletQuantity">Quantity for kvantitet av pallen. vurderes tatt vekk siden det opprettes en ny id for hver pall som blir
        /// opprettet.</param> 
        /// <param name="itemList">En liste med varer som er på pallen, denne må vurderes videre om den faktisk trengs i senere utvikling.</param> 
        public Pallet(int internalPalletId, string palletName, string palletDescription, int palletWeight,string palletStorageType, string palletLocation, int palletQuantity, List<Item> itemList)
        {
            this.internalPalletId = internalPalletId;
            this.palletName = palletName;
            this.palletDescription = palletDescription;
            this.palletWeight = palletWeight;
            this.palletStorageType = palletStorageType;
            this.palletLocation = palletLocation;
            this.palletQuantity = palletQuantity;
            this.itemList = itemList;

        }

     
    }
}

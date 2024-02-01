namespace jechFramework.Models
{
    internal class Pallet
    {
        /// <summary>
        /// Pallet klassen er laget for nasse samlinger av en gitt Item for å kunne lagre paller av en vare
        /// som kommer inn, i stedet for å skrive 500 nye varer av prduktid 1 så kan man opprette en pall av produktid 1.
        /// </summary>
        public int internalPalletId  //item + pallet id
        { get; private set; }

        public string palletName
        { get; private set; }

        public string palletDescription
        { get; private set; }

        public int palletWeight
        { get; private set; }

        public string palletStorageType
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

        /// <summary>
        /// Kontruktør for pallet.
        /// </summary>
        /// <param name="internalPalletId"></param> Her har tenkes det å bruke først Item sin interne id og derretter legge til en pall id for en pall med gitt vare(Item).
        /// <param name="palletName"></param> Navn for en gitt pall.
        /// <param name="palletDescription"></param> beskrivelse for pallen.
        /// <param name="palletWeight"></param> Vekt for den gitte pallen. denne vekten pleier ikke å endre seg mye med tiden så hvis en pall med vareid 1 veier 750 så gjør den noe
        /// det i senere tid også.
        /// <param name="palletStorageType"></param> storageType er for informasjon om hvilket type lager det burde/må stå på.
        /// <param name="palletLocation"></param> Location for lokasjon på pallen i lageret.
        /// <param name="palletQuantity"></param> Quantity for kvantitet av pallen. vurderes tatt vekk siden det opprettes en ny id for hver pall som blir
        /// opprettet.
        /// <param name="itemList"></param> En liste med varer som er på pallen, denne må vurderes videre om den faktisk trengs i senere utvikling
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

        /// <summary>
        ///  en intern funksjon for å kunne kalkulere hvor mye en pall vil veie uten at man skulle trenge å veie hver pall.
        ///  det er en forventing til hva slags pall som skal brukes, denne forventningen er satt til europaller som veier fra 20 til 25 kilo.
        ///  det er derfor vi plusser på 25 etter kalkulasjonen.
        /// </summary>
        /// <returns></returns>
        public int CalculateWeightForPallet()
        {
            int totalWeight = 0;
            foreach (var I in itemList)

            {
                totalWeight += (I.weight) + 25;

            }
            return totalWeight;
        }
    }
}

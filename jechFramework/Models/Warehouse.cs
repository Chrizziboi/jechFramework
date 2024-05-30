namespace jechFramework.Models
{
    /// <summary>
    /// Representerer et varehus.
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// Henter eller setter ID-en for varehuset.
        /// </summary>
        public int warehouseId { get; set; }

        /// <summary>
        /// Henter eller setter navnet på varehuset.
        /// </summary>
        public string warehouseName { get; set; }

        /// <summary>
        /// Henter eller setter beskrivelsen av varehuset.
        /// </summary>
        public string warehouseDescription { get; set; }

        /// <summary>
        /// Henter eller setter adressen til varehuset.
        /// </summary>
        public string warehouseAddress { get; set; }

        /// <summary>
        /// Henter eller setter byen hvor varehuset ligger.
        /// </summary>
        public string warehouseCity { get; set; }

        /// <summary>
        /// Henter eller setter landet hvor varehuset ligger.
        /// </summary>
        public string warehouseCountry { get; set; }

        /// <summary>
        /// Henter eller setter kapasiteten til varehuset.
        /// </summary>
        public int warehouseCapacity { get; set; } = 5;

        /// <summary>
        /// Henter eller setter bredden på pallehylle i varehuset.
        /// </summary>
        public int warehousePalletShelfWidth { get; set; } = 80;

        /// <summary>
        /// Henter eller setter listen over soner i varehuset.
        /// </summary>
        public List<Zone> zoneList { get; set; } = new List<Zone>();

        /// <summary>
        /// Henter eller setter listen over ansatte i varehuset.
        /// </summary>
        public List<Employee> employeeList { get; set; } = new List<Employee>();

        /// <summary>
        /// Henter eller setter listen over varer i varehuset.
        /// </summary>
        public List<Item> itemList { get; set; } = new List<Item>(); // nye created item list

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen.
        /// </summary>
        public Warehouse()
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen med spesifisert ID, navn og kapasitet.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="warehouseName">Navnet til varehuset.</param>
        /// <param name="warehouseCapacity">Kapasiteten til varehuset.</param>
        public Warehouse(int warehouseId, string warehouseName, int warehouseCapacity)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
            this.warehouseCapacity = warehouseCapacity;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="warehouseName">Navnet til varehuset.</param>
        /// <param name="warehouseDescription">Beskrivelsen av varehuset.</param>
        /// <param name="warehouseAddress">Adressen til varehuset.</param>
        /// <param name="warehouseCity">Byen hvor varehuset ligger.</param>
        /// <param name="warehouseCountry">Landet hvor varehuset ligger.</param>
        /// <param name="warehouseCapacity">Kapasiteten til varehuset.</param>
        public Warehouse(
            int warehouseId, 
            string warehouseName, 
            string warehouseDescription, 
            string warehouseAddress, 
            string warehouseCity, 
            string warehouseCountry, 
            int warehouseCapacity)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity;

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="warehouseName">Navnet til varehuset.</param>
        /// <param name="warehouseDescription">Beskrivelsen av varehuset.</param>
        /// <param name="warehouseAddress">Adressen til varehuset.</param>
        /// <param name="warehouseCity">Byen hvor varehuset ligger.</param>
        /// <param name="warehouseCountry">Landet hvor varehuset ligger.</param>
        /// <param name="warehouseCapacity">Kapasiteten til varehuset.</param>
        public Warehouse(
            string warehouseName, 
            string warehouseDescription, 
            string warehouseAddress, 
            string warehouseCity, 
            string warehouseCountry, 
            int warehouseCapacity)
        {
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="warehouseName">Navnet til varehuset.</param>
        /// <param name="warehouseAddress">Adressen til varehuset.</param>
        /// <param name="warehouseCity">Byen hvor varehuset ligger.</param>
        /// <param name="warehouseCountry">Landet hvor varehuset ligger.</param>
        /// <param name="warehouseCapacity">Kapasiteten til varehuset.</param>
        public Warehouse(
            string warehouseName, 
            string warehouseAddress, 
            string warehouseCity, 
            string warehouseCountry, 
            int warehouseCapacity)
        { 
            this.warehouseName = warehouseName;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity; 
        }


        /// <summary>
        /// Initialiserer en ny instans av <see cref="Warehouse"/> klassen med spesifiserte parametere.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="warehouseName">Navnet til varehuset.</param>
        /// <param name="warehouseDescription">Beskrivelsen av varehuset.</param>
        /// <param name="warehouseAddress">Adressen til varehuset.</param>
        /// <param name="warehouseCity">Byen hvor varehuset ligger.</param>
        /// <param name="warehouseCountry">Landet hvor varehuset ligger.</param>
        /// <param name="warehouseCapacity">Kapasiteten til varehuset.</param>
        /// <param name="warehousePalletShelfWidth">Bredden på pallehylle i varehuset.</param>
        public Warehouse(
            int warehouseId, 
            string warehouseName, 
            string warehouseDescription, 
            string warehouseAddress, 
            string warehouseCity, 
            string warehouseCountry, 
            int warehouseCapacity, 
            int warehousePalletShelfWidth)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity;
            this.warehousePalletShelfWidth = warehousePalletShelfWidth;

        }
    }
}
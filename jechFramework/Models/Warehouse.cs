using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class Warehouse
    {
        ///<summary>
        /// Initialiserer variabler med get og set metoder
        /// </summary>
        public int warehouseId { get; set; }
        public string warehouseName { get; set; }
        public string warehouseDescription { get; set; }
        public string warehouseAddress { get; set; }
        public string warehouseCity { get; set; }
        public string warehouseCountry { get; set; }
        public int warehouseCapacity { get; set; } = 5;
        // maks antall soner  per varehus.
        public int warehousePalletShelfWidth { get; set; } = 80;
        

        public List<Zone> zoneList { get; set; } = new List<Zone>(); 
        /// <param name="zoneList">Dette er en liste for de forskjellige sonene i et varehus</param>
        /// 
        public List<Employee> employeeList { get; set; } = new List<Employee>();
        /// <param name="employeeList">Listen over alle ansatte i et varehus.</param>
        /// 
        public List<Item> itemList { get; set; } = new List<Item>(); // nye created item list
        /// <param name="ItemList">Listen over alle Item-objekter i et varehus.</param>
        /// 

        public Warehouse()
        {

        }

        public Warehouse(int warehouseId, string warehouseName, int warehouseCapacity)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
            this.warehouseCapacity = warehouseCapacity;
        }

       
        public Warehouse(int warehouseId, string warehouseName, string warehouseDescription, string warehouseAddress, 
            string warehouseCity, string warehouseCountry, int warehouseCapacity)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity;

        }
        

        public Warehouse(string warehouseName, string warehouseDescription, string warehouseAddress, 
            string warehouseCity, string warehouseCountry, int warehouseCapacity)
        {
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity;
        }


        public Warehouse(string warehouseName, string warehouseAddress, string warehouseCity, 
            string warehouseCountry, int warehouseCapacity)
        { 
            this.warehouseName = warehouseName;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCity = warehouseCity;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCapacity = warehouseCapacity; 
        }      
        

        /// <summary>
        /// Konstruktør for varehus.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        /// <param name="warehouseName">Dette er for å kunne ha navn på et varehus</param>
        /// <param name="warehouseDescription">Denne brukes for å gi en beskrivelse for varehuset om man ønsker</param>
        /// <param name="warehouseAddress">Dette er bare en adresse til et varehus så man vet hvor det er</param>
        /// <param name="warehouseCity">Dette brukes for å vise hvilken by varehuset ligger i</param>
        /// <param name="warehouseCountry">Dette viser hvilken land et varehus ligger i</param>
        /// <param name="warehouseCapacity">Dette er den totale kapasiteten til et varehus</param>
        public Warehouse(int warehouseId, string warehouseName, string warehouseDescription, string warehouseAddress, string warehouseCity, string warehouseCountry, int warehouseCapacity, int warehousePalletShelfWidth)
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
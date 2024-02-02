using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Warehouse
    {
        public string warehouseId { get; set; }
        public string warehouseName { get; set; }
        public string warehouseDescription { get; set; }
        public string warehouseAddress {  get; set; }
        public string warehouseCity { get; set; }
        public string warehouseCountry { get; set; }
        public int warehouseCapacity { get; set; }
        public int warehousePalletShelfWidth { get; set; } = 80;
        public string warehouseType { get; set; } //kjølelager, fryselager osv
        public string warehouseZone { get; set; } //for å kunne dele et lager inn i soner
        public int warehouseHasEmployeeId { get; set; }
        public Warehouse(string warehouseName, string warehouseDescription, string warehouseAddress, 
            string warehouseCity, string warehouseCountry, int warehouseCapacity)
        {
            this.warehouseName = warehouseName;
            this.warehouseDescription = warehouseDescription;
            this.warehouseAddress = warehouseAddress;
            this.warehouseCountry = warehouseCountry;
            this.warehouseCity = warehouseCity;
            this.warehouseCapacity = warehouseCapacity;
        }
        


    }
}

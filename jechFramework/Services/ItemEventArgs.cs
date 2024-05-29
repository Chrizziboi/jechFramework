using jechFramework.Models;
using jechFramework.Services;
using System;

namespace jechFramework.Services
{
    /// <summary>
    /// Event arguments for hendelser relatert til varer.
    /// </summary>
    public class ItemEventArgs : EventArgs
    {
        public Item item = new();

        /// <summary>
        /// Henter ID-en til varehuset.
        /// </summary>
        public int warehouseId { get; private set; }

        /// <summary>
        /// Henter den interne ID-en til varen.
        /// </summary>
        public int internalId { get; private set; }

        /// <summary>
        /// Henter den eksterne ID-en til varen, hvis tilgjengelig.
        /// </summary>
        public int? externalId { get; private set; }

        /// <summary>
        /// Henter navnet på varen.
        /// </summary>
        public string? name { get; private set; } = string.Empty;

        /// <summary>
        /// Henter eller setter lagringstypen til varen.
        /// </summary>
        public StorageType storageType { get; set; }

        /// <summary>
        /// Henter ID-en til sonen.
        /// </summary>
        public int zoneId { get; private set; }

        /// <summary>
        /// Henter datoen og tidspunktet for hendelsen.
        /// </summary>
        public DateTime dateTime { get; private set; }

        /// <summary>
        /// Henter kvantiteten av varen.
        /// </summary>
        public int quantity { get; private set; }

        /// <summary>
        /// Henter ID-en til den nye sonen.
        /// </summary>
        public int newZone { get; private set; }

        /// <summary>
        /// Henter ID-en til den gamle sonen.
        /// </summary>
        public int oldZone { get; private set; }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="zoneId">ID-en til sonen.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        /// <param name="externalId">Den eksterne ID-en til varen, hvis tilgjengelig.</param>
        /// <param name="name">Navnet på varen.</param>
        /// <param name="storageType">Lagringstypen til varen.</param>
        /// <param name="dateTime">Datoen og tidspunktet for hendelsen.</param>
        /// <param name="quantity">Kvantiteten av varen.</param>
        /// <param name="newZone">ID-en til den nye sonen.</param>
        /// <param name="oldZone">ID-en til den gamle sonen.</param>
        public ItemEventArgs(
            int warehouseId, 
            int zoneId, 
            int internalId, 
            int? externalId, 
            string? name, 
            StorageType storageType, 
            DateTime DateTime, 
            int quantity, 
            int newZone, 
            int oldZone)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.storageType = storageType;
            this.zoneId = zoneId;
            this.dateTime = DateTime;
            this.quantity = quantity;
            this.newZone = newZone;
            this.oldZone = oldZone;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen for vareopprettelseshendelser.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        /// <param name="externalId">Den eksterne ID-en til varen, hvis tilgjengelig.</param>
        /// <param name="name">Navnet på varen.</param>
        /// <param name="storageType">Lagringstypen til varen.</param>
        public ItemEventArgs(int warehouseId, int internalId, int? externalId, string name, StorageType storageType)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.storageType = storageType;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen for varetilførselshendelser.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="zoneId">ID-en til sonen.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        /// <param name="dateTime">Datoen og tidspunktet for hendelsen.</param>
        /// <param name="quantity">Kvantiteten av varen.</param>
        public ItemEventArgs(int warehouseId, int zoneId, int internalId, DateTime dateTime, int quantity = 1)
        {
            this.warehouseId = warehouseId;
            this.zoneId = zoneId;
            this.internalId = internalId;
            this.dateTime = dateTime;
            this.quantity = quantity;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen for varefjerninghendelser.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        /// <param name="quantity">Kvantiteten av varen.</param>
        public ItemEventArgs(int warehouseId, int internalId, ushort quantity)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.quantity = quantity;
        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen for varens flyttehendelser.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        /// <param name="newZone">ID-en til den nye sonen.</param>
        public ItemEventArgs(int warehouseId, int internalId, int newZone)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.newZone = newZone;

        }

        /// <summary>
        /// Initialiserer en ny instans av <see cref="ItemEventArgs"/>-klassen for vareoppslagshendelser.
        /// </summary>
        /// <param name="warehouseId">ID-en til varehuset.</param>
        /// <param name="internalId">Den interne ID-en til varen.</param>
        public ItemEventArgs(int warehouseId, int internalId)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
        }
    }
}

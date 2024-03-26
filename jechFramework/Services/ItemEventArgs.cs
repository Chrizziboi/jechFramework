using jechFramework.Services;
using System;

namespace jechFramework.Services


{
    public class ItemEventArgs : EventArgs
    {
        public int warehouseId { get; private set; }
        public int internalId { get; private set; }
        public int? externalId { get; private set; }
        public string name { get; private set; }
        public Enum storagetype { get; private set; }
        public int zoneId { get; private set; }
        public DateTime dateTime { get; private set; }
        public int quantity { get; private set; }
        public int newZone { get; private set; }
        public int oldZone { get; private set; }

        public ItemEventArgs(int warehouseId, int internalId, int? externalId, string name, Enum storagetype, int zoneId, DateTime DateTime, int quantity, int newZone, int oldZone)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.storagetype = storagetype;
            this.zoneId = zoneId;
            this.dateTime = DateTime;
            this.quantity = quantity;
            this.newZone = newZone;
            this.oldZone = oldZone;
        }

        public ItemEventArgs(int warehouseId, int internalId, int? externalId, string name, Enum storagetype)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.storagetype = storagetype;
        }

        public ItemEventArgs(int internalId, int zoneId, DateTime dateTime, int warehouseId, int quantity)
        {
            this.internalId = internalId;
            this.zoneId = zoneId;
            this.dateTime = dateTime;
            this.warehouseId = warehouseId;
            this.quantity = quantity;
        }

        public ItemEventArgs(int warehouseId, int internalId)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
        }

        public ItemEventArgs(int warehouseId, int internalId, int newZone)
        {
            this.warehouseId = warehouseId;
            this.internalId = internalId;
            this.newZone = newZone;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Item
    {
        public int internalId 
        { get; private set; }

        public int externalId
        { get; private set; }

        public string name 
        { get; private set; }

        public string description 
        { get; private set; }

        public int weight
        { get; private set; }

        public string type 
        { get; private set; }

        public string storageType
        { get; private set; }

        public string location 
        { get; private set; }

        public int quantity 
        { get; private set; }

        public Item(int internalId, string name, string type, string storageType) 
        {
            this.internalId = internalId;
            this.name = name;
            this.type = type;
            this.storageType = storageType;

        }
        public Item(int internalId, int externalId, string name, string description, string type, string storageType)
        { 
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.description = description;
            this.type = type;
            this.storageType = storageType;
            
        }
 
        public Item(int internalId, int externalId, string name, string description, string type, string storageType, string location, int quantity)
        { 
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.description = description;
            this.type = type;
            this.storageType = storageType;
            this.location = location;
            this.quantity = quantity;

        }

        public int getWeight()
        {
            this.weight = weight;
            return weight;
        }
        public int getQuantity()
        {
            this.quantity = quantity;
            return quantity;
        }

        public int CalculateWeightForItem()
        {
            int totalWeight = 0;
            {

                totalWeight = weight * quantity;
                return totalWeight;

            }
        }
    }
}

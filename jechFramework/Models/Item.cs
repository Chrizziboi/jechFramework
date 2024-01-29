using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    internal class Item
    {
        /// <summary>
        /// Item klassen er laget for å kunne opprette gjenstander på lageret.
        /// </summary>

        public int internalId 
        { get; private set; }

        public int externalId
        { get; private set; }

        public string name 
        { get; private set; }

        public string description 
        { get;  set; }

        public int weight
        { get; private set; }

        public string type 
        { get; private set; }

        public string storageType
        { get; set; }

        public string location 
        { get;  set; }

        public int quantity 
        { get;  set; }

        /// <summary>
        /// det er laget konstruktører for å kunne opprette objekter av klassen item.
        /// </summary>
        /// 
        public Item(int internalId, string name, int weight, string type, string storageType) 
        {
            this.internalId = internalId;
            this.name = name;
            this.weight = weight;
            this.type = type;
            this.storageType = storageType;

        }
        public Item(int internalId, int externalId, string name, string description, int weight, string type, string storageType)
        { 
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.type = type;
            this.storageType = storageType;
            
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="internalId"></param> har brukt internalId for å vise iden på produktet internt for varehuset.
        /// <param name="externalId"></param> externalId for tilfellene man skulle trenge leverandør sin produkt id.
        /// <param name="name"></param> navn er for å kunne gi navn til en gitt vare.
        /// <param name="description"></param> description for å kunne gi en lett beskrivelse av vare og eventuelt ekstra informasjon.
        /// <param name="type"></param> type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.
        /// <param name="storageType"></param> storageType er for informasjon om hvilket type lager det burde stå på.
        /// <param name="location"></param> Lcation er for å vise hvor i lageret det ligger.
        /// <param name="quantity"></param> Quantity er for hvor mange av den gitte varen det er på lager.
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

        //public int getWeight()
        //{
        //    this.weight = weight;
        //    return weight;
        //}
        //public int getQuantity()
        //{
        //    this.quantity = quantity;
        //    return quantity;
        //}

    }
}

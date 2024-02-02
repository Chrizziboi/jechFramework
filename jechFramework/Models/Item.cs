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
        { get; set; }

        public int externalId
        { get; set; }

        public string name 
        { get; set; }

        public string description 
        { get;  set; }

        public int weight
        { get; set; }

        public string type 
        { get; set; }

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
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param> 
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param> 
        /// <param name="name">navn er for å kunne gi navn til en gitt vare.</param> 
        /// <param name="description">description for å kunne gi en lett beskrivelse av vare og eventuelt ekstra informasjon.</param> 
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param> 
        /// <param name="storageType">storageType er for informasjon om hvilket type lager det burde/må stå på.</param> 
        /// <param name="location">Lcation er for å vise hvor i lageret det ligger.</param> 
        /// <param name="quantity">Quantity er for hvor mange av den gitte varen det er på lager.</param> 
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

    }
}

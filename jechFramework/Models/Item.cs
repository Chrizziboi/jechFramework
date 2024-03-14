using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    public class Item
    {
        /// <summary>
        /// Item klassen er laget for å kunne opprette gjenstander på lageret.
        /// </summary>

        public int internalId { get; set; }

        public int? externalId { get; set; } = null;

        public string name { get; set; }

        public string? description { get; set; } = null;

        public int weight { get; set; }

        public string type { get; set; }

        public string storageType { get; set; }

        public int? zoneId { get; set; } = 0;

        public int quantity { get; set; } = 1;

        public DateTime dateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen uten parametere.
        /// </summary>
        public Item()
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 1 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        public Item(int internalId)
        {
            this.internalId = internalId;

        }


        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 2 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="location">Location er for å vise hvor i lageret det ligger.</param>
        public Item(int internalId, int zoneId)
        {
            this.internalId = internalId;
            this.zoneId = zoneId;

        }


        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 3 parameter.
        /// </summary>
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param>
        /// <param name="name">name er for å kunne gi navn til en gitt vare.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        public Item(int externalId, string name, string type)
        {
            this.externalId = externalId;
            this.name = name;
            this.type = type;

        }


        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 4 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param>
        /// <param name="name">name er for å kunne gi navn til en gitt vare.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        public Item(int internalId, int externalId, string name, string type)
        { 
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.type = type;

        }


        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 6 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="name">name er for å kunne gi navn til en gitt vare.</param>
        /// <param name="weight">weight er for vekten på en gitt gjenstand.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        /// <param name="storageType">>storageType er for informasjon om hvilket type lager det burde/må stå på.</param>
        /// <param name="dateTime">dateTime er for registrering og historikk for Item.cs objekter.</param>
        public Item(int internalId, string name, int weight, string type, string storageType, DateTime dateTime)
        {
            this.internalId = internalId;
            this.name = name;
            this.weight = weight;
            this.type = type;
            this.storageType = storageType;
            this.dateTime = dateTime;

        }

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med 7 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="externalId"></param>
        /// <param name="name">name er for å kunne gi navn til en gitt vare.</param>
        /// <param name="description">description for å kunne gi en lett beskrivelse av vare og eventuelt ekstra informasjon.</param>
        /// <param name="weight">weight er for vekten på en gitt gjenstand.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        /// <param name="storageType">storageType er for informasjon om hvilket type lager det burde/må stå på.</param>
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
        /// Initialiserer en ny instans av Item-klassen med 10 parameter.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param> 
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param> 
        /// <param name="name">navn er for å kunne gi navn til en gitt vare.</param> 
        /// <param name="description">description for å kunne gi en lett beskrivelse av vare og eventuelt ekstra informasjon.</param> 
        /// <param name="weight">weight er for vekten på en gitt gjenstand.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param> 
        /// <param name="storageType">storageType er for informasjon om hvilket type lager det burde/må stå på.</param> 
        /// <param name="location">Lcation er for å vise hvor i lageret det ligger.</param> 
        /// <param name="quantity">Quantity er for hvor mange av den gitte varen det er på lager.</param>
        /// <param name="dateTime">dateTime er for registrering og historikk for Item.cs objekter.</param>
        public Item(int internalId, int externalId, string name, string description, int weight, string type, string storageType, int zoneId, int quantity, DateTime dateTime)
        {
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.type = type;
            this.storageType = storageType;
            this.zoneId = zoneId;
            this.quantity = quantity;
            this.dateTime = DateTime.Now;

        }

        
    }
}

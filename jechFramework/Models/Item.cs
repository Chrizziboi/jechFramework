using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Models
{
    /// <summary>
    /// Item-klassen representerer en gjenstand i lageret. Den inneholder informasjon som intern og ekstern ID, navn, beskrivelse, vekt, type, lagertype, sone ID, mengde og tidspunkt for siste endring.
    /// </summary>
    public class Item
    {
        public int shelfId;

        /// <summary>
        /// Henter eller setter intern ID for varen. 
        /// </summary>
        public int internalId { get; set; }

        /// <summary>
        /// Henter eller setter ekstern ID for varen, hvis tilgjengelig.
        /// </summary>
        public int? externalId { get; set; } = null;

        /// <summary>
        /// Henter eller setter navnet på varen.
        /// </summary>
        public string? name { get; set; } = string.Empty;

        /// <summary>
        /// Henter eller setter en beskrivelse av varen.
        /// </summary>
        public string? description { get; set; } = null;

        /// <summary>
        /// Henter eller setter vekten til varen.
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// Henter eller setter typen av varen.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Henter eller setter lagertypen for varen.
        /// </summary>
        public StorageType storageType { get; set; }

        /// <summary>
        /// Henter eller setter sone ID for varen. Kan være null.
        /// </summary>
        public int? zoneId { get; set; } = 0;

        /// <summary>
        /// Henter eller setter mengden av varen.
        /// </summary>
        public ushort quantity { get; set; } = 1;

        /// <summary>
        /// Henter eller setter tidspunktet for siste endring av varen.
        /// </summary>
        public DateTime dateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen uten parametere.
        /// </summary>
        public Item()
        {

        }

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med fire parametere.
        /// </summary>
        /// <param name="internalId">Intern ID for å identifisere varen i lageret.</param>
        /// <param name="externalId">Ekstern ID for leverandørens produkt ID.</param>
        /// <param name="name">Navn på varen.</param>
        /// <param name="type">Type av varen, for eksempel mikroklut kontra vanlig klut.</param>
        public Item(int internalId, int externalId, string name, StorageType storageType)
        {
            this.internalId = internalId;
            this.externalId = externalId;
            this.name = name;
            this.storageType = storageType;
        }

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med fire parametere.
        /// </summary>
        /// <param name="internalId">Intern ID for å identifisere varen i lageret.</param>
        /// <param name="name">Navn på varen.</param>
        /// <param name="storageType">Lagringstype for varen.</param>
        /// <param name="zoneId">Sone ID hvor varen er plassert.</param>
        public Item(int internalId, string name, StorageType storageType, int zoneId)
        { 
            this.internalId= internalId;
            this.name = name;
            this.storageType = storageType;
            this.zoneId = zoneId;
        }


        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med seks parametere.
        /// </summary>
        /// <param name="internalId">Intern ID for å identifisere varen i lageret.</param>
        /// <param name="name">Navn på varen.</param>
        /// <param name="weight">Vekt av varen.</param>
        /// <param name="type">Type av varen, for eksempel mikroklut kontra vanlig klut.</param>
        /// <param name="storageType">Lagringstype for varen.</param>
        /// <param name="dateTime">Tidspunkt for registrering og historikk for varen.</param>
        public Item(int internalId, string name, int weight, string type, StorageType storageType, DateTime dateTime)
        {
            this.internalId = internalId;
            this.name = name;
            this.weight = weight;
            this.type = type;
            this.storageType = storageType;
            this.dateTime = dateTime;
        }

        /// <summary>
        /// Initialiserer en ny instans av Item-klassen med syv parametere.
        /// </summary>
        /// <param name="internalId">Intern ID for å identifisere varen i lageret.</param>
        /// <param name="externalId">Ekstern ID for leverandørens produkt ID.</param>
        /// <param name="name">Navn på varen.</param>
        /// <param name="description">Beskrivelse av varen for ekstra informasjon.</param>
        /// <param name="weight">Vekt av varen.</param>
        /// <param name="type">Type av varen, for eksempel mikroklut kontra vanlig klut.</param>
        /// <param name="storageType">Lagringstype for varen.</param>
        public Item(
            int internalId, 
            int externalId, 
            string name, 
            string description, 
            int weight, 
            string type, 
            StorageType storageType)
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
        /// Initialiserer en ny instans av Item-klassen med ti parametere.
        /// </summary>
        /// <param name="internalId">Intern ID for å identifisere varen i lageret.</param> 
        /// <param name="externalId">Ekstern ID for leverandørens produkt ID.</param> 
        /// <param name="name">Navn på varen.</param> 
        /// <param name="description">Beskrivelse av varen for ekstra informasjon.</param> 
        /// <param name="weight">Vekt av varen.</param>
        /// <param name="type">Type av varen, for eksempel mikroklut kontra vanlig klut.</param> 
        /// <param name="storageType">Lagringstype for varen.</param> 
        /// <param name="zoneId">Sone ID hvor varen er plassert.</param> 
        /// <param name="quantity">Antall av varen på lager.</param>
        /// <param name="dateTime">Tidspunkt for registrering og historikk for varen.</param>
        public Item(
            int internalId, 
            int externalId, 
            string name, 
            string description, 
            int weight, 
            string type, 
            StorageType storageType, 
            int zoneId, 
            ushort quantity, 
            DateTime dateTime)
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

using jechFramework.Interfaces;
using jechFramework.Models;
using System.IO;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner for Item.cs
    /// </summary>
    public class ItemService
    {
        private WarehouseService warehouseService;
        private static List<Item> ItemsInWarehouseList = new List<Item>();
        private static List<Item> createdItemsList = new List<Item>();
        public ItemService(WarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }


        /// <summary>
        ///  Funksjon for å opprette og legge til nye varer i createdItems listen, altså en liste over Items-gjenstander 
        ///  som er opprettet i varehuset.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param>
        /// <param name="name">navn er for å kunne gi navn til en gitt vare.</param>
        /// <param name="type">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        /// <exception cref="InvalidOperationException">En exception for et tilfelle der en Item med samme internalId blir opprettet.</exception>
        public void CreateItem(int internalId, int? externalId, string name, string type)
        {
            if (createdItemsList.Any(i => i.internalId == internalId))
            {
                throw new InvalidOperationException("Item with this internal ID already exists in the creation list.");
            }

            var newItem = new Item
            {
                internalId = internalId,
                externalId = externalId,
                name = name,
                type = type
            };

            createdItemsList.Add(newItem);
        }


        /// <summary>
        /// Funksjon for å legge en Item-gjenstand inn i lageret(wareHouseItemList), altså som legges inn i lageret sin liste.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="location">Lcation er for å vise hvor i lageret det ligger.</param>
        /// <param name="dateTime">dateTime er for registrering og historikk for Item.cs objekter.</param>
        /// <exception cref="InvalidOperationException">En exception som oppstår når det ikke finnes noen Items-objekter med det gitte internalId.</exception>
        public void AddItem(int internalId, int zoneId, DateTime dateTime, int warehouseId)
        {
            var itemToAdd = createdItemsList.FirstOrDefault(i => i.internalId == internalId);
            if (itemToAdd == null)
            {
                throw new InvalidOperationException("No item with this internal ID exists in the creation list.");
            }

            var zone = warehouseService.FindZoneById(warehouseId, zoneId); // Bruker instansen 'warehouseService'
            if (zone == null)
            {
                throw new InvalidOperationException($"Zone with ID {zoneId} in Warehouse {warehouseId} does not exist.");
            }

            itemToAdd.zoneId = zoneId;
            itemToAdd.dateTime = dateTime;
            ItemsInWarehouseList.Add(itemToAdd);

            LogItemMovement(new ItemHistory(internalId, null, zoneId, dateTime)); // Juster denne linjen basert på faktiske parametere forventet av ItemHistory
        }


        /// <summary>
        /// Funksjon for å fjerne en Item-gjenstand ut fra lageret.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveItem(int internalId)
        {
            var item = ItemsInWarehouseList.FirstOrDefault(i => i.internalId == internalId);
            if (item != null)
            {
                if (item.quantity > 1)
                {
                    // Reduserer antallet med én hvis det er mer enn én på lager.
                    item.quantity -= 1;
                }
                else
                {
                    // Fjerner varen helt hvis dette var den siste.
                    ItemsInWarehouseList.Remove(item);
                    Console.WriteLine($"Item with internal ID {internalId} is now out of stock and has been removed from the warehouse list.");
                    // Her kan du implementere ytterligere logikk for varsling eller håndtering av utsolgte varer.
                }
            }
            else
            {
                throw new InvalidOperationException("Item not found.");
            }
        }


        /// <summary>
        /// Funksjon for å kunne flytte en Item-gjenstand til en ny lokasjon i lageret.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="newLocation">newLocation er for å sette en ny lokasjon på en Item gjenstand.</param>
        public void MoveItemToLocation(int internalId, int newLocation)
            {
             var item = ItemsInWarehouseList.FirstOrDefault(i => i.internalId == internalId);
             if (item != null)
             {
                var oldLocation = item.zoneId; // Lagrer den gamle lokasjonen før oppdatering
                item.zoneId = newLocation; // Oppdaterer lokasjonen
                item.dateTime = DateTime.Now; // Oppdaterer tidsstempel

                // Oppretter en ny ItemHistory oppføring
                var itemHistory = new ItemHistory(internalId, oldLocation, newLocation, item.dateTime);

                // Anta at vi har en metode for å logge denne historikken til en fil
                LogItemMovement(itemHistory);

                Console.WriteLine($"Item {internalId} has been moved from {oldLocation} to {newLocation} at {item.dateTime}");
                }
                else
                {
                    Console.WriteLine($"Item with internal ID {internalId} not found. No action taken.");
                }
        }


        /// <summary>
        /// En hjelpefunksjon for å logge bevegelsen til en fil.
        /// </summary>
        /// <param name="itemHistory">En klasse for å registrere historikk for Item-gjenstander.</param>
        private void LogItemMovement(ItemHistory itemHistory)
        {
            // Format for logginnlegget
            var logEntry = $"{itemHistory.internalId},{itemHistory.oldZone},{itemHistory.newZone},{itemHistory.dateTime}\n";

            // Spesifiser stien til loggfilen
            var logFilePath = "ItemMovements.log";

            // Skriver logginnlegget til filen
            File.AppendAllText(logFilePath, logEntry);
        }
        /// <summary>
        /// Funksjon for å telle antall Item-gjenstander med gitt internalId.
        /// </summary>
        /// <param name="internalId">navn er for å kunne gi navn til en gitt vare.</param>
        /// <returns>Returnerer en Int for alle registrerte Item-gjenstander med gitt internalId.</returns>
        public int FindHowManyItemsInItemList()
        {

            int countedItems = ItemsInWarehouseList.Count();

            // var returnedCountedItems = $"The counted items for item: {internalId} is: {countedItems}.";

            return countedItems;

        }


        /// <summary>
        /// Funksjon for å telle kvantiteten for et gitt Item-gjenstand.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <returns>Returnerer en Int med totalt antall Kvantitet.</returns>
        public int FindHowManyItemQuantityByInternalId(int internalId)
        {
            // Summerer 'quantity' for alle varer som matcher den gitte 'internalId'
            int totalQuantity = ItemsInWarehouseList.Where(item => item.internalId == internalId)
                                                 .Sum(item => item.quantity);
            return totalQuantity;
        }



        /// <summary>
        /// Inne i ItemService-klassen
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        public void FindItemByInternalIdInWarehouse(int internalId)
        {

            var item = ItemsInWarehouseList.FirstOrDefault(item => item.internalId == internalId);

            if (item == null)
            {
                throw new InvalidOperationException($"An Item with id{item} could not be found.");
            }

            else if (ItemsInWarehouseList == null)
            {
                throw new InvalidOperationException($"");
            }

            Console.WriteLine($"item Id: {item.internalId}\n" +
                              $"item Name: {item.name}\n" +
                              $"item : {item.type}\n");

        }


        public void ClearWarehouseData()
        {
            ItemsInWarehouseList.Clear(); // Tømmer listen over varer i varehuset
            createdItemsList.Clear();          // Eventuelt tøm andre lister eller ressurser om nødvendig
        }


        public int? GetLocationByInternalId(int internalId)
        {
            var item = ItemsInWarehouseList.FirstOrDefault(i => i.internalId == internalId);
            // Returnerer zoneId som int?. Hvis ingen zoneId, returnerer null.
            return item?.zoneId;
        }



        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}
        public bool ItemExists(int internalId)
        {
            return createdItemsList.Any(i => i.internalId == internalId);
        }
    }
       
}
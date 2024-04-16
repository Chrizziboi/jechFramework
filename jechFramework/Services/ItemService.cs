
using jechFramework.Models;
using jechFramework.Services;
using System.IO;
using System.Xml.Linq;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner for Item.cs
    /// </summary>
    public class ItemService
    {
        private WarehouseService warehouseService = new();

        private Warehouse warehouse; // Referanse til Warehouse objekt for å aksessere ItemList
        private Zone zone;
        // private static List<Item> ItemsInZoneList = new List<Item>();
        // private static List<Item> ItemList = new List<Item>();

        public delegate void ItemCreatedEventHandler(int warehouseId, int internalId, int? externalId, string name, Enum storagetype);
        public delegate void ItemAddedEventHandler(int internalId, int zoneId, DateTime dateTime, int warehouseId, int quantity = 1);
        public delegate void ItemRemovedEventHandler(int warehouseId, int internalId);
        public delegate void ItemMovedEventHandler(int warehouseId, int internalId, int newZone);

        public event EventHandler<ItemEventArgs> ItemCreated;
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved;
        public event EventHandler<ItemEventArgs> ItemMoved;

        public void OnItemCreated(int warehouseId, int internalId, int? externalId, string name, StorageType storageType)
        {
            ItemCreated?.Invoke(this, new ItemEventArgs(warehouseId, internalId, externalId, name, storageType));
        }

        public void OnItemAdded(int internalId, int zoneId, DateTime dateTime, int warehouseId, int quantity = 1)
        {
            ItemAdded?.Invoke(this, new ItemEventArgs(internalId, zoneId, dateTime, warehouseId, quantity));
        }

        public void OnItemRemoved(int warehouseId, int internalId)
        {
            ItemRemoved?.Invoke(this, new ItemEventArgs(warehouseId, internalId));
        }

        public void OnItemMoved(int warehouseId, int internalId, int newZone)
        {
            ItemMoved?.Invoke(this, new ItemEventArgs(warehouseId, internalId, newZone));
        }

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
        /// <param name="itemPacketList">type er ment for foreksempel at et gitt produkt er en mikroklut, og ikke en vanlig klut.</param>
        /// <exception cref="ServiceException">En exception for et tilfelle der en Item med samme internalId blir opprettet.</exception>
        public void CreateItem(int warehouseId, int internalId, int? externalId, string name, StorageType storageType)
        {
            try
            {
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found. Skipping item creation.");
                }

                if (warehouse.itemList.Any(i => i.internalId == internalId))
                {
                    throw new ServiceException($"Item with internal ID {internalId} already exists. Skipping item creation.");
                }

                var newItem = new Item
                {
                    internalId = internalId,
                    externalId = externalId,
                    name = name,
                    storageType = storageType
                };

                warehouse.itemList.Add(newItem);

                OnItemCreated(warehouseId, internalId, externalId, name, storageType);

                Console.WriteLine($"New item created: {name} with ID: {internalId} and Storagetype: {storageType} in warehouse ID: {warehouseId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating item: {ex.Message}");
            }
        }




        /// <summary>
        /// Funksjon for å legge en Item-gjenstand inn i lageret(wareHouseItemList), altså som legges inn i lageret sin liste.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="zoneId">zoneId er for å vise hvor i lageret Item.cs objekter ligger.</param>
        /// <param name="dateTime">dateTime er for registrering og historikk for Item.cs objekter.</param>
        /// <param name="warehouseId">warehouseId er for å indentifisere hvilket varehus som skal brukes.</param>
        /// <param name="quantity">quantity er for å legge til x antall av gitt Item.cs objekt.</param>
        /// <exception cref="ServiceException">En exception som oppstår når det ikke finnes noen Items-objekter med det gitte internalId.</exception>
        public void AddItem(int internalId, int zoneId, DateTime dateTime, int warehouseId, int quantity = 1)
        {
            try
            {
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                }

                var item = warehouse.itemList.FirstOrDefault(i => i.internalId == internalId);
                if (item == null)
                {
                    throw new ServiceException($"Item with internal ID {internalId} not found. Item must be created before adding to zone.");
                }

                var availableZone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (availableZone == null || !warehouseService.IsStorageTypeCompatible(availableZone, item))
                {
                    throw new ServiceException($"Unable to find an available zone {zoneId} in warehouse {warehouseId}, or the item's storage type is not compatible.");
                }

                bool capacityFound = false;
                Shelf selectedShelf = null;
                foreach (var shelf in availableZone.shelves)
                {
                    if (warehouseService.HasAvailableCapacity(shelf))
                    {
                        capacityFound = true;
                        selectedShelf = shelf;
                        break;
                    }
                }

                if (!capacityFound)
                {
                    throw new ServiceException($"No available shelf capacity in zone {zoneId} for item {internalId}.");
                }

                var zoneItem = availableZone.itemsInZoneList.FirstOrDefault(zi => zi.internalId == internalId);
                if (zoneItem != null)
                {
                    zoneItem.quantity += quantity;
                    zoneItem.dateTime = dateTime;
                }
                else
                {
                    item.zoneId = zoneId;
                    availableZone.itemsInZoneList.Add(new Item
                    {
                        internalId = item.internalId,
                        externalId = item.externalId,
                        name = item.name,
                        storageType = item.storageType,
                        zoneId = zoneId,
                        quantity = quantity,
                        dateTime = dateTime
                    });
                }

                // Plasser item på valgt hylle
                warehouseService.PlaceItemOnShelf(internalId, selectedShelf.shelfId, zoneId, warehouseId);

                Console.WriteLine($"Item with internal ID {internalId} added to zone {zoneId} with quantity {quantity}.");
                Console.WriteLine($"Item with internal ID {internalId} placed on shelf {selectedShelf.shelfId} in zone {zoneId}.");
                OnItemAdded(internalId, zoneId, dateTime, warehouseId, quantity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding item: {ex.Message}");
            }
        }


        /// <summary>
        /// Funksjon for å fjerne en Item-gjenstand ut fra lageret.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>

        /// <exception cref="ServiceException"></exception>
        public void RemoveItem(int warehouseId, int internalId, int quantity)
        {
            try
            {
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                }

                // Anta at vi nå har en struktur for å finne varen i riktig sone
                Models.Item itemToRemove = null;
                Zone zoneOfItem = null;
                foreach (var zone in warehouse.zoneList)
                {
                    itemToRemove = zone.itemsInZoneList.FirstOrDefault(item => item.internalId == internalId);
                    if (itemToRemove != null)
                    {
                        zoneOfItem = zone;
                        break;
                    }
                }

                if (itemToRemove == null)
                {
                    throw new ServiceException($"Item {internalId} not found.");
                }

                // Reduserer antall eller fjerner varen helt
                if (itemToRemove.quantity > quantity)
                {
                    itemToRemove.quantity -= quantity;
                }
                else
                {
                    zoneOfItem.itemsInZoneList.Remove(itemToRemove);

                    OnItemRemoved(warehouseId, internalId);
                    //Console.WriteLine($"Item with internal ID {internalId} is now out of stock and has been removed from zone {zoneOfItem.zoneId}.");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Funksjon for å kunne flytte en Item-gjenstand til en ny lokasjon i lageret.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="newLocation">newLocation er for å sette en ny lokasjon på en Item gjenstand.</param>

        public void MoveItemToLocation(int warehouseId, int internalId, int newZone)
        {
            try
            {
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found. Cannot move item.");
                }

                Models.Item item = null;
                Zone oldZoneObj = null;
                foreach (var zone in warehouse.zoneList)
                {
                    item = zone.itemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                    if (item != null)
                    {
                        oldZoneObj = zone;
                        break;
                    }
                }

                if (item == null)
                {
                    throw new ServiceException($"Item with internal ID {internalId} not found in any zone. No action taken.");
                }

                var newZoneObj = warehouse.zoneList.FirstOrDefault(z => z.zoneId == newZone);
                if (newZoneObj == null)
                {
                    throw new ServiceException($"New zone with ID {newZone} not found in warehouse {warehouseId}. Cannot move item.");
                }

                // Check item and zone compatibility before moving the item
                if (!CheckItemAndZoneCompatibility(item, newZoneObj))
                {
                    throw new ServiceException($"Item {internalId} cannot be moved to zone {newZone} due to incompatible storage types.");
                }

                TimeSpan totalTime = oldZoneObj.itemRetrievalTime + newZoneObj.itemPlacementTime;
                DateTime newDateTime = item.dateTime.Add(totalTime);

                oldZoneObj.itemsInZoneList.Remove(item);
                item.zoneId = newZone;  // Update zoneId before adding it to new zone
                item.dateTime = newDateTime;
                newZoneObj.itemsInZoneList.Add(item);

                LogItemMovement(new ItemHistory(internalId, oldZoneObj.zoneId, newZone, item.dateTime));
                OnItemMoved(warehouseId, internalId, newZone);
                Console.WriteLine($"Item {internalId} has been moved from zone {oldZoneObj.zoneId} to zone {newZone} at {item.dateTime}. Total movement time: {totalTime}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while moving item: {ex.Message}");
            }
        }

        public bool CheckItemAndZoneCompatibility(Item item, Zone availableZone)
        {
            try
            {
                var compatibility = availableZone.zonePacketList.Contains(item.storageType);
                if (!compatibility)
                {
                    Console.WriteLine($"Adding item {item.internalId} with storage type {item.storageType} cannot be placed in" +
                                      $" the zone {availableZone.zoneId} since this zone has a storage type of {String.Join(", ", availableZone.zonePacketList)}.");
                    return false;
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return false;  // Return false to indicate incompatibility if an exception occurs
            }

            return true;
        }



        public void GetItemAllInfo(int warehouseId, int internalId)
        {
            try
            {
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                    return;
                }

                var item = warehouse.itemList.FirstOrDefault(i => i.internalId == internalId);
                if (item == null)
                {
                    Console.WriteLine($"Item with internal ID {internalId} not found.");
                    return;
                }

                // Skriv ut all tilgjengelig informasjon om item
                Console.WriteLine($"Item Information:");
                Console.WriteLine($"Internal ID: {item.internalId}");
                Console.WriteLine($"External ID: {(item.externalId.HasValue ? item.externalId.ToString() : "Not Available")}");
                Console.WriteLine($"Name: {item.name}");
                Console.WriteLine($"Description: {item.description ?? "No Description"}");
                Console.WriteLine($"Type: {item.type}");
                Console.WriteLine($"Storage Type: {item.storageType}");
                Console.WriteLine($"Zone ID: {item.zoneId ?? 0}");
                Console.WriteLine($"Quantity: {item.quantity}");
                Console.WriteLine($"Date Time: {item.dateTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving item information: {ex.Message}");
            }
        }






        /// <summary>
        /// En hjelpefunksjon for å logge bevegelsen til en fil.
        /// </summary>
        /// <param name="itemHistory">En klasse for å registrere historikk for Item-gjenstander.</param>
        private void LogItemMovement(ItemHistory itemHistory)
        {
            // Konverterer oldZone til en streng, bruker tom streng hvis null
            var oldZoneString = itemHistory.oldZone.HasValue ? itemHistory.oldZone.Value.ToString() : "NULL";

            // Format for logginnlegget
            var logEntry = $"{itemHistory.internalId},{oldZoneString},{itemHistory.newZone},{itemHistory.dateTime}\n";

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
        public int FindHowManyItemsInItemList(int warehouseId, int zoneId)
        {
            try
            {
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                    // Ingen varer å telle hvis lageret ikke finnes.
                }

                var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (zone == null)
                {
                    throw new ServiceException($"Zone with ID {zoneId} in warehouse {warehouseId} does not exist.");
                    // Ingen varer å telle hvis sonen ikke finnes.
                }
                else
                {
                    int countedItems = zone.itemsInZoneList.Count;
                    return countedItems;
                }
                
                
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

        }


        /// <summary>
        /// Funksjon for å telle kvantiteten for et gitt Item-gjenstand.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <returns>Returnerer en Int med totalt antall Kvantitet.</returns>
        public int FindHowManyItemQuantityByInternalId(int warehouseId, int internalId)
        {
            var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
            }

            int totalQuantity = 0;
            foreach (var zone in warehouse.zoneList)
            {
                totalQuantity += zone.itemsInZoneList.Where(item => item.internalId == internalId)
                                                      .Sum(item => item.quantity);
            }

            return totalQuantity;
        }


        /// <summary>
        /// Inne i ItemService-klassen
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        public void FindItemByInternalIdInWarehouse(int internalId)
        {

            var item = zone.itemsInZoneList.FirstOrDefault(item => item.internalId == internalId);

            if (item == null)
            {
                throw new ServiceException($"An Item with id{item} could not be found.");
            }

            else if (zone.itemsInZoneList == null)
            {
                throw new ServiceException($"Error 404");
            }

            Console.WriteLine($"item Id: {item.internalId}\n" +
                              $"item Name: {item.name}\n" +
                              $"item : {item.storageType}\n");

        }


        public void ClearWarehouseData()
        {
            // Anta at WarehouseService inneholder en metode for å hente alle lagre.
            var allWarehouses = warehouseService.GetAllWarehouses();

            foreach (var warehouse in allWarehouses)
            {
                // Tømmer varelisten for hvert lager
                warehouse.itemList.Clear();

                // Går gjennom hver sone i lageret og tømmer varelisten
                foreach (var zone in warehouse.zoneList)
                {
                    zone.itemsInZoneList.Clear();
                }
            }

            Console.WriteLine("All warehouse and zone item data cleared.");
        }


        public int? GetLocationByInternalId(int warehouseId, int internalId)
        {
            try
            {
            // Finn det spesifikke Warehouse-objektet ved hjelp av warehouseId.
            var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
            }

            // Gå gjennom alle soner i lageret for å finne varen.
            foreach (var zone in warehouse.zoneList)
            {
                var item = zone.itemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                if (item != null)
                {
                    // Returnerer zoneId som int? når varen er funnet.
                    return zone.zoneId;
                }
            }

            // Returnerer null hvis varen ikke finnes i noen sone.
            return null;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }




        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}
        public bool ItemExists(int warehouseId, int internalId)
        {
            try
            {
                // Først, finn det aktuelle Warehouse-objektet ved hjelp av warehouseId
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found. Cannot check if item exists.");
                }

                // Sjekk deretter om et Item med gitt internalId eksisterer i dette Warehouse-objektets ItemList
                return warehouse.itemList.Any(i => i.internalId == internalId);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
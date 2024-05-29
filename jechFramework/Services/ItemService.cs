using jechFramework.Models;
using jechFramework.Services;
using System;
using System.IO;
using System.Xml.Linq;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner I ItemService.cs
    /// </summary>
    public class ItemService
    {
        private WarehouseService warehouseService = new();

        private Warehouse warehouse; // Referanse til Warehouse objekt for å aksessere ItemList
        private Zone zone;

        public delegate void ItemCreatedEventHandler(
            int warehouseId,
            int internalId, 
            int? externalId, 
            string? name, 
            Enum storagetype);

        public delegate void ItemAddedEventHandler(
            int warehouseId, 
            int zoneId, 
            int internalId, 
            DateTime dateTime,  
            int quantity = 1);

        public delegate void ItemRemovedEventHandler(int warehouseId, int internalId, ushort quantity);
        public delegate void ItemMovedEventHandler(int warehouseId, int internalId, int newZone);

        public event EventHandler<ItemEventArgs> ItemCreated;
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved;
        public event EventHandler<ItemEventArgs> ItemMoved;

        public void OnItemCreated(int warehouseId, int internalId, int? externalId, string? name, StorageType storageType)
        {
            ItemCreated?.Invoke(this, new ItemEventArgs(warehouseId, internalId, externalId, name, storageType));
        }

        public void OnItemAdded(int warehouseId, int zoneId, int internalId, DateTime dateTime, int quantity = 1)
        {
            ItemAdded?.Invoke(this, new ItemEventArgs(warehouseId, zoneId, internalId, dateTime, quantity));
        }

        public void OnItemRemoved(int warehouseId, int internalId, ushort quantity)
        {
            ItemRemoved?.Invoke(this, new ItemEventArgs(warehouseId, internalId, quantity));
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
        /// Funksjon for å opprette og legge til nye varer i varehuset.
        /// </summary>
        /// <param name="warehouseId">warehouseId for å vise id'en på produktet internt for varehuset.</param>
        /// <param name="internalId">internalId for å bruke id'en på produktet internt for varehuset.</param>
        /// <param name="externalId">externalId for tilfellene man skulle trenge leverandør sin produkt id.</param>
        /// <param name="name">name er for å kunne gi navn til en gitt vare.</param>
        /// <param name="storageType">storageType for å kunne sette hvilket type produkt form/størrelse.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet eller allerede finnes.</exception>
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

                //Console.WriteLine($"New item created: {name} with ID: {internalId} and Storagetype: {storageType} in warehouse ID: {warehouseId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating item: {ex.Message}");
            }
        }

        /// <summary>
        /// Funksjon for å legge en Item-gjenstand inn i lageret(wareHouseItemList), altså som legges inn i lageret sin liste.
        /// </summary>
        /// <param name="warehouseId">warehouseId for å indentifisere hvilket varehus som skal brukes.</param>
        /// <param name="zoneId">zoneId for å vise hvor i lageret Item.cs objekter ligger.</param>
        /// <param name="internalId">internalId for å bruke id'en på produktet internt for varehuset.</param>
        /// <param name="dateTime">dateTime for registrering og historikk for Item.cs objekter.</param>
        /// <param name="quantity">quantity for å legge til x antall av gitt Item.cs objekt.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet eller kapasitet er null.</exception>
        public void AddItem(int warehouseId, int zoneId, int internalId, DateTime dateTime, ushort quantity = 1)
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

                var zoneItem = availableZone.itemsInZoneList.FirstOrDefault(zi => zi.internalId == internalId);
                
                if (zoneItem != null)
                {
                    zoneItem.quantity += quantity;  // Øk eksisterende kvantitet, ikke overskriv
                    zoneItem.dateTime = dateTime;
                    Console.WriteLine($"{quantity} of item: {internalId} has been successfully added to zone: {zoneId} in Warehouse: {warehouseId}. Total quantity now: {zoneItem.quantity}.");
                }
                
                else
                {
                    // Opprett ny vareoppføring hvis den ikke finnes fra før
                    availableZone.itemsInZoneList.Add(new Item
                    {
                        internalId = internalId,
                        externalId = item.externalId,
                        name = item.name,
                        storageType = item.storageType,
                        zoneId = zoneId,
                        quantity = quantity,
                        dateTime = dateTime
                    });

                    Console.WriteLine($"New item with internal ID {internalId} added to zone {zoneId} with quantity {quantity}.");
                }

                LogAddition(warehouseId, zoneId, internalId, quantity); // Logg operasjonen
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding item: {ex.Message}.");
            }
        }

        /// <summary>
        /// funksjon for å logge historikk over til en loggfil.
        /// </summary>
        /// <param name="warehouseId">warehouseId for å vise id'en på produktet internt for varehuset.</param>
        /// <param name="zoneId">zoneId for å vise hvor i lageret Item.cs objekter ligger.</param>
        /// <param name="internalId">internalId for å vise id'en til item-gjenstanden.</param>
        /// <param name="quantity">quantity er for hvor mange av den gitte varen det gjelder.</param>
        private void LogAddition(int warehouseId, int zoneId, int internalId, ushort quantity)
        {
            var logEntry = $"{DateTime.Now}: Added item with internal ID {internalId} to zone {zoneId} in warehouse {warehouseId} with quantity {quantity}.\n";
            var logFilePath = "ItemAdditions.log"; // Du kan velge å bruke samme loggfil som bevegelser eller en egen fil for tilføyelser

            File.AppendAllText(logFilePath, logEntry);
        }

        /// <summary>
        /// Funksjon for å fjerne en Item-gjenstand ut fra lageret.
        /// </summary>
        /// <param name="warehouseId">warehouseId for å vise id'en på produktet internt for varehuset.</param>
        /// <param name="internalId">internalId for å vise id'en til item-gjenstanden.</param>
        /// <param name="quantity">quantity er for hvor mange av den gitte varen det gjelder.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet.</exception>
        public void RemoveItem(int warehouseId, int internalId, ushort quantity)
        {
            try
            {
                var warehouse = warehouseService.FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                }

                var itemToRemove = warehouse.zoneList.SelectMany(z => z.itemsInZoneList)
                                                     .FirstOrDefault(item => item.internalId == internalId);

                if (itemToRemove == null)
                {
                    throw new ServiceException($"Item {internalId} not found.");
                }

                if (itemToRemove.quantity > quantity)
                {
                    itemToRemove.quantity -= quantity;
                }

                else
                {
                    warehouse.zoneList.ForEach(z => z.itemsInZoneList.Remove(itemToRemove)); // Remove from all zones
                    OnItemRemoved(warehouseId, internalId, quantity); // Trigger event if item is completely removed
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
        /// <param name="warehouseId">Parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="newZone">newZone er for å sette en ny sone på en Item-gjenstand.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet.</exception>
        public void MoveItemToLocation(int warehouseId, int internalId, int newZone)
        {
            try
            {
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse {warehouseId} not found.");
                }

                var item = warehouse.zoneList.SelectMany(z => z.itemsInZoneList).FirstOrDefault(i => i.internalId == internalId);
                
                if (item == null)
                {
                    throw new ServiceException($"Item {internalId} not found.");
                }

                var oldZoneObj = warehouse.zoneList.FirstOrDefault(z => z.itemsInZoneList.Contains(item));
                var newZoneObj = warehouse.zoneList.FirstOrDefault(z => z.zoneId == newZone);
                
                if (newZoneObj == null)
                {
                    throw new ServiceException($"New zone {newZone} not found.");
                }

                if (!CheckItemAndZoneCompatibility(item, newZoneObj))
                {
                    Console.WriteLine($"Incompatible item {item.name} (ID: {internalId}) for new zone {newZoneObj.zoneName}.");
                    return;
                }

                TimeSpan totalTime = oldZoneObj.itemRetrievalTime + newZoneObj.itemPlacementTime;
                DateTime newDateTime = item.dateTime.Add(totalTime);

                oldZoneObj.itemsInZoneList.Remove(item);
                item.dateTime = newDateTime;
                newZoneObj.itemsInZoneList.Add(item);

                LogItemMovement(new ItemHistory(internalId, oldZoneObj.zoneId, newZone, item.dateTime, totalTime));
                Console.WriteLine($"Moved '{item.name}' (ID: {internalId}) from '{oldZoneObj.zoneName}' to '{newZoneObj.zoneName}'. Time: {totalTime}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}.");
            }
        }

        /// <summary>
        /// Funksjon for å sjekke om sonene er kompatible med hverandre, altså storageType passer.
        /// </summary>
        /// <param name="item">Gjeldene Item-objekt.</param>
        /// <param name="availableZone">Gjeldene sone.</param>
        /// <returns>Returnerer true hvis kompatibel, ellers false.</returns>
        public bool CheckItemAndZoneCompatibility(Item item, Zone availableZone)
        {
            if (availableZone.zonePacketList == null || availableZone.zonePacketList.Count == 0)
            {
                Console.WriteLine($"Zone {availableZone.zoneId} has no defined storage types. Defined types are required for compatibility check.");
                return false;
            }

            Console.WriteLine($"Checking compatibility for item {item.internalId} with type {item.storageType} against zone {availableZone.zoneId} with types {String.Join(", ", availableZone.zonePacketList)}.");
            var compatibility = availableZone.zonePacketList.Contains(item.storageType);
            
            if (!compatibility)
            {
                Console.WriteLine($"Incompatible storage type. Item {item.internalId} with type {item.storageType} cannot be placed in zone {availableZone.zoneId} with types {String.Join(", ", availableZone.zonePacketList)}.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Funksjon for å få tak i all Item-objekt informasjon for en gitt Item.
        /// </summary>
        /// <param name="warehouseId">Parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <returns>Returnerer Item objektet hvis funnet, ellers null.</returns>
        public Item GetItemAllInfo(int warehouseId, int internalId)
        {
            try
            {
                var warehouse = warehouseService.warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                
                if (warehouse == null)
                {
                    Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                    return null;
                }

                Item item = null;
                bool foundInZone = false;

                // Søk først i alle zoner etter varen
                foreach (var zone in warehouse.zoneList)
                {
                    item = zone.itemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                    
                    if (item != null)
                    {
                        foundInZone = true;
                        break;  // Finn varen og avbryt løkken
                    }
                   
                }

                // Hvis ikke funnet i soner, sjekk hovedlagerlisten
                if (item == null)
                {
                    item = warehouse.itemList.FirstOrDefault(i => i.internalId == internalId);
                    
                    if (item != null)
                    {
                        Console.WriteLine($"Item with internal ID {internalId} not found in the specified warehouse {warehouseId}.");
                        return null; // Returner etter å ha informert at varen ikke finnes i lageret
                    }
                    
                }

                // Skriv ut all tilgjengelig informasjon om item siden den ble funnet i en sone
                Console.WriteLine($"----- Item Information: -----");
                Console.WriteLine($"Internal ID: {item.internalId}");
                Console.WriteLine($"External ID: {(item.externalId.HasValue ? item.externalId.ToString() : "Not Available")}");
                Console.WriteLine($"Name: {item.name}");
                Console.WriteLine($"Description: {item.description ?? "No Description"}");
                Console.WriteLine($"Type: {item.type}");
                Console.WriteLine($"Storage Type: {item.storageType}");
                Console.WriteLine($"Zone ID: {item.zoneId ?? 0}");
                Console.WriteLine($"Quantity: {item.quantity}");
                Console.WriteLine($"Last Moved Time: {item.dateTime}");

                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving item information: {ex.Message}.");
                return null;
            }
        }

        /// <summary>
        /// En hjelpefunksjon for å logge bevegelsen til en fil.
        /// </summary>
        /// <param name="itemHistory">En klasse for å registrere historikk for Item-gjenstander.</param>
        private void LogItemMovement(ItemHistory itemHistory)
        {
            var oldZoneString = itemHistory.oldZone.HasValue ? itemHistory.oldZone.Value.ToString() : "NULL";
            var logEntry = $"{itemHistory.internalId},{oldZoneString},{itemHistory.newZone},{itemHistory.dateTime:yyyy-MM-dd HH:mm:ss}\n";
            File.AppendAllText(ItemHistoryService.logFilePath, logEntry);
            Console.WriteLine($"Logged: {itemHistory.internalId}, from Zone {oldZoneString} to {itemHistory.newZone}, on {itemHistory.dateTime:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// Funksjon for å telle antall Item-gjenstander i en gitt sone.
        /// </summary>
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="zoneId">parameter for valgt sone.</param>
        /// <returns>Returnerer en Int for alle registrerte Item-gjenstander med gitt internalId.</returns>
        public int FindHowManyItemsInZone(int warehouseId, int zoneId)
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
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise id'en på produktet internt for varehuset.</param>
        /// <returns>Returnerer en Int med totalt antall Kvantitet.</returns>
        /// <exception cref="ServiceException">Varehuset finnes ikke.</exception>
        public int FindItemQuantityInWarehouse(int warehouseId, int internalId)
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
        /// funksjon for å finne en item-gjenstand ved bruk av internalId.
        /// </summary>
        /// <param name="internalId">internalId for å vise id'en på produktet internt for varehuset.</param>
        /// <exception cref="ServiceException">item-gjenstanden finnes ikke.</exception>
        public void FindItemByInternalIdInWarehouse(int internalId)
        {

            var item = zone.itemsInZoneList.FirstOrDefault(item => item.internalId == internalId);

            if (item == null)
            {
                throw new ServiceException($"An Item with id{item} could not be found.");
            }

            else if (zone.itemsInZoneList == null)
            {
                throw new ServiceException($"Zone {zone} does not exist.");
            }

            Console.WriteLine($"item Id: {item.internalId}\n" +
                              $"item Name: {item.name}\n" +
                              $"item : {item.storageType}\n");

        }

        /// <summary>
        /// Funksjon for å tømme varehus-data.
        /// </summary>
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

        /// <summary>
        /// Funksjon for å finne lokasjonen for en Item-gjenstand ved bruk av dens Id.
        /// </summary>
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise id'en på produktet internt for varehuset.</param>
        /// <returns>returnerer en int for lokasjonen.</returns>
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

        /// <summary>
        /// Funksjon for å sjekke om en Item-gjenstand eksisterer.
        /// </summary>
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise id'en på produktet internt for varehuset.</param>
        /// <returns>returnerer true eller false om Item-gjenstanden finnes eller ikke.</returns>
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
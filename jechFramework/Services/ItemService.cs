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
        /// <param name="internalId">internalId for å bruke id'en på produktet internt for varehuset.</param>
        /// <param name="zoneId">zoneId for å vise hvor i lageret Item.cs objekter ligger.</param>
        /// <param name="dateTime">dateTime for registrering og historikk for Item.cs objekter.</param>
        /// <param name="warehouseId">warehouseId for å indentifisere hvilket varehus som skal brukes.</param>
        /// <param name="quantity">quantity for å legge til x antall av gitt Item.cs objekt.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet eller kapasitet er null.</exception>
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

                var zoneItem = availableZone.itemsInZoneList.FirstOrDefault(zi => zi.internalId == internalId);
                if (zoneItem != null)
                {
                    zoneItem.quantity += quantity;  // Øk eksisterende kvantitet, ikke overskriv
                    zoneItem.dateTime = dateTime;
                    Console.WriteLine($"{quantity} of item: {internalId} has been successfully added to zone: {zoneId} in Warehouse: {warehouseId}. Total quantity now: {zoneItem.quantity}");
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

                LogAddition(internalId, zoneId, dateTime, warehouseId, quantity); // Logg operasjonen
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding item: {ex.Message}");
            }
        }




        private void LogAddition(int internalId, int zoneId, DateTime dateTime, int warehouseId, int quantity)
        {
            var logEntry = $"{DateTime.Now}: Added item with internal ID {internalId} to zone {zoneId} in warehouse {warehouseId} with quantity {quantity}.\n";
            var logFilePath = "ItemAdditions.log"; // Du kan velge å bruke samme loggfil som bevegelser eller en egen fil for tilføyelser

            File.AppendAllText(logFilePath, logEntry);
        }


        /// <summary>
        /// Funksjon for å fjerne en Item-gjenstand ut fra lageret.
        /// </summary>
        /// <param name="warehouseId">warehouseId for å vise id'en på produktet internt for varehuset.</param>
        /// <param name="internalId">internalId for å vise id'en til varehuset.</param>
        /// <param name="quantity">Quantity er for hvor mange av den gitte varen det gjelder.</param>
        /// <exception cref="ServiceException">Objekter ikke funnet.</exception>
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
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found. Cannot move item.");
                }

                var item = warehouse.zoneList.SelectMany(z => z.itemsInZoneList).FirstOrDefault(i => i.internalId == internalId);
                if (item == null)
                {
                    throw new ServiceException($"Item with internal ID {internalId} not found in any zone. No action taken.");
                }

                var oldZoneObj = warehouse.zoneList.FirstOrDefault(z => z.itemsInZoneList.Contains(item));
                var newZoneObj = warehouse.zoneList.FirstOrDefault(z => z.zoneId == newZone);
                if (newZoneObj == null)
                {
                    throw new ServiceException($"New zone with ID {newZone} not found in warehouse {warehouseId}. Cannot move item.");
                }

                if (!CheckItemAndZoneCompatibility(item, newZoneObj))
                {
                    return;
                }

                TimeSpan totalTime = oldZoneObj.itemRetrievalTime + newZoneObj.itemPlacementTime;
                DateTime newDateTime = item.dateTime.Add(totalTime);

                oldZoneObj.itemsInZoneList.Remove(item);
                item.dateTime = newDateTime;  // Update dateTime to include the move duration
                newZoneObj.itemsInZoneList.Add(item);

                // Logg bevegelsen etter vellykket flytting
                LogItemMovement(new ItemHistory(internalId, oldZoneObj.zoneId, newZone, item.dateTime, totalTime));

                Console.WriteLine($"Item {internalId} has been successfully moved from zone {oldZoneObj.zoneId} to zone {newZone}. Total movement time: {totalTime}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while moving item: {ex.Message}");
            }
        }





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

                Item item = null;

                // Søk først i alle zoner etter varen
                foreach (var zone in warehouse.zoneList)
                {
                    item = zone.itemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                    if (item != null)
                    {
                        break;  // Finn varen og avbryt løkken
                    }
                }

                // Hvis ikke funnet i soner, sjekk hovedlagerlisten
                if (item == null)
                {
                    item = warehouse.itemList.FirstOrDefault(i => i.internalId == internalId);
                    if (item == null)
                    {
                        Console.WriteLine($"Item with internal ID {internalId} not found in any zones or the main list.");
                        return;
                    }
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
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="zoneId">parameter for valgt sone.</param>
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
        /// <param name="warehouseId">parameter for valgt varehus.</param>
        /// <param name="internalId">internalId for å vise id'en på produktet internt for varehuset.</param>
        /// <returns>Returnerer en Int med totalt antall Kvantitet.</returns>
        /// <exception cref="ServiceException">Varehuset finnes ikke.</exception>
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
                throw new ServiceException($"Error 404");
            }

            Console.WriteLine($"item Id: {item.internalId}\n" +
                              $"item Name: {item.name}\n" +
                              $"item : {item.storageType}\n");

        }


        /// <summary>
        /// Funksjon for å tømme varehus-data
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
        /// <returns>returnerer True eller False om Item-gjenstanden finnes eller ikke.</returns>
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



        /// <summary>
        /// Funksjon for å sjekke om Item-gjenstandens storageType er innenfor sonen sin storageType.
        /// </summary>
        /// <param name="item">Parameter for en Item-gjenstand.</param>
        /// <param name="availableZone">Parameter for den sonen man vil sjekke kompatibilitet mot.</param>
        /// <returns>Returnerer True eller False om Item-gjenstanden er kompatibel med sonen eller ikke.</returns>
       
    }
}
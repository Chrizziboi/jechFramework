using jechFramework.Interfaces;
using jechFramework.Models;
using System.IO;
using System.Xml.Linq;

namespace jechFramework.Services
{
    /// <summary>
    /// Funksjoner for Item.cs
    /// </summary>
    public class ItemService
    {
        private WarehouseService warehouseService;
        private Warehouse warehouse; // Referanse til Warehouse objekt for å aksessere ItemList
        private Zone zone;
        // private static List<Item> ItemsInZoneList = new List<Item>();
        // private static List<Item> ItemList = new List<Item>();
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
        public void CreateItem(int warehouseId, int internalId, int? externalId, string name, string type)
        {
            Warehouse warehouse = null;

            try
            {
                warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false); // Ingen utskrift av detaljer
                if (warehouse == null)
                {
                    Console.WriteLine($"Warehouse with ID {warehouseId} not found. Skipping item creation.");
                    return;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"An error occurred while trying to find warehouse {warehouseId}: {ex.Message}. Skipping item creation.");
                return;
            }

            // Sjekk om et Item med samme internalId allerede finnes i ItemList for det Warehouse-objektet
            if (warehouse.ItemList.Any(i => i.internalId == internalId))
            {
                Console.WriteLine($"Item with internal ID {internalId} already exists in the warehouse item list. Skipping item creation.");
                return; // Returnerer fra metoden for å unngå å legge til duplikate items.
            }

            // Opprett et nytt Item-objekt og legg det til i ItemList for det funnet Warehouse-objektet
            var newItem = new Item
            {
                internalId = internalId,
                externalId = externalId,
                name = name,
                type = type
            };

            warehouse.ItemList.Add(newItem);
            Console.WriteLine($"New item created: {name} with ID: {internalId} in warehouse ID: {warehouseId}.");
        }





        /// <summary>
        /// Funksjon for å legge en Item-gjenstand inn i lageret(wareHouseItemList), altså som legges inn i lageret sin liste.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="location">Lcation er for å vise hvor i lageret det ligger.</param>
        /// <param name="dateTime">dateTime er for registrering og historikk for Item.cs objekter.</param>
        /// <exception cref="InvalidOperationException">En exception som oppstår når det ikke finnes noen Items-objekter med det gitte internalId.</exception>
        public void AddItem(int internalId, int initialZoneId, DateTime dateTime, int warehouseId, int quantity = 1)
        {
            try
            {
                // Forsøker å finne en tilgjengelig sone som kan akseptere varen, starter med den foretrukne sonen.
                var availableZone = warehouseService.FindAvailableZoneForItem(warehouseId, initialZoneId, quantity);
                if (availableZone == null)
                {
                    Console.WriteLine($"Unable to find an available zone for item {internalId} in warehouse {warehouseId}.");
                    return;
                }

                // Sjekker om item allerede eksisterer i den tilgjengelige sonen
                var itemToAdd = availableZone.ItemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                if (itemToAdd != null)
                {
                    // Øker kvantiteten for eksisterende item
                    itemToAdd.quantity += quantity;
                    Console.WriteLine($"Quantity of item with internal ID {internalId} in zone {availableZone.zoneId} increased by {quantity}.");
                }
                else
                {
                    // Oppretter og legger til en ny item hvis den ikke eksisterer
                    itemToAdd = new Item { internalId = internalId, zoneId = availableZone.zoneId, dateTime = dateTime, quantity = quantity };
                    availableZone.ItemsInZoneList.Add(itemToAdd);
                    Console.WriteLine($"Item with internal ID {internalId} added to zone {availableZone.zoneId} with quantity {quantity}.");
                }

                // Logger bevegelsen for den nye eller oppdaterte item
                LogItemMovement(new ItemHistory(internalId, null, availableZone.zoneId, dateTime));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to add item {internalId} to warehouse {warehouseId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Funksjon for å fjerne en Item-gjenstand ut fra lageret.
        /// </summary>
        /// <param name="internalId">har brukt internalId for å vise iden på produktet internt for varehuset.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveItem(int warehouseId, int internalId)
        {
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                throw new InvalidOperationException($"Warehouse with ID {warehouseId} not found.");
            }

            // Anta at vi nå har en struktur for å finne varen i riktig sone
            Item itemToRemove = null;
            Zone zoneOfItem = null;
            foreach (var zone in warehouse.zoneList)
            {
                itemToRemove = zone.ItemsInZoneList.FirstOrDefault(item => item.internalId == internalId);
                if (itemToRemove != null)
                {
                    zoneOfItem = zone;
                    break;
                }
            }

            if (itemToRemove == null)
            {
                throw new InvalidOperationException("Item not found.");
            }

            // Reduserer antall eller fjerner varen helt
            if (itemToRemove.quantity > 1)
            {
                itemToRemove.quantity -= 1;
            }
            else
            {
                zoneOfItem.ItemsInZoneList.Remove(itemToRemove);
                Console.WriteLine($"Item with internal ID {internalId} is now out of stock and has been removed from zone {zoneOfItem.zoneId}.");
            }
        }



        /// <summary>
        /// Funksjon for å kunne flytte en Item-gjenstand til en ny lokasjon i lageret.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <param name="newLocation">newLocation er for å sette en ny lokasjon på en Item gjenstand.</param>
        public void MoveItemToLocation(int warehouseId, int internalId, int newZone)
        {
            // Først finn det spesifikke Warehouse-objektet
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found. Cannot move item.");
                return;
            }

            // Finn item i alle soner i det valgte lageret
            Item item = null;
            foreach (var zone in warehouse.zoneList)
            {
                item = zone.ItemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                if (item != null) break;
            }

            if (item == null)
            {
                Console.WriteLine($"Item with internal ID {internalId} not found in any zone. No action taken.");
                return;
            }

            var oldZone = item.zoneId;
            var newZoneObj = warehouse.zoneList.FirstOrDefault(z => z.zoneId == newZone);
            if (newZoneObj == null)
            {
                Console.WriteLine($"New zone with ID {newZone} not found in warehouse {warehouseId}. Cannot move item.");
                return;
            }

            // Oppdaterer lokasjonen og tidsstempel
            item.zoneId = newZone;
            item.dateTime = DateTime.Now;

            // Oppdaterer også sonens liste hvis nødvendig
            if (!newZoneObj.ItemsInZoneList.Contains(item))
            {
                newZoneObj.ItemsInZoneList.Add(item);
                // Anta at vi også må fjerne item fra den gamle sonen, her må du implementere logikk for det
            }

            LogItemMovement(new ItemHistory(internalId, oldZone, newZone, item.dateTime));
            Console.WriteLine($"Item {internalId} has been moved from zone {oldZone} to zone {newZone} at {item.dateTime}.");
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
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                return 0; // Ingen varer å telle hvis lageret ikke finnes.
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            if (zone == null)
            {
                Console.WriteLine($"Zone with ID {zoneId} in warehouse {warehouseId} does not exist.");
                return 0; // Ingen varer å telle hvis sonen ikke finnes.
            }

            int countedItems = zone.ItemsInZoneList.Count;
            return countedItems;
        }



        /// <summary>
        /// Funksjon for å telle kvantiteten for et gitt Item-gjenstand.
        /// </summary>
        /// <param name="internalId">internalId for å vise iden på produktet internt for varehuset.</param>
        /// <returns>Returnerer en Int med totalt antall Kvantitet.</returns>
        public int FindHowManyItemQuantityByInternalId(int warehouseId, int internalId)
        {
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                return 0;
            }

            int totalQuantity = 0;
            foreach (var zone in warehouse.zoneList)
            {
                totalQuantity += zone.ItemsInZoneList.Where(item => item.internalId == internalId)
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

            var item = zone.ItemsInZoneList.FirstOrDefault(item => item.internalId == internalId);

            if (item == null)
            {
                throw new InvalidOperationException($"An Item with id{item} could not be found.");
            }

            else if (zone.ItemsInZoneList == null)
            {
                throw new InvalidOperationException($"");
            }

            Console.WriteLine($"item Id: {item.internalId}\n" +
                              $"item Name: {item.name}\n" +
                              $"item : {item.type}\n");

        }


        public void ClearWarehouseData()
        {
            // Anta at WarehouseService inneholder en metode for å hente alle lagre.
            var allWarehouses = warehouseService.GetAllWarehouses();

            foreach (var warehouse in allWarehouses)
            {
                // Tømmer varelisten for hvert lager
                warehouse.ItemList.Clear();

                // Går gjennom hver sone i lageret og tømmer varelisten
                foreach (var zone in warehouse.zoneList)
                {
                    zone.ItemsInZoneList.Clear();
                }
            }

            Console.WriteLine("All warehouse and zone item data cleared.");
        }


        public int? GetLocationByInternalId(int warehouseId, int internalId)
        {
            // Finn det spesifikke Warehouse-objektet ved hjelp av warehouseId.
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                return null;
            }

            // Gå gjennom alle soner i lageret for å finne varen.
            foreach (var zone in warehouse.zoneList)
            {
                var item = zone.ItemsInZoneList.FirstOrDefault(i => i.internalId == internalId);
                if (item != null)
                {
                    // Returnerer zoneId som int? når varen er funnet.
                    return zone.zoneId;
                }
            }

            // Returnerer null hvis varen ikke finnes i noen sone.
            return null;
        }




        //public UpdateItemMovement(int internalId, string n)
        //{
        //    
        //}
        public bool ItemExists(int warehouseId, int internalId)
        {
            // Først, finn det aktuelle Warehouse-objektet ved hjelp av warehouseId
            var warehouse = warehouseService.FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found. Cannot check if item exists.");
                return false;
            }

            // Sjekk deretter om et Item med gitt internalId eksisterer i dette Warehouse-objektets ItemList
            return warehouse.ItemList.Any(i => i.internalId == internalId);
        }
    }

}
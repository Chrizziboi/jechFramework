using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    public class WarehouseService //Service klasse for Warehouse.cs, Employee.cs og Zone.cs
    {

        private readonly Warehouse warehouseInstance = new();
        private readonly Shelf shelfInstance = new();

        private readonly WaresOutService waresOutService = new WaresOutService();

        public List<Warehouse> warehouseList = new List<Warehouse>();
        // Liste med varehus


        ///                                       ///
        /// Event handler for WarehouseService.cs ///
        ///                                       ///


        public delegate void WarehouseCreatedEventHandler(Warehouse warehouse);
        public delegate void WarehouseRemovedEventHandler(Warehouse warehouse);
        public delegate void ZoneCreatedEventHandler(Warehouse warehouse, Zone zone);
        public delegate void ZoneRemovedEventHandler(Warehouse warehouse, Zone zone);
        public delegate void EmployeeCreatedEventHandler(Warehouse warehouse, Employee employee);
        public delegate void EmployeeRemovedEventHandler(Warehouse warehouse, Employee employee);

        public event EventHandler<WarehouseEventArgs> WarehouseCreated;
        public event EventHandler<WarehouseEventArgs> WarehouseRemoved;
        public event EventHandler<ZoneEventArgs> ZoneCreated;
        public event EventHandler<ZoneEventArgs> ZoneRemoved;
        public event EventHandler<EmployeeEventArgs> EmployeeCreated;
        public event EventHandler<EmployeeEventArgs> EmployeeRemoved;


        public void OnWarehouseCreated(Warehouse warehouse)
        {
            //Console.WriteLine($"Warehouse created with Id: {warehouse}.");
            WarehouseCreated?.Invoke(this, new WarehouseEventArgs(warehouse));
        }

        public void OnWarehouseRemoved(Warehouse warehouse)
        {
            WarehouseRemoved?.Invoke(this, new WarehouseEventArgs(warehouse));
        }

        public void OnZoneCreated(Warehouse warehouse, Zone zone)
        {
            ZoneCreated?.Invoke(this, new ZoneEventArgs(warehouse, zone));
        }

        public void OnZoneRemoved(Warehouse warehouse, Zone zone)
        {
            ZoneRemoved?.Invoke(this, new ZoneEventArgs(warehouse, zone));
        }

        public void OnEmployeeCreated(Warehouse warehouse, Employee employee)
        {
            EmployeeCreated?.Invoke(this, new EmployeeEventArgs(warehouse, employee));
        }

        public void OnEmployeeRemoved(Warehouse warehouse, Employee employee)
        {
            EmployeeRemoved?.Invoke(this, new EmployeeEventArgs(warehouse, employee));
        }

        ///                                        ///
        ///   Service funksjoner for Warehouse.cs  ///
        ///                                        ///

        /// <summary>
        /// Funksjon for å opprette et varehus.
        /// </summary>
        public void CreateWarehouse(int warehouseId, string warehouseName, int warehouseCapacity)
        {
            try
            {
                // Sjekk om varehuset allerede eksisterer
                if (warehouseList.Any(w => w.warehouseId == warehouseId))
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} already exists.");
                }

                // Opprett et nytt varehus
                var newWarehouse = new Warehouse(warehouseId, warehouseName, warehouseCapacity);
                warehouseList.Add(newWarehouse);

                // Kall til ItemHistoryService for å sikre at loggfilen eksisterer
                ItemHistoryService.EnsureLogfileExists();

                // Kall til OnWarehouseCreated metode for å håndtere post-creation logikk
                OnWarehouseCreated(newWarehouse);

                Console.WriteLine($"Warehouse created with ID: {warehouseId}, Name: {warehouseName}, Capacity: {warehouseCapacity}");
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error creating warehouse: {ex.Message}");
            }
        }

        /// <summary>
        /// Funksjon for å finne et varehus i varehus-listen
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <exception cref="ServiceException"></exception>


        /// <exception cref="ServiceException"></exception>
        public Warehouse FindWarehouseInWarehouseListWithPrint(int warehouseId, bool printDetails = true)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(wh => wh.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"A warehouse with id {warehouseId} could not be found.");
                }

                if (printDetails)
                {
                    Console.WriteLine($"warehouse Id: {warehouse.warehouseId}\n" +
                                      $"warehouse Name: {warehouse.warehouseName}\n" +
                                      $"warehouse Capacity: {warehouse.warehouseCapacity}\n");
                }

                return warehouse; // Returnerer Warehouse-objektet hvis funnet
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return null; // Returnerer null hvis det oppstår en feil
            }
        }
        public List<Warehouse> GetAllWarehouses()
        {
            return warehouseList;
            

        }

        /// <summary>
        /// Funksjon for å ta vekk opprettede varehus fra varehuslisten.
        /// </summary>
        /// <param name="warehouseId">Int tall for å gi en identifikator for et gitt varehus.</param>
        public void RemoveWarehouse(int warehouseId)
        {
            var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

            if (warehouse == null)
            {
                throw new ServiceException($"A warehouse with id: {warehouseId} could not be found.");
            }

            warehouseList.Remove(warehouse);

            OnWarehouseRemoved(warehouse);

        }

        ///                                        ///
        ///   Service funksjoner for Employee.cs   ///
        ///                                        ///

        /// <summary>
        /// Her er det laget en funksjon for å kunne lage en sone hvor man kan lage navn og velge kapasiteten til en ny sone.
        /// </summary>
        public void CreateZone(int warehouseId, int zoneId, string zoneName, int zoneCapacity, TimeSpan itemPlacementTime, TimeSpan itemRetrievalTime, StorageType storageType)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} does not exist.");
                }
                var existingZone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (existingZone != null)
                {
                    throw new ServiceException($"Zone with id {zoneId} already exists in Warehouse with id {warehouseId}.");
                }

                // Check if adding this zone would exceed warehouse capacity
                if (warehouse.zoneList.Count + 1 > warehouse.warehouseCapacity)
                {
                    throw new ServiceException($"Adding zone with id {zoneId} would exceed warehouse capacity, " +
                                               $"therefore Zone not created.");
                }

                Zone newZone = new(zoneId, zoneName, zoneCapacity, itemPlacementTime, itemRetrievalTime, storageType);
                warehouse.zoneList.Add(newZone);

                OnZoneCreated(warehouse, newZone);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CreateZoneWithMultipleType(int warehouseId, int zoneId, string zoneName, int zoneCapacity, TimeSpan itemPlacementTime, TimeSpan itemRetrievalTime, List<StorageType> zonePacketList)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} does not exist.");
                }
                var existingZone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (existingZone != null)
                {
                    throw new ServiceException($"Zone with id {zoneId} already exists in Warehouse with id {warehouseId}.");
                }

                if (warehouse.zoneList.Count + 1 > warehouse.warehouseCapacity)
                {
                    throw new ServiceException($"Adding zone with id {zoneId} would exceed warehouse capacity, therefore Zone not created.");
                }

                Zone newZone = new Zone(zoneId, zoneName, zoneCapacity, itemPlacementTime, itemRetrievalTime, zonePacketList);

                warehouse.zoneList.Add(newZone);
                OnZoneCreated(warehouse, newZone);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Funksjon for å ta vekk en sone fra et varehus.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        /// <param name="zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner.</param>
        public void RemoveZone(int warehouseId, int zoneId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} does not exist.");
                }

                var zoneToRemove = warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId);
                if (zoneToRemove == null)
                {
                    throw new ServiceException($"Zone with id {zoneId} does not exist in Warehouse with 5id {warehouseId}. Therefore zone could not be removed.");
                }

                warehouse.zoneList.Remove(zoneToRemove);
                OnZoneRemoved(warehouse, zoneToRemove);

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally handle the exception here if needed
            }
        }
        public bool CanAddItemsToZone(int warehouseId, int zoneId, int quantityToAdd)
        {
            try
            {
                var warehouse = FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                }

                var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (zone == null)
                {
                    throw new ServiceException($"Zone with ID {zoneId} in Warehouse {warehouseId} does not exist.");
                }

                int currentItemCount = zone.itemsInZoneList.Sum(item => item.quantity);
                return currentItemCount + quantityToAdd <= zone.shelfCapacity;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Funksjon for å skrive ut alle soner for et varehus.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        public void GetAllZonesInWarehouse(int warehouseId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                Console.WriteLine($"Zones in Warehouse {warehouse.warehouseName}:");

                //var zone = warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId);

                foreach (var zone in warehouse.zoneList)
                {
                    Console.WriteLine($"Zone ID: {zone.zoneId}, Name: {zone.zoneName}, Capacity: {zone.shelfCapacity} , {zone.itemPlacementTime}, {zone.itemRetrievalTime}, {zone.storageType}");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// Funksjon for å finne en sone ved hjelp av Id.
        /// </summary>
        /// <param name="warehouseId"></param>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public void FindZoneById(int warehouseId, int zoneId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} not found");
                }
                else if (warehouse.zoneList.Any(zone => zone.zoneId != zoneId))
                {
                    throw new ServiceException($"Zone with id {zoneId} not found in warehouse with id {warehouseId}");
                }
                else
                {
                    Console.WriteLine(warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId));
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            /*if (warehouse != null)
            {
                return warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId);
            }
            return null; // Returnerer null hvis varehuset eller sonen ikke ble funnet

            */
        }
        //$"Zone with id {zoneId} not found in warehouse with id {warehouseId}"

        public Zone FindAvailableZoneForItem(int warehouseId, int preferredZoneId, int quantity)
        {
            try
            {
                var warehouse = FindWarehouseInWarehouseListWithPrint(warehouseId, false);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                }

                // Først sjekk den foretrukne sonen
                var preferredZone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == preferredZoneId);
                if (preferredZone != null && CanAddItemsToZone(warehouseId, preferredZoneId, quantity))
                {
                    return preferredZone;
                }

                // Hvis den foretrukne sonen ikke har plass, søk etter en annen sone
                foreach (var zone in warehouse.zoneList)
                {
                    if (CanAddItemsToZone(warehouseId, zone.zoneId, quantity))
                    {
                        return zone;
                    }
                }

                // Hvis ingen soner har plass, returner null
                throw new ServiceException($"No available zone found in warehouse {warehouseId} for quantity {quantity}.");
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Funksjon for å skrive ut alle varer i en spesifisert sone.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        /// <param name="zoneId">Dette er en Id for hver sone for å lett kunne holde orden på soner.</param>
        public void GetAllItemsInZone(int warehouseId, int zoneId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                var zone = warehouse.zoneList.FirstOrDefault(zone => zone.zoneId == zoneId);
                if (zone == null)
                {
                    throw new ServiceException($"Zone with id: {zoneId} does not exist in Warehouse id {warehouseId}.");
                }

                Console.WriteLine($"Items in Zone '{zone.zoneName}' in Warehouse '{warehouse.warehouseName}':");

                if (zone.itemsInZoneList.Count == 0)
                {
                    throw new ServiceException("There are no items in this zone.");
                }
                else
                {
                    foreach (var item in zone.itemsInZoneList)
                    {
                        Console.WriteLine($"Item ID: {item.internalId}, Name: {item.name}, Quantity: {item.quantity}");
                    }

                }
            }

            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        ///                                              ///
        ///   Service funksjoner for Employee.cs         ///
        ///                                              ///

        /// <summary>
        /// Funksjon for å oprette ansatte for et gitt varehus.
        /// </summary>
        public void CreateEmployee(int warehouseId, int employeeId, string employeeName)
        {

            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                //if (employeeId == null)
                //{
                //    throw new ServiceException($"Employee with id: {employeeId} Already exists.");
                //}

                Employee newEmployee = new(employeeId, employeeName);
                warehouse.employeeList.Add(newEmployee);
                OnEmployeeCreated(warehouse, newEmployee);

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// Funksjon for å ta vekk en ansatt fra et gitt varehus.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        /// <param name="employeeId">Ansatt id.</param>
        public void RemoveEmployee(int warehouseId, int employeeId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");

                }
                var employee = warehouse.employeeList.FirstOrDefault(Employee => Employee.employeeId == employeeId);
                if (employee == null)
                {
                    throw new ServiceException($"Employee with id: {employeeId} does not exists.");
                }

                warehouse.employeeList.Remove(employee);
                OnEmployeeRemoved(warehouse, employee);

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        /// <summary>
        /// Funksjon for å vise frem alle ansatte for et varehus.
        /// </summary>
        /// <param name="warehouseId">int tall for å gi en identifikator for et gitt varehus.</param>
        public void GetAllEmployeesInWarehouse(int warehouseId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                Console.WriteLine($"Employees in Warehouse '{warehouse.warehouseName}':");

                foreach (var employee in warehouse.employeeList)
                {
                    Console.WriteLine($"Employee ID: {employee.employeeId}, Name: {employee.employeeName}");
                }
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SetAccessToHighValueGoods(int warehouseId, int employeeId, bool hasAccess)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                var employee = warehouse.employeeList.FirstOrDefault(employee => employee.employeeId == employeeId);

                if (employee == null)
                {
                    throw new ServiceException($"Employee with id: {employeeId} does not exists.");
                }

                employee.employeeAuthorizationToHighValueGoods = hasAccess;
                Console.WriteLine($"Access to high value goods for employee {employeeId} in warehouse {warehouseId} set to {hasAccess}.");
                //sett til event

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// for å kunne vise en bruker hvilken autorisasjon den ansatte har
        public void CheckEmployeeAccessStatus(int warehouseId, int employeeId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                var employee = warehouse.employeeList.FirstOrDefault(employee => employee.employeeId == employeeId);

                if (employee == null)
                {
                    throw new ServiceException($"Employee with id: {employeeId} does not exists.");
                }

                Console.WriteLine($"Employee {employeeId} has authorization status: {employee.employeeAuthorizationToHighValueGoods}.");

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        ///                                              ///
        ///       Service funksjoner for Shelf.cs        ///
        ///                                              ///



        public void AddShelfToZone(int zoneId, int length, int depth, int palletCapacity, int floors = 0)
        {
            foreach (var warehouse in warehouseList)
            {
                var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (zone == null)
                {
                    throw new ServiceException($"Zone with ID {zoneId} not found.");
                }

                if (zone.shelves.Count >= zone.shelfCapacity)
                {
                    throw new ServiceException($"Cannot add more shelves to zone {zone.zoneName} as it has reached its capacity.");
                    // Avslutter metoden hvis kapasiteten er nådd
                }

                // Opprett et nytt Shelf-objekt ved å bruke de medfølgende parameterne
                Shelf newShelf = new Shelf(length, depth, palletCapacity, floors)
                {
                    // ShelfId genereres automatisk i Shelf-konstruktøren
                };

                // Legg til det nyopprettede hylleobjektet i sonens shelves-liste
                zone.shelves.Add(newShelf);
                Console.WriteLine($"Shelf with ID {newShelf.shelfId} added to zone {zone.zoneName}.");
            }
        }


        public void RemoveShelfFromZone(int zoneId, int shelfId)
        {
            foreach (var warehouse in warehouseList)
            {
                var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (zone != null)
                {
                    var shelfToRemove = zone.shelves.FirstOrDefault(s => s.shelfId == shelfId);
                    if (shelfToRemove != null)
                    {
                        zone.shelves.Remove(shelfToRemove);
                        Console.WriteLine($"Shelf with ID {shelfId} removed from zone {zone.zoneName}.");
                        return;
                    }
                    else
                    {
                        throw new ServiceException($"Shelf with ID {shelfId} not found in zone {zone.zoneName}.");
                    }
                }
            }
            throw new ServiceException($"Zone with ID {zoneId} not found.");
        }




        public void UpdateShelf(int shelfId, int newLength, int newDepth, int newCapacity)
        {
            var shelf = FindShelfById(shelfId);
            if (shelf != null)
            {
                // Oppdaterer reolens egenskaper med de nye verdiene
                shelf.length = newLength;
                shelf.depth = newDepth;
                shelf.palletCapacity = newCapacity;
                Console.WriteLine($"Shelf with ID {shelfId} has been updated.");
            }
            else
            {
                throw new ServiceException($"Shelf with ID {shelfId} not found.");
            }
        }



        public Shelf FindShelfById(int shelfId)
        {
            foreach (var warehouse in warehouseList)
            {
                foreach (var zone in warehouse.zoneList)
                {
                    var shelf = zone.shelves.FirstOrDefault(s => s.shelfId == shelfId);
                    if (shelf != null)
                    {
                        return shelf; // Returnerer reolen hvis funnet
                    }
                }
            }
            return null; // Returnerer null hvis reolen ikke ble funnet
        }



        public List<Shelf> GetAllShelvesInZone(int warehouseId, int zoneId)
        {
            try
            {
                // Finner det spesifikke varehuset basert på warehouseId
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    return new List<Shelf>();
                    throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                     // Returnerer en tom liste hvis varehuset ikke ble funnet
                }

                // Finner den spesifikke sonen innenfor det valgte varehuset
                var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
                if (zone == null)
                {
                    return new List<Shelf>();
                    throw new ServiceException($"Zone with ID {zoneId} in warehouse {warehouseId} does not exist.");
                    // Returnerer en tom liste hvis sonen ikke ble funnet
                }

                // Returnerer listen over hyller i sonen
                return zone.shelves;
            }
            catch (Exception ex)
            {
                return new List<Shelf>(); // Returnerer en tom liste ved feil
                throw new ServiceException($"An unexpected error occurred: {ex.Message}");
                
            }
        }

        public int CalculateTotalCapacityInZone(int zoneId)
        {
            int totalCapacity = 0;
            foreach (var warehouse in warehouseList)
            {
                foreach (var zone in warehouse.zoneList)
                {
                    if (zone.zoneId == zoneId)
                    {
                        foreach (var shelf in zone.shelves)
                        {
                            totalCapacity += shelf.palletCapacity; // Summerer kapasiteten for hver hylle
                        }
                        return totalCapacity; // Returnerer den totale kapasiteten for sonen
                    }
                }
            }
            return 0; // Returnerer 0 hvis sonen ikke ble funnet
        }

        public bool HasAvailableCapacity(Shelf shelf)
        {
            // Anta at Shelf-objektet har en egenskap 'PalletCapacity' som angir maks antall paller,
            // og en liste over paller 'Pallets' som allerede er plassert på reolen.
            int currentPalletCount = waresOutService.palletList.Count;

            // Sjekker om antallet paller som allerede er plassert på reolen er mindre enn reolens totale kapasitet.
            return currentPalletCount < shelf.palletCapacity;
        }

        public void PlaceItemOnShelf(int internalId, int shelfId, int zoneId, int warehouseId)
        {
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with ID {warehouseId} not found.");
                return;
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            if (zone == null)
            {
                throw new ServiceException($"Zone with ID {zoneId} not found in warehouse {warehouseId}.");
            }

            var shelf = zone.shelves.FirstOrDefault(s => s.shelfId == shelfId);
            if (shelf == null)
            {
                throw new ServiceException($"Shelf with ID {shelfId} not found in zone {zoneId}.");
            }

            if (!HasAvailableCapacity(shelf))
            {
                throw new ServiceException($"No available capacity on shelf {shelf.shelfId} in zone {zoneId}.");
            }

            var item = warehouse.itemList.FirstOrDefault(i => i.internalId == internalId);
            if (item == null)
            {
                throw new ServiceException($"Item with internal ID {internalId} not found in warehouse {warehouseId}.");

            }

            if (!zone.itemsInZoneList.Contains(item))
            {
                zone.itemsInZoneList.Add(item);
                Console.WriteLine($"Item {item.internalId} placed on shelf {shelf.shelfId} in zone {zoneId}.");
            }
            else
            {
                throw new ServiceException($"Item {item.internalId} is already placed in zone {zoneId}.");
            }
        }
        //public bool IsStorageTypeCompatible(Zone zone, Item item)
        //{
        //    // Sjekker om zonens liste over støttede lagringstyper inneholder item sin lagringstype
        //    bool isCompatible = zone.zonePacketList.Contains(item.storageType);
        //    if (!isCompatible)
        //    {
        //        Console.WriteLine($"Adding item {item.internalId} with storage type {item.storageType} cannot be placed in the zone {zone.zoneId} since this zone supports storage types of {string.Join(", ", zone.zonePacketList)}.");
        //    }
        //    return isCompatible;
        //}


        public bool IsStorageTypeCompatible(Zone zone, Item item)
        {
            // Hvis zonePacketList er definert og inneholder elementer, bruk den for kompatibilitetssjekk
            if (zone.zonePacketList != null && zone.zonePacketList.Any())
            {
                return zone.zonePacketList.Contains(item.storageType);
            }
            // Hvis zonePacketList ikke er brukt eller er tom, fall tilbake på å bruke sonens enkelt StorageType for sjekk
            else
            {
                return zone.storageType == item.storageType || zone.storageType == StorageType.None;
            }
        }

    }
}

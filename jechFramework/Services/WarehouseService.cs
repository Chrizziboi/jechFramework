using jechFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace jechFramework.Services
{
    /// <summary>
    /// Service-klasse for håndtering av operasjoner relatert til varehus, ansatte, soner og hyller.
    /// </summary>
    public class WarehouseService 
    {

        private readonly Warehouse warehouseInstance;
        private readonly Shelf shelfInstance;
        private readonly ItemService itemService;
        private readonly WaresOutService waresOutService;

        /// <summary>
        /// Liste over alle varehus.
        /// </summary>
        public List<Warehouse> warehouseList = new List<Warehouse>();

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

        /// <summary>
        /// Kalles når et nytt varehus opprettes.
        /// </summary>
        /// <param name="warehouse">Det opprettede varehuset.</param>
        public void OnWarehouseCreated(Warehouse warehouse)
        {       
            WarehouseCreated?.Invoke(this, new WarehouseEventArgs(warehouse));
        }

        /// <summary>
        /// Kalles når et varehus fjernes.
        /// </summary>
        /// <param name="warehouse">Det fjernede varehuset.</param>
        public void OnWarehouseRemoved(Warehouse warehouse)
        {
            WarehouseRemoved?.Invoke(this, new WarehouseEventArgs(warehouse));
        }

        /// <summary>
        /// Kalles når en ny sone opprettes.
        /// </summary>
        /// <param name="warehouse">Varehuset som sonen tilhører.</param>
        /// <param name="zone">Den opprettede sonen.</param>
        public void OnZoneCreated(Warehouse warehouse, Zone zone)
        {
            ZoneCreated?.Invoke(this, new ZoneEventArgs(warehouse, zone));
        }

        /// <summary>
        /// Kalles når en sone fjernes.
        /// </summary>
        /// <param name="warehouse">Varehuset som sonen tilhører.</param>
        /// <param name="zone">Den fjernede sonen.</param>
        public void OnZoneRemoved(Warehouse warehouse, Zone zone)
        {
            ZoneRemoved?.Invoke(this, new ZoneEventArgs(warehouse, zone));
        }

        /// <summary>
        /// Kalles når en ny ansatt opprettes.
        /// </summary>
        /// <param name="warehouse">Varehuset som den ansatte tilhører.</param>
        /// <param name="employee">Den opprettede ansatte.</param>
        public void OnEmployeeCreated(Warehouse warehouse, Employee employee)
        {
            EmployeeCreated?.Invoke(this, new EmployeeEventArgs(warehouse, employee));
        }

        /// <summary>
        /// Kalles når en ansatt fjernes.
        /// </summary>
        /// <param name="warehouse">Varehuset som den ansatte tilhører.</param>
        /// <param name="employee">Den fjernede ansatte.</param>
        public void OnEmployeeRemoved(Warehouse warehouse, Employee employee)
        {
            EmployeeRemoved?.Invoke(this, new EmployeeEventArgs(warehouse, employee));
        }

        ///                                        ///
        ///   Service funksjoner for Warehouse.cs  ///
        ///                                        ///

        /// <summary>
        /// Oppretter et nytt varehus.
        /// </summary>
        /// <param name="warehouseId">ID for det nye varehuset.</param>
        /// <param name="warehouseName">Navn på det nye varehuset.</param>
        /// <param name="warehouseCapacity">Kapasiteten til det nye varehuset.</param>                              
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
                Console.WriteLine($"Error creating warehouse: {ex.Message}.");
            }
        }

        /// <summary>
        /// Finner et varehus i listen og skriver ut detaljene hvis ønskelig.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset som skal finnes.</param>
        /// <param name="printDetails">Angir om detaljene til varehuset skal skrives ut.</param>
        /// <returns>Returnerer det funnet varehuset.</returns>
        /// <exception cref="ServiceException">Kastes hvis varehuset ikke finnes.</exception>
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

        /// <summary>
        /// Returnerer en liste over alle varehus.
        /// </summary>
        /// <returns>Liste over alle varehus.</returns>
        public List<Warehouse> GetAllWarehouses()
        {
            return warehouseList;
            

        }

        /// <summary>
        /// Fjerner et varehus fra listen.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset som skal fjernes.</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset ikke finnes.</exception>
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
        /// Oppretter en ny sone i et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset hvor sonen skal opprettes.</param>
        /// <param name="zoneId">ID for den nye sonen.</param>
        /// <param name="zoneName">Navn på den nye sonen.</param>
        /// <param name="zoneCapacity">Kapasiteten til den nye sonen.</param>
        /// <param name="itemPlacementTime">Tiden det tar å plassere varer i sonen.</param>
        /// <param name="itemRetrievalTime">Tiden det tar å hente varer fra sonen.</param>
        /// <param name="storageType">Lagringstypen til den nye sonen.</param>
        /// <exception cref="ServiceException">Kastes hvis det oppstår en feil ved opprettelse av sonen.</exception>
        public void CreateZone(
            int warehouseId, 
            int zoneId, 
            string zoneName, 
            int zoneCapacity, 
            TimeSpan itemPlacementTime, 
            TimeSpan itemRetrievalTime, 
            StorageType storageType)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} does not exist.");
                }

                if (warehouse.zoneList.Any(z => z.zoneId == zoneId))
                {
                    throw new ServiceException($"Zone with id {zoneId} already exists in Warehouse with id {warehouseId}.");
                }

                if (warehouse.zoneList.Count >= warehouse.warehouseCapacity)
                {
                    throw new ServiceException($"Cannot add new zone. WarehouseId {warehouseId} at capacity.");
                }

                var newZone = new Zone(zoneId, zoneName, zoneCapacity, itemPlacementTime, itemRetrievalTime, storageType);
                warehouse.zoneList.Add(newZone);
                OnZoneCreated(warehouse, newZone);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Oppretter en ny sone med flere lagringstyper i et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset hvor sonen skal opprettes.</param>
        /// <param name="zoneId">ID for den nye sonen.</param>
        /// <param name="zoneName">Navn på den nye sonen.</param>
        /// <param name="zoneCapacity">Kapasiteten til den nye sonen.</param>
        /// <param name="itemPlacementTime">Tiden det tar å plassere varer i sonen.</param>
        /// <param name="itemRetrievalTime">Tiden det tar å hente varer fra sonen.</param>
        /// <param name="zonePacketList">Liste over lagringstyper for sonen.</param>
        /// <exception cref="ServiceException">Kastes hvis det oppstår en feil ved opprettelse av sonen.</exception>
        public void CreateZoneWithMultipleType(
            int warehouseId,
            int zoneId,
            string zoneName,
            int zoneCapacity,
            TimeSpan itemPlacementTime,
            TimeSpan itemRetrievalTime,
            List<StorageType> zonePacketList)
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

                if (zonePacketList == null || zonePacketList.Count == 0)
                {
                    throw new ServiceException("Zone must have at least one storage type defined.");
                }

                Zone newZone = new Zone(zoneId, zoneName, zoneCapacity, itemPlacementTime, itemRetrievalTime, zonePacketList);
                warehouse.zoneList.Add(newZone);

                // Print the storage types for verification
                Console.WriteLine($"Zone Created: {zoneName} and ID: {zoneId} with Storage Types: {string.Join(", ", zonePacketList)}");

                OnZoneCreated(warehouse, newZone);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        /// Fjerner en sone fra et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset hvor sonen skal fjernes.</param>
        /// <param name="zoneId">ID for sonen som skal fjernes.</param>
        /// <exception cref="ServiceException">Kastes hvis det oppstår en feil ved fjerning av sonen.</exception>
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

        /// <summary>
        /// Sjekker om det er plass til å legge til flere varer i en sone.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="quantityToAdd">Antall varer som skal legges til.</param>
        /// <returns>Returnerer true hvis det er plass, ellers false.</returns>
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
        /// Returnerer en liste over alle soner i et varehus og skriver ut detaljene.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <returns>Liste over alle soner i varehuset.</returns>
        /// <exception cref="ServiceException">Kastes hvis varehuset ikke finnes.</exception>
        public List<Zone> GetAllZonesInWarehouse(int warehouseId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                if (warehouse.zoneList == null || !warehouse.zoneList.Any())
                {
                    Console.WriteLine($"No zones found in Warehouse {warehouse.warehouseName}.");
                    return new List<Zone>();  // Returnerer en tom liste hvis ingen soner finnes
                }

                // Hvis soner finnes, skriver vi ut informasjon om hver sone og returnerer listen
                Console.WriteLine($"Zones in Warehouse {warehouse.warehouseName}:");
                foreach (var zone in warehouse.zoneList)
                {
                    Console.WriteLine(zone.ToString());
                }

                return warehouse.zoneList; // Returnerer listen av soner
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Zone>(); // Returnerer en tom liste i tilfelle av feil
            }
        }

        /// <summary>
        /// Finner en sone basert på ID.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <returns>Returnerer sonen hvis funnet, ellers null.</returns>
        public Zone FindZoneById(int warehouseId, int zoneId)
        {
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);

            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with id {warehouseId} not found.");
                return null;
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);

            if (zone == null)
            {
                Console.WriteLine($"Zone with id {zoneId} not found in warehouse with id {warehouseId}.");
                return null;
            }

            else
            {
                Console.WriteLine($"ZoneId: {zoneId} found in warehouse {warehouseId}.");
            }

            return zone;
        }

        /// <summary>
        /// Finner en tilgjengelig sone for en vare basert på preferanse og mengde.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="preferredZoneId">Foretrukket sone-ID.</param>
        /// <param name="quantity">Mengde varer.</param>
        /// <returns>Returnerer en tilgjengelig sone hvis funnet, ellers null.</returns>
        /// <exception cref="ServiceException">Kastes hvis ingen tilgjengelig sone finnes.</exception>
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
        /// Returnerer en liste over alle varer i en spesifisert sone og skriver ut detaljene.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <returns>Liste over alle varer i sonen.</returns>
        /// <exception cref="ServiceException">Kastes hvis sonen ikke finnes eller er tom.</exception>
        public List<Item> GetAllItemsInZone(int warehouseId, int zoneId)
        {
            List<Item> items = new List<Item>();
            try
            {
                Warehouse warehouse = warehouseList.FirstOrDefault(wh => wh.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                Zone zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);

                if (zone == null)
                {
                    throw new ServiceException($"Zone with id: {zoneId} does not exist in Warehouse id {warehouseId}.");
                }

                if (zone.itemsInZoneList.Count == 0)
                {
                    throw new ServiceException("There are no items in this zone.");
                }

                items = zone.itemsInZoneList;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return items;
        }

        ///                                              ///
        ///   Service funksjoner for Employee.cs         ///
        ///                                              ///

        /// <summary>
        /// Oppretter en ny ansatt for et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <param name="employeeName">Navn på den ansatte.</param>
        /// <exception cref="ServiceException">Kastes hvis det oppstår en feil ved opprettelse av den ansatte.</exception>
        public void CreateEmployee(int warehouseId, int employeeId, string employeeName)
        {

            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }
                
                if (employeeId == null)
                {
                    throw new ServiceException($"Employee with id: {employeeId} Already exists.");
                }

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
        /// Fjerner en ansatt fra et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <exception cref="ServiceException">Kastes hvis den ansatte ikke finnes.</exception>
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
        /// Returnerer en liste over alle ansatte i et varehus.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <returns>Liste over alle ansatte i varehuset.</returns>
        /// <exception cref="ServiceException">Kastes hvis varehuset ikke finnes.</exception>
        public List<Employee> GetAllEmployeesInWarehouse(int warehouseId)
        {
            try
            {
                var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id: {warehouseId} does not exist.");
                }

                // Returnerer listen direkte uten utskrift
                return warehouse.employeeList;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Employee>(); // Returnerer en tom liste hvis det oppstår en feil
            }
        }

        /// <summary>
        /// Setter tilgang til høyverdivarer for en ansatt.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <param name="hasAccess">True hvis den ansatte skal ha tilgang, ellers false.</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset eller den ansatte ikke finnes.</exception>
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

        /// <summary>
        /// Sjekker tilgangsstatusen til en ansatt for høyverdivarer.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="employeeId">ID for den ansatte.</param>
        /// <returns>True hvis den ansatte har tilgang, ellers false.</returns>
        /// <exception cref="ServiceException">Kastes hvis varehuset eller den ansatte ikke finnes.</exception>
        public bool CheckEmployeeAccessStatus(int warehouseId, int employeeId)
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
                    throw new ServiceException($"Employee with id: {employeeId} does not exist.");
                }

                // Returnerer autorisasjonsstatusen til den ansatte
                return employee.employeeAuthorizationToHighValueGoods;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                return false; // Returnerer false hvis det oppstår en feil
            }
        }

        ///                                              ///
        ///       Service funksjoner for Shelf.cs        ///
        ///                                              ///

        /// <summary>
        /// Legger til en hylle i en sone.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="length">Lengden på hyllen.</param>
        /// <param name="depth">Dybden på hyllen.</param>
        /// <param name="palletCapacity">Pallkapasiteten til hyllen.</param>
        /// <param name="floors">Antall etasjer (valgfritt).</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset eller sonen ikke finnes, eller kapasiteten er nådd.</exception>
        public void AddShelfToZone(
            int warehouseId,
            int zoneId,
            int length,
            int depth,
            int palletCapacity,
            int floors = 0)
        {

            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            
            if (warehouse == null) 
            { 
                throw new ServiceException($"Warehouse with Id {warehouseId} not found.");
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            
            if (zone == null)
            {
                throw new ServiceException($"Zone with ID {zoneId} not found.");
            }

            if (zone.shelves == null)
            {
                zone.shelves = new List<Shelf>();  // Initialiserer shelves hvis den er null
            }

            if (zone.shelves.Count >= zone.shelfCapacity)
            {
                throw new ServiceException($"Cannot add more shelves to zone {zone.zoneName} as it has reached its capacity.");
            }

            Shelf newShelf = new Shelf(length, depth, palletCapacity, floors);
            zone.shelves.Add(newShelf);
            Console.WriteLine($"Shelf with ID {newShelf.shelfId} added to zone {zone.zoneName}.");
            
        }
        /// <summary>
        /// Fjerner en hylle fra en sone.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="shelfId">ID for hyllen som skal fjernes.</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset, sonen eller hyllen ikke finnes.</exception>
        public void RemoveShelfFromZone(int warehouseId, int zoneId, int shelfId)
        {
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with Id {warehouseId} not found.");
            }

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

        /// <summary>
        /// Oppdaterer egenskapene til en hylle.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="shelfId">ID for hyllen som skal oppdateres.</param>
        /// <param name="newLength">Ny lengde for hyllen.</param>
        /// <param name="newDepth">Ny dybde for hyllen.</param>
        /// <param name="newCapacity">Ny kapasitet for hyllen.</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset, sonen eller hyllen ikke finnes.</exception>
        public void UpdateShelf(
            int warehouseId,
            int zoneId,
            int shelfId,
            int newLength,
            int newDepth,
            int newCapacity)
        {
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with Id {warehouseId} not found.");
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            
            if (zone != null)
            {
                var shelf = FindShelfById(warehouseId, zoneId, shelfId);
                
                if (shelf != null)
                {
                    // Oppdaterer reolens egenskaper med de nye verdiene
                    shelf.length = newLength;
                    shelf.depth = newDepth;
                    shelf.palletCapacity = newCapacity;
                    Console.WriteLine($"Shelf with ID {shelfId} has been updated.");
                }
            }
            
            else
            {
                throw new ServiceException($"Shelf with ID {shelfId} not found.");
            }
        }

        /// <summary>
        /// Finner en hylle basert på ID.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="shelfId">ID for hyllen.</param>
        /// <returns>Returnerer hyllen hvis funnet, ellers null.</returns>
        public Shelf FindShelfById(int warehouseId, int zoneId, int shelfId)
        {
            
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            
            if (warehouse == null)
            {
                Console.WriteLine($"No warehouse found with ID {warehouseId}.");
                return null; 
            }

            
            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            
            if (zone == null)
            {
                Console.WriteLine($"No zone found with ID {zoneId} in warehouse ID {warehouseId}.");
                return null; 
            }

            
            var shelf = zone.shelves.FirstOrDefault(s => s.shelfId == shelfId);
            
            if (shelf != null)
            {
                return shelf; 
            }

            Console.WriteLine($"No shelf found with ID {shelfId} in zone ID {zoneId} of warehouse ID {warehouseId}.");
            return null; 
        }

        /// <summary>
        /// Returnerer en liste over alle hyller i en sone.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <returns>Liste over alle hyller i sonen.</returns>
        /// <exception cref="ServiceException">Kastes hvis varehuset eller sonen ikke finnes.</exception>
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
                throw new ServiceException($"An unexpected error occurred: {ex.Message}.");    
            }
        }

        /// <summary>
        /// Beregner den totale kapasiteten for varer i en sone.
        /// </summary>
        /// <param name="zoneId">ID for sonen.</param>
        /// <returns>Total kapasitet for varer i sonen.</returns>
        public int CalculateTotalItemCapacityInZone(int zoneId)
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

        /// <summary>
        /// Plasserer en vare på en hylle.
        /// </summary>
        /// <param name="warehouseId">ID for varehuset.</param>
        /// <param name="zoneId">ID for sonen.</param>
        /// <param name="shelfId">ID for hyllen.</param>
        /// <param name="internalId">Intern ID for varen.</param>
        /// <exception cref="ServiceException">Kastes hvis varehuset, sonen, hyllen eller varen ikke finnes.</exception>
        public void PlaceItemOnShelf(int warehouseId, int zoneId, int shelfId, int internalId)
        {
            var warehouse = warehouseList.FirstOrDefault(w => w.warehouseId == warehouseId);
            
            if (warehouse == null)
            {
                throw new ServiceException($"Warehouse with ID {warehouseId} not found.");  
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

        /// <summary>
        /// Sjekker om lagringstypen til en vare er kompatibel med en sone.
        /// </summary>
        /// <param name="zone">Sonen som skal sjekkes.</param>
        /// <param name="item">Varen som skal sjekkes.</param>
        /// <returns>True hvis kompatibel, ellers false.</returns>
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

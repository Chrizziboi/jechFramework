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

        private List<Warehouse> warehouseList = new List<Warehouse>();
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
                var newWarehouse = new Warehouse(warehouseId, warehouseName, warehouseCapacity);
                if (warehouseList.Any(w => w.warehouseId == warehouseId))
                {
                    throw new ServiceException($"Warehouse already exist.");
                    // (myValue == 1 || myValue == 2 || myValue == 3) !=
                    //  || or - && and
                }

                warehouseList.Add(newWarehouse);

                OnWarehouseCreated(newWarehouse);
                
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Funksjon for å finne et varehus i varehus-listen
        /// </summary>
        /// <param name="warehouseId"></param>

        /// <exception cref="ServiceException"></exception>
        public void FindWareHouseInWarehouseList(int warehouseId)
        {
            try 
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);

                if (warehouse == null)
                {
                    
                    throw new ServiceException($"A warehouse with id{warehouseId} could not be found.");

                }

                Console.WriteLine($"warehouse Id: {warehouse.warehouseId}\n" +
                                  $"warehouse Name: {warehouse.warehouseName}\n" +
                                  $"warehouse Capacity: {warehouse.warehouseCapacity}\n");

            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
            }

        /// <exception cref="InvalidOperationException"></exception>
        public Warehouse FindWarehouseInWarehouseListWithPrint(int warehouseId, bool printDetails = true)
        {
            var warehouse = warehouseList.FirstOrDefault(wh => wh.warehouseId == warehouseId);

            if (warehouse == null)
            {
                Console.WriteLine($"A warehouse with id: {warehouseId} could not be found.");
                throw new InvalidOperationException($"A warehouse with id {warehouseId} could not be found.");
            }

            if (printDetails)
            {
                Console.WriteLine($"warehouse Id: {warehouse.warehouseId}\n" +
                                  $"warehouse Name: {warehouse.warehouseName}\n" +
                                  $"warehouse Capacity: {warehouse.warehouseCapacity}\n");
            }

            return warehouse; // Returnerer Warehouse-objektet hvis funnet
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
        public void CreateZone(int warehouseId, int zoneId, string zoneName, int zoneCapacity)
        {

            try
            {
                var warehouse = warehouseList.FirstOrDefault(warehouse => warehouse.warehouseId == warehouseId);
                if (warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} does not exist.");
                }
                var existingZone = warehouse.zoneList.FirstOrDefault(existingZone => existingZone.zoneId == zoneId);
                if (existingZone != null)
                {
                    throw new ServiceException($"Zone with id {zoneId} already exists in Warehouse with id {warehouseId}.");
                }

                // Check if adding this zone would exceed warehouse capacity
                int totalZoneCapacity = warehouse.zoneList.Count() + 1;

                if (totalZoneCapacity > warehouse.warehouseCapacity)
                {
                    throw new ServiceException($"Adding zone with id {zoneId} would exceed warehouse capacity, " +
                                               $"therefore Zone not created.");
                }

                Zone newZone = new(zoneId, zoneName, zoneCapacity);
                warehouse.zoneList.Add(newZone);

                OnZoneCreated(warehouse, newZone);
                //Console.WriteLine($"Successfully created Zone: {zoneId} in warehouse: {warehouseId}.");
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Message);
                // Optionally handle the exception here if needed
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
            var warehouse = FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                return false;
            }

            var zone = warehouse.zoneList.FirstOrDefault(z => z.zoneId == zoneId);
            if (zone == null)
            {
                Console.WriteLine($"Zone with ID {zoneId} in Warehouse {warehouseId} does not exist.");
                return false;
            }

            int currentItemCount = zone.ItemsInZoneList.Sum(item => item.quantity);
            return currentItemCount + quantityToAdd <= zone.zoneCapacity;
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

                foreach (var zone in warehouse.zoneList)
                {
                    Console.WriteLine($"Zone ID: {zone.zoneId}, Name: {zone.zoneName}, Capacity: {zone.zoneCapacity}");
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
                if(warehouse == null)
                {
                    throw new ServiceException($"Warehouse with id {warehouseId} not found");
                }
                else if(warehouse.zoneList.Any(zone => zone.zoneId != zoneId))
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
            var warehouse = FindWarehouseInWarehouseList(warehouseId, false);
            if (warehouse == null)
            {
                Console.WriteLine($"Warehouse with ID {warehouseId} not found.");
                return null;
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
            Console.WriteLine($"No available zone found in warehouse {warehouseId} for quantity {quantity}.");
            return null;
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

                if (zone.ItemsInZoneList.Count == 0)
                {
                    Console.WriteLine("There are no items in this zone.");
                }
                else
                {
                    foreach (var item in zone.ItemsInZoneList)
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
    }
}
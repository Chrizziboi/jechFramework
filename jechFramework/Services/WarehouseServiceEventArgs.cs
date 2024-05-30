using jechFramework.Models;

namespace jechFramework.Services
{

    /// <summary>
    /// Event arguments klasse for WarehouseService.
    /// </summary>
    public class WarehouseServiceEventArgs()
    {

    }

    /// <summary>
    /// Event arguments klasse for Warehouse-relaterte hendelser.
    /// </summary>
    public class WarehouseEventArgs : EventArgs
    {

        /// <summary>
        /// Lageret som er relatert til hendelsen.
        /// </summary>
        public Warehouse Warehouse { get; private set; }

        /// <summary>
        /// Konstruktør for WarehouseEventArgs.
        /// </summary>
        /// <param name="warehouse">Lageret relatert til hendelsen.</param>
        public WarehouseEventArgs(Warehouse warehouse)
        {
            this.Warehouse = warehouse;
        }
    }

    /// <summary>
    /// Event arguments klasse for Zone-relaterte hendelser.
    /// </summary>
    public class ZoneEventArgs : EventArgs
    {

        /// <summary>
        /// Lageret som inneholder sonen.
        /// </summary>
        public Warehouse Warehouse { get; private set; }

        /// <summary>
        /// Sonen som er relatert til hendelsen.
        /// </summary>
        public Zone Zone { get; private set; }

        /// <summary>
        /// Konstruktør for ZoneEventArgs.
        /// </summary>
        /// <param name="warehouse">Lageret som inneholder sonen.</param>
        /// <param name="zone">Sonen relatert til hendelsen.</param>
        public ZoneEventArgs(Warehouse warehouse, Zone zone)
        {
            this.Warehouse = warehouse;
            this.Zone = zone;
        }
    }

    /// <summary>
    /// Event arguments klasse for Employee-relaterte hendelser.
    /// </summary>
    public class EmployeeEventArgs : EventArgs
    {

        /// <summary>
        /// Lageret som inneholder den ansatte.
        /// </summary>
        public Warehouse Warehouse { get; private set; }

        /// <summary>
        /// Den ansatte som er relatert til hendelsen.
        /// </summary>
        public Employee Employee { get; private set; }

        /// <summary>
        /// Konstruktør for EmployeeEventArgs.
        /// </summary>
        /// <param name="warehouse">Lageret som inneholder den ansatte.</param>
        /// <param name="employee">Den ansatte relatert til hendelsen.</param>
        public EmployeeEventArgs(Warehouse warehouse, Employee employee)
        {
            this.Warehouse = warehouse;
            this.Employee = employee;
        }
    }

}
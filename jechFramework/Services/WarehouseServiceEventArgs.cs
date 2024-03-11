using jechFramework.Models;

namespace jechFramework.Services
{
    public class WarehouseServiceEventArgs()
    {

    }

    public class WarehouseEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }

        public WarehouseEventArgs(Warehouse warehouse)
        {
            this.Warehouse = warehouse;

        }
    }

    public class ZoneEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }
        public Zone Zone { get; private set; }

        public ZoneEventArgs(Warehouse warehouse, Zone zone)
        {
            this.Warehouse = warehouse;
            this.Zone = zone;
        }
    }

    public class EmployeeEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }
        public Employee Employee { get; private set; }

        public EmployeeEventArgs(Warehouse warehouse, Employee employee)
        {
            this.Warehouse = warehouse;
            this.Employee = employee;
        }
    }

}
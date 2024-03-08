using jechFramework.Models;

namespace jechFramework.Services
{
        public class WarehouseEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }

        public WarehouseEventArgs(Warehouse warehouse)
        {
            Warehouse = warehouse;
        }
    }

    public class ZoneEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }
        public Zone Zone { get; private set; }

        public ZoneEventArgs(Warehouse warehouse, Zone zone)
        {
            Warehouse = warehouse;
            Zone = zone;
        }
    }

    public class EmployeeEventArgs : EventArgs
    {
        public Warehouse Warehouse { get; private set; }
        public Employee Employee { get; private set; }

        public EmployeeEventArgs(Warehouse warehouse, Employee employee)
        {
            Warehouse = warehouse;
            Employee = employee;
        }
    }



}
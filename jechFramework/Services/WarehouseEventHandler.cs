using jechFramework.Models;

namespace jechFramework.Services
{
    public class WarehouseEventHandler : EventArgs
    {
        public Warehouse Warehouse { get; private set; }

        public WarehouseEventHandler(Warehouse warehouse)
        {
            Warehouse = warehouse;

        }

     //protected virtual void OnWarehouseCreated(Warehouse warehouse) 
     //{
     //    if (WarehouseCreated != null)
     //    {
     //        OnWarehouseCreated(this, new WarehouseEventHandler(warehouse));
     //
     //    }
     //}
     //
      private static void Service_WarehouseCreated(object source, WarehouseEventHandler args) 
      {
            Console.WriteLine($"Warehouse Created: {args.Warehouse.warehouseName}");
      }
       
    }
}
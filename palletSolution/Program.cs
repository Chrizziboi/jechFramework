using jechFramework.Models;
using jechFramework.Services;

namespace palletSolution
{
    class Program
    {
        static void Main(string[] args)
        {

            WarehouseService WService = new();
            PalletService PService = new();
            ItemService IService = new(WService);
            ItemHistoryService IHService = new();
            //Item Item = new();
            WaresInService waresInService = new WaresInService(IService, WService);
            WaresOutService waresOutService = new WaresOutService(IService);
            PalletService palletService = new();

        }
    }
}

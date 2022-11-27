using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RastrPVEqConsole.Models;

namespace RastrPVEqConsole.Infrastructure
{
    public static class RastrSupplierAsync
    {
        public static async Task<List<Node>> GetNodesAsync()
        {
            Console.WriteLine(">>> Start nodes creating");
            var nodes = await Task.Run(() => RastrSupplier.GetNodes());
            Console.WriteLine(">>> Nodes created");
            return nodes;
        }

        public static async Task<List<AdjustmentRange>> GetAdjustmentRangesAsync()
        {
            Console.WriteLine(">>> Start adjustment ranges creating");
            var adjustmentRanges = await Task.Run(() => RastrSupplier.GetAdjustmentRanges());
            Console.WriteLine(">>> Аdjustment ranges created");
            return adjustmentRanges;
        }
    }
}

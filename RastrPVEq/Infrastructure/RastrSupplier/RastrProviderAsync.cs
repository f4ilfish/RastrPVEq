using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RastrPVEq.Models.PowerSystem;

namespace RastrPVEq.Infrastructure.RastrSupplier
{
    /// <summary>
    /// Rastr provider async class 
    /// </summary>
    public static class RastrProviderAsync
    {
        /// <summary>
        /// Get nodes async
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Node>> GetNodesAsync(CancellationToken token)
        {
            var nodes = await Task.Run(() => RastrSupplier.GetNodes(), token);

            if (token.IsCancellationRequested) return nodes;

            return nodes;
        }

        /// <summary>
        /// Get generators async
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <returns></returns>
        public static async Task<List<Generator>> GetGeneratorsAsync(List<Node> nodes, CancellationToken token)
        {
            var generators = await Task.Run(() => RastrSupplier.GetGenerators(nodes), token);

            if (token.IsCancellationRequested) return generators;

            return generators;
        }

        /// <summary>
        /// Get branches async
        /// </summary>
        /// <param name="nodes">List of branches</param>
        /// <returns></returns>
        public static async Task<List<Branch>> GetBranchesAsync(List<Node> nodes, CancellationToken token)
        {
            var branches = await Task.Run(() => RastrSupplier.GetBranches(nodes), token);

            if (token.IsCancellationRequested) return branches;

            return branches;
        }
    }
}

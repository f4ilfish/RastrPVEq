using System.Collections.Generic;
using System.Threading.Tasks;
using RastrPVEq.Models.RastrWin3;

namespace RastrPVEq.Infrastructure.RastrWin3
{
    /// <summary>
    /// Rastr supply async method provider class
    /// </summary>
    public static class RastrSupplierAsync
    {
        /// <summary>
        /// Get nodes async
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Node>> GetNodesAsync()
        {
            var nodes = await Task.Run(() => RastrSupplier.GetNodes());
            return nodes;
        }

        /// <summary>
        /// Get generators async
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <param name="pqDiagrams">List of PQ diagrams</param>
        /// <returns></returns>
        public static async Task<List<Generator>> GetGeneratorsAsync(List<Node> nodes, List<PQDiagram> pqDiagrams)
        {
            var generators = await Task.Run(() => RastrSupplier.GetGenerators(nodes, pqDiagrams));
            return generators;
        }

        /// <summary>
        /// Get PQ diagrams async
        /// </summary>
        /// <returns></returns>
        public static async Task<List<PQDiagram>> GetPQDiagramsAsync()
        {
            var pqDiagrams = await Task.Run(() => RastrSupplier.GetPQDiagrams());
            return pqDiagrams;
        }

        /// <summary>
        /// Get branches async
        /// </summary>
        /// <param name="nodes">List of branches</param>
        /// <returns></returns>
        public static async Task<List<Branch>> GetBranchesAsync(List<Node> nodes)
        {
            var branches = await Task.Run(() => RastrSupplier.GetBranches(nodes));
            return branches;
        }
    }
}

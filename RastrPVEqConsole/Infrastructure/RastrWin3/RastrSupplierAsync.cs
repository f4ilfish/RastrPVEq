using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RastrPVEqConsole.Models.RastrWin3;

namespace RastrPVEqConsole.Infrastructure.RastrWin3
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
            Console.WriteLine(">>> Start nodes creating");
            var nodes = await Task.Run(() => RastrSupplier.GetNodes());
            Console.WriteLine(">>> Nodes created");
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
            Console.WriteLine(">>> Start generators creating");
            var generators = await Task.Run(() => RastrSupplier.GetGenerators(nodes, pqDiagrams));
            Console.WriteLine(">>> Generators created");
            return generators;
        }

        /// <summary>
        /// Get PQ diagrams async
        /// </summary>
        /// <returns></returns>
        public static async Task<List<PQDiagram>> GetPQDiagramsAsync()
        {
            Console.WriteLine(">>> Start PQ diagrams creating");
            var pqDiagrams = await Task.Run(() => RastrSupplier.GetPQDiagrams());
            Console.WriteLine(">>> PQ diagrams created");
            return pqDiagrams;
        }

        /// <summary>
        /// Get branches async
        /// </summary>
        /// <param name="nodes">List of branches</param>
        /// <returns></returns>
        public static async Task<List<Branch>> GetBranchesAsync(List<Node> nodes)
        {
            Console.WriteLine(">>> Start branches creating");
            var branches = await Task.Run(() => RastrSupplier.GetBranches(nodes));
            Console.WriteLine(">>> Branches created");
            return branches;
        }
    }
}

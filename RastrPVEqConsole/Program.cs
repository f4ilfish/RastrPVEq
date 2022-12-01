using RastrPVEqConsole.Infrastructure;
using RastrPVEqConsole.Models;
using ASTRALib;
using System.Diagnostics;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static async Task Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\до эквивалента.rg2",
                                             "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            Console.WriteLine(">>> File downloaded without exceptions");

            var nodesTask = RastrSupplierAsync.GetNodesAsync();
            await nodesTask;

            var nodes = nodesTask.Result;
            
            var branchesTask = RastrSupplierAsync.GetBranchesAsync(nodes);
            var rangesTask = RastrSupplierAsync.GetAdjustmentRangesAsync();
            await rangesTask;
            
            var ranges = rangesTask.Result;
            var pqDiagrams = RastrSupplier.GetPQDiagrams(ranges);
            Console.WriteLine(">>> PQ diagrams сreated");

            var generatorsTask = RastrSupplierAsync.GetGeneratorsAsync(nodes, pqDiagrams);
            await Task.WhenAll(branchesTask, generatorsTask);
            
            var branches = branchesTask.Result;
            var generators = generatorsTask.Result;

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            Console.ReadKey();
        }
    }
}
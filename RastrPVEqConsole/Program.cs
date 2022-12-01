using RastrPVEqConsole.Infrastructure;
using System.Diagnostics;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static async Task Main()
        {
            RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\часть до эквивалента.rg2",
                                             "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            Console.WriteLine(">>> File downloaded without exceptions");

            Stopwatch stopwatch = new ();
            stopwatch.Start();

            var nodesTask = RastrSupplierAsync.GetNodesAsync();
            var pqDiagramsTask = RastrSupplierAsync.GetPQDiagramsAsync();
            
            await nodesTask;
            var nodes = nodesTask.Result;
            var branchesTask = RastrSupplierAsync.GetBranchesAsync(nodes);

            await pqDiagramsTask;
            var pqDiagrams = pqDiagramsTask.Result;
            var generatorsTask = RastrSupplierAsync.GetGeneratorsAsync(nodes, pqDiagrams);
                        
            await Task.WhenAll(branchesTask, generatorsTask);
            var branches = branchesTask.Result;
            var generators = generatorsTask.Result;

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
using RastrPVEqConsole.Infrastructure.RastrWin3;
using RastrPVEqConsole.Infrastructure.Topology;
using RastrPVEqConsole.Models.Topology;
using RastrPVEqConsole.Models.RastrWin3;
using System.Diagnostics;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static async Task Main()
        {
            //RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\до эквивалента.rg2",
            //                                 "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            //Console.WriteLine(">>> File downloaded without exceptions");

            #region Async
            //Stopwatch stopwatch = new();
            //stopwatch.Start();

            //var nodesTask = RastrSupplierAsync.GetNodesAsync();
            //var pqDiagramsTask = RastrSupplierAsync.GetPQDiagramsAsync();

            //await nodesTask;
            //var nodes = nodesTask.Result;
            //var branchesTask = RastrSupplierAsync.GetBranchesAsync(nodes);

            //await pqDiagramsTask;
            //var pqDiagrams = pqDiagramsTask.Result;
            //var generatorsTask = RastrSupplierAsync.GetGeneratorsAsync(nodes, pqDiagrams);

            //await Task.WhenAll(branchesTask, generatorsTask);
            //var branches = branchesTask.Result;
            //var generators = generatorsTask.Result;

            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            #endregion

            #region Sync
            //Stopwatch stopwatch = new();
            //stopwatch.Start();

            //var nodes = RastrSupplier.GetNodes();
            //Console.WriteLine("Node downloaded");
            //var pqDiagramms = RastrSupplier.GetPQDiagrams();
            //Console.WriteLine("PQ diagram downloaded");
            //var branches = RastrSupplier.GetBranches(nodes);
            //Console.WriteLine("Branches downloaded");
            //var generators = RastrSupplier.GetGenerators(nodes, pqDiagramms);
            //Console.WriteLine("Generators downloaded");

            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //Console.ReadKey();
            #endregion

            var aNode = new Node(1, ElementStatus.Enable, 1, "A", 10);
            var bNode = new Node(2, ElementStatus.Enable, 2, "B", 10);
            var cNode = new Node(3, ElementStatus.Enable, 3, "C", 10);
            var dNode = new Node(4, ElementStatus.Enable, 4, "D", 10);
            var eNode = new Node(5, ElementStatus.Enable, 5, "E", 10);
            var fNode = new Node(6, ElementStatus.Enable, 6, "F", 10);

            var abBranch = new Branch(1, ElementStatus.Enable, BranchType.Line, "AB", 1, 0, 0);
            var bcBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "BC", 1, 0, 0);
            var bdBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "BD", 1, 0, 0);
            var deBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "DE", 1, 0, 0);
            var cfBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "CF", 1, 0, 0);

            IGraph<Node> graph = new Graph<Node>();

            var a = graph.AddVertex(aNode);
            var b = graph.AddVertex(bNode);
            var c = graph.AddVertex(cNode);
            var d = graph.AddVertex(dNode);
            var e = graph.AddVertex(eNode);
            var f = graph.AddVertex(fNode);

            graph.AddEdge(a, b, abBranch.Resistance);
            graph.AddEdge(b, a, abBranch.Resistance);

            graph.AddEdge(b, d, bdBranch.Resistance);
            graph.AddEdge(d, b, bdBranch.Resistance);

            graph.AddEdge(d, e, deBranch.Resistance);
            graph.AddEdge(e, d, deBranch.Resistance);

            graph.AddEdge(b, c, bcBranch.Resistance);
            graph.AddEdge(c, b, bcBranch.Resistance);

            graph.AddEdge(c, f, cfBranch.Resistance);
            graph.AddEdge(f, c, cfBranch.Resistance);

            //Console.WriteLine("Depth-First iterative path between two vertices: ");
            //var depthFirstIterative = DepthFirstTraversal<Node>.DepthFirstIterative(graph, a, e);

            //foreach (var vertex in depthFirstIterative)
            //{
            //    Console.WriteLine(vertex.Data.Name);
            //}

            //Console.WriteLine($"Is acyclic: {graph.IsAcyclic()}");

            //Console.WriteLine($"Has adjacent: {graph.AreAdjacent(b, a)}");

            List<DistanceModel<Node>> shortestPathTable = DijkstraAlgorithm<Node>.DijkstraMethod(graph, a);

            foreach (var path in shortestPathTable)
            {
                Console.WriteLine(path);
            }
        }
    }
}
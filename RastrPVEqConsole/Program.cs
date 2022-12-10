using RastrPVEqConsole.Infrastructure;
using RastrPVEqConsole.Infrastructure.Topology;
using RastrPVEqConsole.Models.Topology;
using RastrPVEqConsole.Models.RastrWin3;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static async Task Main()
        {
            //RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\часть до эквивалента.rg2",
            //                                 "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            //Console.WriteLine(">>> File downloaded without exceptions");

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

            var aNode = new Node(1, ElementStatus.Enable, 1, "A", 10);
            var bNode = new Node(2, ElementStatus.Enable, 2, "B", 10);
            var cNode = new Node(3, ElementStatus.Enable, 3, "C", 10);
            var dNode = new Node(4, ElementStatus.Enable, 4, "D", 10);
            var eNode = new Node(5, ElementStatus.Enable, 5, "E", 10);

            var abBranch = new Branch(1, ElementStatus.Enable, BranchType.Line, "AB", 6, 0, 0);
            var adBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "AD", 1, 0, 0);
            var deBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "DE", 1, 0, 0);
            var dbBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "DB", 2, 0, 0);
            var ebBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "EB", 2, 0, 0);
            var ecBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "EC", 5, 0, 0);
            var cbBranch = new Branch(2, ElementStatus.Enable, BranchType.Line, "CB", 5, 0, 0);

            IGraph<Node> graph = new Graph<Node>();

            var a = graph.AddVertex(aNode);
            var b = graph.AddVertex(bNode);
            var c = graph.AddVertex(cNode);
            var d = graph.AddVertex(dNode);
            var e = graph.AddVertex(eNode);

            //graph.AddEdge(a, b, abBranch.Resistance);
            graph.AddEdge(b, a, abBranch.Resistance);

            graph.AddEdge(a, d, adBranch.Resistance);
            //graph.AddEdge(d, a, adBranch.Resistance);

            graph.AddEdge(d, e, deBranch.Resistance);
            //graph.AddEdge(e, d, deBranch.Resistance);

            graph.AddEdge(d, b, dbBranch.Resistance);
            //graph.AddEdge(b, d, dbBranch.Resistance);

            graph.AddEdge(e, b, ebBranch.Resistance);
            //graph.AddEdge(b, e, ebBranch.Resistance);

            graph.AddEdge(e, c, ecBranch.Resistance);
            //graph.AddEdge(c, e, ecBranch.Resistance);

            graph.AddEdge(c, b, cbBranch.Resistance);
            //graph.AddEdge(b, c, cbBranch.Resistance);

            Console.WriteLine("Depth-First iterative path between two vertices: ");
            var depthFirstIterative = DepthFirstTraversal<Node>.DepthFirstIterative(graph, b, c);

            foreach (var vertex in depthFirstIterative)
            {
                Console.WriteLine(vertex.Data.Name);
            }

            Console.WriteLine($"Is acyclic: {graph.IsAcyclic()}");

            Console.WriteLine($"Has adjacent: {graph.AreAdjacent(b, a)}");

            //var shortestPathTable = DijkstraAlgorithm<Node>.DijkstraMethod(graph, a);

            //foreach (var path in shortestPathTable)
            //{
            //    Console.WriteLine(path);
            //}
        }
    }
}
using RastrPVEqConsole.Infrastructure;
using RastrPVEqConsole.Models;
using ASTRALib;
using System.Diagnostics;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\до эквивалента.rg2",
                                             "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            Console.WriteLine(">>> File downloaded without exceptions");

            //var tmpNode = RastrSupplier.GetNodeByIndex(0);
            //Console.WriteLine(">>> Sample node created");

            //var tmpBranch = RastrSupplier.GetBranchByIndex(0);
            //Console.WriteLine(">>> Sample branch created");

            //var tmpAdjustmentRange = RastrSupplier.GetAdjustmentRangeByIndex(0);
            //Console.WriteLine(">>> Sample adjustment range created");

            var node = RastrSupplier.GetNodes();

            Console.WriteLine(">>> List of nodes created");

            var ranges = RastrSupplier.GetAdjustmentRanges();

            Console.WriteLine(">>> List of adjustment ranges created");

            //var listPQDiagrams = from range in ranges
            //                     group range by range.DiagramNumber into groupRanges
            //                     select new { diagramNumber = groupRanges.Key, diagramRanges = groupRanges.Count() };

            //Console.WriteLine(">>> Adjustment ranges grouped");

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);

            Console.ReadKey();
        }


       
    }
}
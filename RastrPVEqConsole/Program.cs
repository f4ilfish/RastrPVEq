using RastrPVEqConsole.Infrastructure;
using RastrPVEqConsole.Models;
using ASTRALib;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static void Main()
        {
            RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\до эквивалента.rg2",
                                             "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");
            Console.WriteLine(">>> File downloaded without exceptions");

            //var tmpNode = RastrSupplier.GetNodeByIndex(0);
            //Console.WriteLine(">>> Sample node created");

            //var tmpBranch = RastrSupplier.GetBranchByIndex(0);
            //Console.WriteLine(">>> Sample branch created");

            //var tmpAdjustmentRange = RastrSupplier.GetAdjustmentRangeByIndex(0);
            //Console.WriteLine(">>> Sample adjustment range created");

            Console.ReadKey();
        }


       
    }
}
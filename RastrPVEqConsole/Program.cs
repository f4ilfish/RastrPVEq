using RastrPVEqConsole.Infrastructure;
using RastrPVEqConsole.Models;

namespace RastrPVEqConsole
{
    internal class Program
    {
        static void Main()
        {
            RastrSupplier.LoadFileByTemplate("C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\до эквивалента.rg2",
                                             "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2");

            Console.WriteLine("File downloaded without exceptions");

            var tmp = RastrSupplier.GetElementParameterValue("node", "tip", 773);

            //Console.WriteLine($"Item param: {tmp}");

            Console.ReadKey();
        }


        //public static class RastrSupplier
        //{
        //    /// <summary>
        //    /// Rastr com-object filed
        //    /// </summary>
        //    private static readonly Rastr _rastr = new();

        //    /// <summary>
        //    /// Rastr com-object property
        //    /// </summary>
        //    public static Rastr Rastr { get => _rastr; }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="filePath"></param>
        //    /// <param name="templatePath"></param>
        //    public static void LoadFile (string filePath, string templatePath)
        //    {
        //        Rastr.Load(RG_KOD.RG_REPL, filePath, templatePath);
        //    }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="tableName"></param>
        //    /// <param name="columnName"></param>
        //    /// <param name="number"></param>
        //    /// <returns></returns>
        //    /// <exception cref="ArgumentException"></exception>
        //    /// <exception cref="Exception"></exception>
        //    public static int GetIndexByNumber(string tableName, string columnName, int number)
        //    {
        //        if (Rastr.Tables.Find[tableName] == -1) throw new ArgumentException($"Regime template isn't contain {tableName} table");
                
        //        ITable table = Rastr.Tables.Item(tableName);

        //        if (table.Cols.Find[columnName] == -1) throw new ArgumentException($"Table {tableName} isn't contain {columnName}");

        //        ICol columnItem = table.Cols.Item(columnName);

        //        for (int index = 0; index < table.Count; index++)
        //        {
        //            if (columnItem.get_ZN(index) == number)
        //            {
        //                return index;
        //            }
        //        }

        //        throw new Exception();
        //    }
        //}
    }
}
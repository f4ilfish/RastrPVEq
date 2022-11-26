using ASTRALib;
using RastrPVEqConsole.Models;

namespace RastrPVEqConsole.Infrastructure
{
    public static class RastrSupplier
    {
        /// <summary>
        /// Rastr com-object filed
        /// </summary>
        private static readonly Rastr _rastr = new Rastr();

        /// <summary>
        /// Rastr com-object property
        /// </summary>
        public static Rastr Rastr { get => _rastr; }

        /// <summary>
        /// Load regime file
        /// </summary>
        /// <param name="loadFilePath">Path to regime file</param>
        /// <param name="templateFilePath">Path to regime template</param>
        public static void LoadFileByTemplate(string loadFilePath, string templateFilePath)
        {
            CheckFileExistance(loadFilePath);
            CheckFileExistance(templateFilePath);

            string templateFileFormat = Path.GetExtension(templateFilePath);
            
            CheckFileFormat(loadFilePath, templateFileFormat);

            try
            {
                Rastr.Load(RG_KOD.RG_REPL, loadFilePath, templateFilePath);
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Load file or template file is damaged", ex);
            }
        }

        /// <summary>
        /// Save rastr file
        /// </summary>
        /// <param name="saveFilePath">Regime file name</param>
        /// <param name="templateFilePath">Path to regime template</param>
        public static void SaveFileByTemplate(string saveFilePath, string templateFilePath)
        {
            CheckFileExistance(templateFilePath);

            string templateFileFormat = Path.GetExtension(templateFilePath);

            CheckFileFormat(saveFilePath, templateFileFormat);

            try
            {
                Rastr.Save(saveFilePath, templateFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Template file is damaged", ex);
            }
        }

        /// <summary>
        /// Check file existance from path
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private static void CheckFileExistance(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path doesn't inputed");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File on {filePath} path doesn't exist");
        }

        /// <summary>
        /// Check file format from path
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="fileFormat">File format</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="FileLoadException"></exception>
        private static void CheckFileFormat(string filePath, string fileFormat)
        {
            if (Path.GetExtension(filePath) != fileFormat)
                throw new FileLoadException($"File on {filePath} must be in *{fileFormat} format");
        }

        /// <summary>
        /// Get element parameter's value
        /// </summary>
        /// <param name="tableName">Rastr file's table name</param>
        /// <param name="columnName">Table column's name corresponds element's parameter</param>
        /// <param name="elementIndex">Element's index in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static object GetElementParameterValue(string tableName, string columnName, int elementIndex)
        {
            if (Rastr.Tables.Find[tableName] == -1)
                throw new ArgumentException($"Rastr loaded files doesn't contain {tableName}");

            ITable table = Rastr.Tables.Item(tableName);

            if (table.Cols.Find[columnName] == -1)
                throw new ArgumentException($"Table {tableName} doesn't contain {columnName}");

            ICol elementParameter = table.Cols.Item(columnName);

            var numberOfElements = table.Count;

            if (elementIndex >= numberOfElements)
                throw new ArgumentException($"Table {tableName} doesn't contain element with index {elementIndex}. " +
                    $"Table contain {numberOfElements} elements. Max index of element {numberOfElements - 1}");

            return elementParameter.get_ZN(elementIndex);
        }

        private const string nodeTable = "node";
        private const string branchTable = "vetv";
        private const string adjustmentRangeTable = "graphik2";

        private const string elementStatusColumn = "sta";
        private const string elementNameColumn = "name";

        private const string nodeNumberColumn = "ny";
        private const string nodeRatedVoltageColumn = "uhom";

        private const string branchTypeColumn = "tip";
        private const string branchResistanceColumn = "r";
        private const string branchInductanceColumn = "x";
        private const string branchCapacitanceColumn = "b";
        private const string branchTranformerRatioColumn = "ktr";

        private const string adjustmentRangeNumberColumn = "Num";
        private const string adjustmentRangeActivePowerColumn = "P";
        private const string adjustmentRangeMinimumReactivePowerColumn = "Qmin";
        private const string adjustmentRangeMaximumReactivePowerColumn = "Qmax";

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="elementIndex">Index of element in table</param>
        /// <returns></returns>
        public static Node GetNodeByIndex(int elementIndex)
        {
            var elementStatusValue = !(bool)GetElementParameterValue(nodeTable, elementStatusColumn, elementIndex) ? ElementStatus.Enable : ElementStatus.Disable;
            var nodeNumberValue = (int)GetElementParameterValue(nodeTable, nodeNumberColumn, elementIndex);
            var nodeNameValue = (string)GetElementParameterValue(nodeTable, elementNameColumn, elementIndex);
            var nodeRatedVoltageValue = (double)GetElementParameterValue(nodeTable, nodeRatedVoltageColumn, elementIndex);

            return new Node(elementIndex, elementStatusValue, nodeNumberValue, nodeNameValue, nodeRatedVoltageValue);
        }

        /// <summary>
        /// Get branch
        /// </summary>
        /// <param name="elementIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Branch GetBranchByIndex(int elementIndex)
        {
            var elementStatusValue = (int)GetElementParameterValue(branchTable, elementStatusColumn, elementIndex) == 0 ? ElementStatus.Enable : ElementStatus.Disable;
            var branchTypeValue = (int)GetElementParameterValue(branchTable, branchTypeColumn, elementIndex) switch
            {
                0 => BranchType.Line,
                1 => BranchType.Transformer,
                2 => BranchType.Switch,
                _ => throw new ArgumentException("Unknown int branch type"),
            };
            var branchNameValue = (string)GetElementParameterValue(branchTable, elementNameColumn, elementIndex);
            var branchResistanceValue = (double)GetElementParameterValue(branchTable, branchResistanceColumn, elementIndex);
            var branchInductanceValue = (double)GetElementParameterValue(branchTable, branchInductanceColumn, elementIndex);
            var branchCapacitanceValue = (double)GetElementParameterValue(branchTable, branchCapacitanceColumn, elementIndex);
            var branchTranformerRatioValue = (double)GetElementParameterValue(branchTable, branchTranformerRatioColumn, elementIndex);

            return new Branch(elementIndex, 
                              elementStatusValue, 
                              branchTypeValue, 
                              branchNameValue, 
                              branchResistanceValue, 
                              branchInductanceValue, 
                              branchCapacitanceValue, 
                              branchTranformerRatioValue);
        }

        /// <summary>
        /// Get adjustment range
        /// </summary>
        /// <param name="elementIndex"></param>
        /// <returns></returns>
        private static AdjustmentRange GetAdjustmentRangeByIndex(int elementIndex)
        {
            var adjustmentRangeNumberValue = (int)GetElementParameterValue(adjustmentRangeTable, adjustmentRangeNumberColumn, elementIndex);
            var adjustmentRangeActivePowerValue = (double)GetElementParameterValue(adjustmentRangeTable, adjustmentRangeActivePowerColumn, elementIndex);
            var adjustmentRangeMinimumReactivePowerValue = (double)GetElementParameterValue(adjustmentRangeTable, adjustmentRangeMinimumReactivePowerColumn, elementIndex);
            var adjustmentRangeMaximumReactivePowerValue = (double)GetElementParameterValue(adjustmentRangeTable, adjustmentRangeMaximumReactivePowerColumn, elementIndex);

            return new AdjustmentRange(elementIndex, 
                                       adjustmentRangeNumberValue, 
                                       adjustmentRangeActivePowerValue, 
                                       adjustmentRangeMinimumReactivePowerValue, 
                                       adjustmentRangeMaximumReactivePowerValue);
        }
    }
}

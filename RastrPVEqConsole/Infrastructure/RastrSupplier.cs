using ASTRALib;
using RastrPVEqConsole.Models;

namespace RastrPVEqConsole.Infrastructure
{
    /// <summary>
    /// Rastr supply method provider class
    /// </summary>
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
        /// <param name="filePath">Path to file check</param>
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
        private static object GetElementParameterValue(string tableName, string columnName, int elementIndex)
        {
            //if (Rastr.Tables.Find[tableName] == -1)
            //    throw new ArgumentException($"Rastr loaded files doesn't contain {tableName}");

            ITable table = Rastr.Tables.Item(tableName);

            //if (table.Cols.Find[columnName] == -1)
            //    throw new ArgumentException($"Table {tableName} doesn't contain {columnName}");

            ICol elementParameter = table.Cols.Item(columnName);

            var numberOfElements = table.Count;

            //if (elementIndex >= numberOfElements)
            //    throw new ArgumentException($"Table {tableName} doesn't contain element with index {elementIndex}. " +
            //        $"Table contain {numberOfElements} elements. Max index of element {numberOfElements - 1}");

            return elementParameter.get_ZN(elementIndex);
        }

        private const string NodeTable = "node";
        private const string BranchTable = "vetv";
        private const string AdjustmentRangeTable = "graphik2";
        private const string GeneratorTable = "Generator";

        private const string ElementStatusColumn = "sta";
        private const string ElementNameColumn = "name";
        private const string NumElementNumberColumn = "Num";

        private const string NodeNumberColumn = "ny";
        private const string NodeRatedVoltageColumn = "uhom";

        private const string BranchTypeColumn = "tip";
        private const string BranchStartNodeColumn = "ip";
        private const string BranchEndNodeColumn = "iq";
        private const string BranchResistanceColumn = "r";
        private const string BranchInductanceColumn = "x";
        private const string BranchCapacitanceColumn = "b";
        private const string BranchTranformerRatioColumn = "ktr";

        
        private const string AdjustmentRangeActivePowerColumn = "P";
        private const string AdjustmentRangeMinimumReactivePowerColumn = "Qmin";
        private const string AdjustmentRangeMaximumReactivePowerColumn = "Qmax";

        private const string GeneratorNodeNumberColumn = "Node";
        private const string GeneratorNameColumn = "Name";
        private const string GeneratorMaxActivePowerColumn = "Pmax";
        private const string GeneratorNumPQDiagramColumn = "NumPQ";

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="elementIndex">Index of element in table</param>
        /// <returns></returns>
        private static Node GetNodeByIndex(int elementIndex)
        {
            var elementStatusValue = !(bool)GetElementParameterValue(NodeTable, ElementStatusColumn, elementIndex) ? ElementStatus.Enable : ElementStatus.Disable;
            var nodeNumberValue = (int)GetElementParameterValue(NodeTable, NodeNumberColumn, elementIndex);
            var nodeNameValue = (string)GetElementParameterValue(NodeTable, ElementNameColumn, elementIndex);
            var nodeRatedVoltageValue = (double)GetElementParameterValue(NodeTable, NodeRatedVoltageColumn, elementIndex);

            return new Node(elementIndex, elementStatusValue, nodeNumberValue, nodeNameValue, nodeRatedVoltageValue);
        }

        /// <summary>
        /// Get branch
        /// </summary>
        /// <param name="elementIndex">Element's index in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Branch GetBranchByIndex(int elementIndex)
        {
            var elementStatusValue = (int)GetElementParameterValue(BranchTable, ElementStatusColumn, elementIndex) == 0 ? ElementStatus.Enable : ElementStatus.Disable;
            var branchTypeValue = (int)GetElementParameterValue(BranchTable, BranchTypeColumn, elementIndex) switch
            {
                0 => BranchType.Line,
                1 => BranchType.Transformer,
                2 => BranchType.Switch,
                _ => throw new ArgumentException("Unknown int branch type"),
            };
            var branchNameValue = (string)GetElementParameterValue(BranchTable, ElementNameColumn, elementIndex);
            var branchResistanceValue = (double)GetElementParameterValue(BranchTable, BranchResistanceColumn, elementIndex);
            var branchInductanceValue = (double)GetElementParameterValue(BranchTable, BranchInductanceColumn, elementIndex);
            var branchCapacitanceValue = (double)GetElementParameterValue(BranchTable, BranchCapacitanceColumn, elementIndex);
            var branchTranformerRatioValue = (double)GetElementParameterValue(BranchTable, BranchTranformerRatioColumn, elementIndex);

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
        /// <param name="elementIndex">Element's index in table</param>
        /// <returns></returns>
        private static AdjustmentRange GetAdjustmentRangeByIndex(int elementIndex)
        {
            var adjustmentRangeNumberValue = (int)GetElementParameterValue(AdjustmentRangeTable, NumElementNumberColumn, elementIndex);
            var adjustmentRangeActivePowerValue = (double)GetElementParameterValue(AdjustmentRangeTable, AdjustmentRangeActivePowerColumn, elementIndex);
            var adjustmentRangeMinimumReactivePowerValue = (double)GetElementParameterValue(AdjustmentRangeTable, AdjustmentRangeMinimumReactivePowerColumn, elementIndex);
            var adjustmentRangeMaximumReactivePowerValue = (double)GetElementParameterValue(AdjustmentRangeTable, AdjustmentRangeMaximumReactivePowerColumn, elementIndex);

            return new AdjustmentRange(elementIndex, 
                                       adjustmentRangeNumberValue, 
                                       adjustmentRangeActivePowerValue, 
                                       adjustmentRangeMinimumReactivePowerValue, 
                                       adjustmentRangeMaximumReactivePowerValue);
        }

        /// <summary>
        /// Get generator
        /// </summary>
        /// <param name="elementIndex">Element's index in table</param>
        /// <returns></returns>
        private static Generator GetGeneratorByIndex(int elementIndex)
        {
            var elementStatusValue = !(bool)GetElementParameterValue(GeneratorTable, ElementStatusColumn, elementIndex) ? ElementStatus.Enable : ElementStatus.Disable;
            var generatorNumberValue = (int)GetElementParameterValue(GeneratorTable, NumElementNumberColumn, elementIndex);
            var generatorNameValue = (string)GetElementParameterValue(GeneratorTable, GeneratorNameColumn, elementIndex);
            var maxActivePowerValue = (double)GetElementParameterValue(GeneratorTable, GeneratorMaxActivePowerColumn, elementIndex);

            return new Generator(elementIndex,
                                 elementStatusValue, 
                                 generatorNumberValue, 
                                 generatorNameValue, 
                                 maxActivePowerValue);
        }

        /// <summary>
        /// Get nodes
        /// </summary>
        /// <returns></returns>
        public static List<Node> GetNodes()
        {
            var nodes = new List<Node>();

            var numberOfElements = Rastr.Tables.Item(NodeTable).Count;

            for (int i = 0; i < numberOfElements; i++)
            {
                nodes.Add(GetNodeByIndex(i));
            }

            return nodes;
        }

        /// <summary>
        /// Get adjustment ranges
        /// </summary>
        /// <returns></returns>
        public static List<AdjustmentRange> GetAdjustmentRanges()
        {
            var adjustmentRanges = new List<AdjustmentRange>();

            var numberOfElements = Rastr.Tables.Item(AdjustmentRangeTable).Count;

            for (int i = 0; i < numberOfElements; i++)
            {
                adjustmentRanges.Add(GetAdjustmentRangeByIndex(i));
            }

            return adjustmentRanges;
        }

        /// <summary>
        /// Get PQdiagrams
        /// </summary>
        /// <param name="adjustmentRanges">List of adjustment ranges</param>
        /// <returns></returns>
        public static List<PQDiagram> GetPQDiagrams(List<AdjustmentRange> adjustmentRanges)
        {
            var pqDiagramsDict = adjustmentRanges.GroupBy(r => r.DiagramNumber)
                                   .ToDictionary(g => g.Key, g => g.ToList());

            var pqDiagrams = new List<PQDiagram>();

            foreach (var pqDiagram in pqDiagramsDict)
            {
                pqDiagrams.Add(new PQDiagram(pqDiagram.Key, pqDiagram.Value));
            }

            return pqDiagrams;
        }

        /// <summary>
        /// GetGenerators
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <param name="pqDiagrams">List of PQ diagrams</param>
        /// <returns></returns>
        public static List<Generator> GetGenerators(List<Node> nodes, List<PQDiagram> pqDiagrams)
        {
            var nodesDict = nodes.ToDictionary(n => n.NodeNumber, n => n);
            var pqDiagramsDict = pqDiagrams.ToDictionary(d => d.DiagramNumber, d => d);

            var generators = new List<Generator>();

            var numberOfElements = Rastr.Tables.Item(GeneratorTable).Count;

            for (int i = 0; i < numberOfElements; i++)
            {
                var generator = GetGeneratorByIndex(i);

                var generatorNodeNumber = (int)GetElementParameterValue(GeneratorTable, GeneratorNodeNumberColumn, i);
                var generatorPQDiagramNuber = (int)GetElementParameterValue(GeneratorTable, GeneratorNumPQDiagramColumn, i);

                if (nodesDict.ContainsKey(generatorNodeNumber))
                {
                    generator.Node = nodesDict[generatorNodeNumber];
                }

                if (pqDiagramsDict.ContainsKey(generatorPQDiagramNuber))
                {
                    generator.PQDiagram = pqDiagramsDict[generatorPQDiagramNuber];
                }

                generators.Add(generator);
            }

            return generators;
        }

        /// <summary>
        /// Get branches
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <returns></returns>
        public static List<Branch> GetBranches(List<Node> nodes)
        {
            var nodesDict = nodes.ToDictionary(n => n.NodeNumber, n => n);

            var branches = new List<Branch>();

            var numberOfElements = Rastr.Tables.Item(BranchTable).Count;

            for (int i = 0; i < numberOfElements; i++)
            {
                var branch = GetBranchByIndex(i);

                var branchStartNodeNumber = (int)GetElementParameterValue(BranchTable, BranchStartNodeColumn, i);
                var branchEndNodeNumber = (int)GetElementParameterValue(BranchTable, BranchEndNodeColumn, i);

                if (nodesDict.ContainsKey(branchStartNodeNumber))
                {
                    branch.StartNode = nodesDict[branchStartNodeNumber];
                }

                if (nodesDict.ContainsKey(branchEndNodeNumber))
                {
                    branch.EndNode = nodesDict[branchEndNodeNumber];
                }

                branches.Add(branch);
            }

            return branches;
        }
    }
}

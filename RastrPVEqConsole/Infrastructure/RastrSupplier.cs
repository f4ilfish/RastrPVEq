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
        /// Rastr com-object
        /// </summary>
        private static readonly Rastr _rastr = new ();

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
                _rastr.Load(RG_KOD.RG_REPL, loadFilePath, templateFilePath);
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
                _rastr.Save(saveFilePath, templateFilePath);
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
        /// Get element parameter value
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <param name="elementIndex">Element index in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static T GetElementParameterValue<T>(string tableName, string columnName, int elementIndex)
        {
            ITable table = _rastr.Tables.Item(tableName);
            ICol elementParameter = table.Cols.Item(columnName);
            return elementParameter.get_ZN(elementIndex);
        }

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="nodeIndex">Index of node in table</param>
        /// <returns></returns>
        private static Node GetNodeByIndex(int nodeIndex)
        {
            var nodeStatus = !GetElementParameterValue<bool>(RastrNames.NodeTable,
                                                             RastrNames.NodeStatusColumn, 
                                                             nodeIndex) ? ElementStatus.Enable 
                                                                           : ElementStatus.Disable;
            
            var nodeNumber = GetElementParameterValue<int>(RastrNames.NodeTable,
                                                           RastrNames.NodeNumberColumn, 
                                                           nodeIndex);
            
            var nodeName = GetElementParameterValue<string>(RastrNames.NodeTable,
                                                            RastrNames.NodeNameColumn, 
                                                            nodeIndex);
            
            var nodeRatedVoltage = GetElementParameterValue<double>(RastrNames.NodeTable,
                                                                    RastrNames.NodeRatedVoltageColumn, 
                                                                    nodeIndex);

            return new Node(nodeIndex, nodeStatus, nodeNumber, nodeName, nodeRatedVoltage);
        }

        /// <summary>
        /// Get nodes
        /// </summary>
        /// <returns></returns>
        public static List<Node> GetNodes()
        {
            var nodes = new List<Node>();

            var nodesCount = _rastr.Tables.Item(RastrNames.NodeTable).Count;

            for (int i = 0; i < nodesCount; i++)
            {
                nodes.Add(GetNodeByIndex(i));
            }

            return nodes;
        }

        /// <summary>
        /// Get branch
        /// </summary>
        /// <param name="branchIndex">Index of branch in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Branch GetBranchByIndex(int branchIndex)
        {
            var branchStatus = GetElementParameterValue<int>(RastrNames.BranchTable,
                                                             RastrNames.BranchStatusColumn, 
                                                             branchIndex) == 0 ? ElementStatus.Enable 
                                                                               : ElementStatus.Disable;
            
            var branchType = GetElementParameterValue<int>(RastrNames.BranchTable,
                                                           RastrNames.BranchTypeColumn, 
                                                           branchIndex) switch
            {
                0 => BranchType.Line,
                1 => BranchType.Transformer,
                2 => BranchType.Switch,
                _ => throw new ArgumentException("Unknown branch type"),
            };
            
            var branchName = GetElementParameterValue<string>(RastrNames.BranchTable, 
                                                              RastrNames.BranchNameColumn, 
                                                              branchIndex);
            
            var branchResistance = GetElementParameterValue<double>(RastrNames.BranchTable,
                                                                    RastrNames.BranchResistanceColumn, 
                                                                    branchIndex);
            
            var branchInductance = GetElementParameterValue<double>(RastrNames.BranchTable,
                                                                    RastrNames.BranchInductanceColumn, 
                                                                    branchIndex);
            
            var branchTranformationRatio = GetElementParameterValue<double>(RastrNames.BranchTable,
                                                                            RastrNames.BranchTranformationRatioColumn, 
                                                                            branchIndex);

            return new Branch(branchIndex, 
                              branchStatus, 
                              branchType, 
                              branchName, 
                              branchResistance, 
                              branchInductance, 
                              branchTranformationRatio);
        }

        /// <summary>
        /// Get branches
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <returns></returns>
        public static List<Branch> GetBranches(List<Node> nodes)
        {
            var nodesDict = nodes.ToDictionary(n => n.Number, n => n);

            var branches = new List<Branch>();

            var branchesCount = _rastr.Tables.Item(RastrNames.BranchTable).Count;

            for (int i = 0; i < branchesCount; i++)
            {
                var branch = GetBranchByIndex(i);

                var branchStartNodeNumber = GetElementParameterValue<int>(RastrNames.BranchTable,
                                                                          RastrNames.BranchStartNodeColumn, 
                                                                          i);
                
                var branchEndNodeNumber = GetElementParameterValue<int>(RastrNames.BranchTable,
                                                                        RastrNames.BranchEndNodeColumn, 
                                                                        i);

                if (nodesDict.ContainsKey(branchStartNodeNumber))
                {
                    branch.BranchStartNode = nodesDict[branchStartNodeNumber];
                }

                if (nodesDict.ContainsKey(branchEndNodeNumber))
                {
                    branch.BranchEndNode = nodesDict[branchEndNodeNumber];
                }

                branches.Add(branch);
            }

            return branches;
        }

        /// <summary>
        /// Get adjustment range
        /// </summary>
        /// <param name="adjustmentRangeIndex">Index of adjustment range in table</param>
        /// <returns></returns>
        private static AdjustmentRange GetAdjustmentRangeByIndex(int adjustmentRangeIndex)
        {
            var rangeNumber = GetElementParameterValue<int>(RastrNames.AdjustmentRangeTable,
                                                            RastrNames.AdjustmentRangeNumberColumn, 
                                                            adjustmentRangeIndex);
            
            var rangeActivePower = GetElementParameterValue<double>(RastrNames.AdjustmentRangeTable,
                                                                    RastrNames.AdjustmentRangeActivePowerColumn,
                                                                    adjustmentRangeIndex);
            
            var rangeMinimumReactivePower = GetElementParameterValue<double>(RastrNames.AdjustmentRangeTable,
                                                                             RastrNames.AdjustmentRangeMinimumReactivePowerColumn, 
                                                                             adjustmentRangeIndex);
            
            var rangeMaximumReactivePower = GetElementParameterValue<double>(RastrNames.AdjustmentRangeTable,
                                                                             RastrNames.AdjustmentRangeMaximumReactivePowerColumn, 
                                                                             adjustmentRangeIndex);

            return new AdjustmentRange(adjustmentRangeIndex, 
                                       rangeNumber, 
                                       rangeActivePower, 
                                       rangeMinimumReactivePower, 
                                       rangeMaximumReactivePower);
        }

        /// <summary>
        /// Get adjustment ranges
        /// </summary>
        /// <returns></returns>
        private static List<AdjustmentRange> GetAdjustmentRanges()
        {
            var adjustmentRanges = new List<AdjustmentRange>();

            var rangesCount = _rastr.Tables.Item(RastrNames.AdjustmentRangeTable).Count;

            for (int i = 0; i < rangesCount; i++)
            {
                adjustmentRanges.Add(GetAdjustmentRangeByIndex(i));
            }

            return adjustmentRanges;
        }

        /// <summary>
        /// Get PQ diagrams
        /// </summary>
        /// <returns></returns>
        public static List<PQDiagram> GetPQDiagrams()
        {
            var adjustmentRanges = GetAdjustmentRanges();

            var pqDiagramsDict = adjustmentRanges.GroupBy(r => r.PQDiagramNumber)
                                   .ToDictionary(g => g.Key, g => g.ToList());

            var pqDiagrams = new List<PQDiagram>();

            foreach (var pqDiagram in pqDiagramsDict)
            {
                var pqDiagramNumber = pqDiagram.Key;
                var ranges = pqDiagram.Value;
                pqDiagrams.Add(new PQDiagram(pqDiagramNumber, ranges));
            }

            return pqDiagrams;
        }

        /// <summary>
        /// Get generator
        /// </summary>
        /// <param name="generatorIndex">Index of generator in table</param>
        /// <returns></returns>
        private static Generator GetGeneratorByIndex(int generatorIndex)
        {
            var generatorStatus = !GetElementParameterValue<bool>(RastrNames.GeneratorTable,
                                                                  RastrNames.GeneratorStatusColumn, 
                                                                  generatorIndex) ? ElementStatus.Enable 
                                                                                : ElementStatus.Disable;
            
            var generatorNumber = GetElementParameterValue<int>(RastrNames.GeneratorTable,
                                                                RastrNames.GeneratorNumberColumn, 
                                                                generatorIndex);
            
            var generatorName = GetElementParameterValue<string>(RastrNames.GeneratorTable,
                                                                 RastrNames.GeneratorNameColumn, 
                                                                 generatorIndex);
            
            var generatorMaxActivePower = GetElementParameterValue<double>(RastrNames.GeneratorTable,
                                                                           RastrNames.GeneratorMaxActivePowerColumn, 
                                                                           generatorIndex);

            return new Generator(generatorIndex,
                                 generatorStatus, 
                                 generatorNumber, 
                                 generatorName, 
                                 generatorMaxActivePower);
        }

        /// <summary>
        /// Get generators
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        /// <param name="pqDiagrams">List of PQ diagrams</param>
        /// <returns></returns>
        public static List<Generator> GetGenerators(List<Node> nodes, List<PQDiagram> pqDiagrams)
        {
            var nodesDict = nodes.ToDictionary(n => n.Number, n => n);
            var pqDiagramsDict = pqDiagrams.ToDictionary(d => d.Number, d => d);

            var generators = new List<Generator>();

            var generatorsCount = _rastr.Tables.Item(RastrNames.GeneratorTable).Count;

            for (int i = 0; i < generatorsCount; i++)
            {
                var generator = GetGeneratorByIndex(i);

                var generatorNodeNumber = GetElementParameterValue<int>(RastrNames.GeneratorTable,
                                                                        RastrNames.GeneratorNodeNumberColumn, 
                                                                        i);
                
                var generatorPQDiagramNuber = GetElementParameterValue<int>(RastrNames.GeneratorTable,
                                                                            RastrNames.GeneratorPQDiagramNumberColumn, 
                                                                            i);

                if (nodesDict.ContainsKey(generatorNodeNumber))
                {
                    generator.GeneratorNode = nodesDict[generatorNodeNumber];
                }

                if (pqDiagramsDict.ContainsKey(generatorPQDiagramNuber))
                {
                    generator.GeneratorPQDiagram = pqDiagramsDict[generatorPQDiagramNuber];
                }

                generators.Add(generator);
            }

            return generators;
        }
    }
}

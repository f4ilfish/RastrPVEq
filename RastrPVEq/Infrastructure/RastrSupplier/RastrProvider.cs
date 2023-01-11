using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASTRALib;
using RastrPVEq.Models.PowerSystem;

namespace RastrPVEq.Infrastructure.RastrSupplier
{
    /// <summary>
    /// Rastr supplier class
    /// </summary>
    public static class RastrSupplier
    {
        /// <summary>
        /// Rastr com-object field
        /// </summary>
        private static readonly Rastr _rastr = new();

        /// <summary>
        /// Load file by template
        /// </summary>
        /// <param name="loadFilePath">Path to regime file</param>
        /// <param name="templateFilePath">Path to regime template</param>
        public static void LoadFileByTemplate(string loadFilePath, string templateFilePath)
        {
            CheckFileExistence(loadFilePath);
            CheckFileExistence(templateFilePath);

            var templateFileFormat = Path.GetExtension(templateFilePath);

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
        /// Save file by template
        /// </summary>
        /// <param name="saveFilePath">Regime file name</param>
        /// <param name="templateFilePath">Path to regime template</param>
        public static void SaveFileByTemplate(string saveFilePath, string templateFilePath)
        {
            CheckFileExistence(templateFilePath);

            var templateFileFormat = Path.GetExtension(templateFilePath);

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
        /// Check file existence from path
        /// </summary>
        /// <param name="filePath">Path to file check</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private static void CheckFileExistence(string filePath)
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
        /// <param name="table">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <param name="elementIndex">Element index in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static T GetElementParameterValue<T>(ITable table, string columnName, int elementIndex)
        {
            ICol elementParameter = table.Cols.Item(columnName);
            return elementParameter.get_ZN(elementIndex);
        }

        /// <summary>
        /// Set element parameter value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="elementIndex"></param>
        /// <param name="parameterValue"></param>
        private static void SetElementParameterValue<T>(ITable table, string columnName, int elementIndex, T parameterValue)
        {
            ICol elementParameter = table.Cols.Item(columnName);
            elementParameter.set_ZN(elementIndex, parameterValue);
        }

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="nodeIndex">Index of node in table</param>
        /// <param name="nodesTable"></param>
        /// <returns></returns>
        private static Node GetNodeByIndex(int nodeIndex, ITable nodesTable)
        {
            var nodeNumber = GetElementParameterValue<int>(nodesTable,
                                                           RastrConstantNames.NodeNumberColumn,
                                                           nodeIndex);

            var nodeName = GetElementParameterValue<string>(nodesTable,
                                                            RastrConstantNames.NodeNameColumn,
                                                            nodeIndex);

            var nodeRatedVoltage = GetElementParameterValue<double>(nodesTable,
                                                                    RastrConstantNames.NodeRatedVoltageColumn,
                                                                    nodeIndex);

            var nodeDistrictNumber = GetElementParameterValue<int>(nodesTable,
                                                                   RastrConstantNames.NodeDistrictNumberColumn,
                                                                   nodeIndex);

            var nodeTerritoryNumber = GetElementParameterValue<int>(nodesTable,
                                                                    RastrConstantNames.NodeTerritoryNumberColumn,
                                                                    nodeIndex);

            return new Node(nodeIndex, nodeNumber, nodeName, nodeRatedVoltage, nodeDistrictNumber, nodeTerritoryNumber);
        }

        /// <summary>
        /// Get nodes
        /// </summary>
        /// <returns></returns>
        public static List<Node> GetNodes()
        {
            var nodes = new List<Node>();

            ITable nodesTable = _rastr.Tables.Item(RastrConstantNames.NodeTable);

            for (int i = 0; i < GetNodesCount(); i++)
            {
                nodes.Add(GetNodeByIndex(i, nodesTable));
            }

            return nodes;
        }

        /// <summary>
        /// Get nodes count
        /// </summary>
        /// <returns></returns>
        public static int GetNodesCount()
        {
            ITable nodesTable = _rastr.Tables.Item(RastrConstantNames.NodeTable);
            return nodesTable.Count;
        }

        /// <summary>
        /// Add node
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="nodeTable">Node table</param>
        private static void AddNode(Node node, ITable nodeTable)
        {
            nodeTable.AddRow();

            var addedRowIndex = nodeTable.Count - 1;

            SetElementParameterValue(nodeTable, RastrConstantNames.NodeNumberColumn, addedRowIndex, node.Number);
            SetElementParameterValue(nodeTable, RastrConstantNames.NodeNameColumn, addedRowIndex, node.Name);
            SetElementParameterValue(nodeTable, RastrConstantNames.NodeRatedVoltageColumn, addedRowIndex, node.RatedVoltage);
            SetElementParameterValue(nodeTable, RastrConstantNames.NodeDistrictNumberColumn, addedRowIndex, node.DistrictNumber);
            SetElementParameterValue(nodeTable, RastrConstantNames.NodeTerritoryNumberColumn, addedRowIndex, node.TerritoryNumber);
        }

        /// <summary>
        /// Add nodes
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        public static void AddNodes(List<Node> nodes)
        {
            ITable nodesTable = _rastr.Tables.Item(RastrConstantNames.NodeTable);

            foreach (var node in nodes)
            {
                AddNode(node, nodesTable);
            }
        }

        /// <summary>
        /// Delete nodes
        /// </summary>
        /// <param name="nodes"></param>
        public static void DeleteNodes(List<Node> nodes)
        {
            ITable nodesTable = _rastr.Tables.Item(RastrConstantNames.NodeTable);

            foreach(var node in nodes)
            {
                nodesTable.SetSel($"ny={node.Number}");

                nodesTable.DelRowS();
            }
        }

        /// <summary>
        /// Get branch
        /// </summary>
        /// <param name="branchIndex">Index of branch in table</param>
        /// <param name="branchesTable"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Branch GetBranchByIndex(int branchIndex, ITable branchesTable)
        {
            var branchType = GetElementParameterValue<int>(branchesTable,
                                                           RastrConstantNames.BranchTypeColumn,
                                                           branchIndex) == 0 ? BranchType.Line 
                                                                             : BranchType.Transformer;

            var branchName = GetElementParameterValue<string>(branchesTable,
                                                              RastrConstantNames.BranchNameColumn,
                                                              branchIndex);

            var branchResistance = GetElementParameterValue<double>(branchesTable,
                                                                    RastrConstantNames.BranchResistanceColumn,
                                                                    branchIndex);

            var branchInductance = GetElementParameterValue<double>(branchesTable,
                                                                    RastrConstantNames.BranchInductanceColumn,
                                                                    branchIndex);

            var branchCapacitance = GetElementParameterValue<double>(branchesTable,
                                                                     RastrConstantNames.BranchCapacitanceColumn,
                                                                     branchIndex);

            var branchTransformationRatio = GetElementParameterValue<double>(branchesTable,
                                                                            RastrConstantNames.BranchTransformationRatioColumn,
                                                                            branchIndex);

            var branchDistrictNumber = GetElementParameterValue<int>(branchesTable,
                                                                     RastrConstantNames.BranchDistrictNumberColumn,
                                                                     branchIndex);

            var branchTerritoryNumber = GetElementParameterValue<int>(branchesTable,
                                                                      RastrConstantNames.BranchTerritoryNumberColumn,
                                                                      branchIndex);

            var branchAdmissableCurrent = GetElementParameterValue<double>(branchesTable,
                                                                           RastrConstantNames.BranchAdmissableCurrentColumn,
                                                                           branchIndex);

            var branchAdmissableEquipmentCurrent = GetElementParameterValue<double>(branchesTable,
                                                                                    RastrConstantNames.BranchEquipmentAdmissableCurrentColumn,
                                                                                    branchIndex);

            return new Branch(branchIndex,
                              branchType,
                              branchName,
                              branchResistance,
                              branchInductance, 
                              branchCapacitance,
                              branchTransformationRatio,
                              branchDistrictNumber, 
                              branchTerritoryNumber, 
                              branchAdmissableCurrent, 
                              branchAdmissableEquipmentCurrent);
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

            ITable branchesTable = _rastr.Tables.Item(RastrConstantNames.BranchTable);

            for (var i = 0; i < GetBranchesCount(); i++)
            {
                var branch = GetBranchByIndex(i, branchesTable);

                var branchStartNodeNumber = GetElementParameterValue<int>(branchesTable,
                                                                          RastrConstantNames.BranchStartNodeColumn,
                                                                          i);

                var branchEndNodeNumber = GetElementParameterValue<int>(branchesTable,
                                                                        RastrConstantNames.BranchEndNodeColumn,
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
        /// Get branches count
        /// </summary>
        /// <returns></returns>
        public static int GetBranchesCount()
        {
            ITable branchesTable = _rastr.Tables.Item(RastrConstantNames.BranchTable);
            return branchesTable.Count;
        }

        /// <summary>
        /// Add branch
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branchTable"></param>
        private static void AddBranch(Branch branch, ITable branchTable)
        {
            branchTable.AddRow();

            var addedRowIndex = branchTable.Count - 1;

            SetElementParameterValue(branchTable, RastrConstantNames.BranchStartNodeColumn, addedRowIndex, branch.BranchStartNode.Number);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchEndNodeColumn, addedRowIndex, branch.BranchEndNode.Number);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchResistanceColumn, addedRowIndex, branch.Resistance);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchInductanceColumn, addedRowIndex, branch.Inductance);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchCapacitanceColumn, addedRowIndex, branch.Capacitance);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchTransformationRatioColumn, addedRowIndex, branch.TransformationRatio);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchDistrictNumberColumn, addedRowIndex, branch.DistrictNumber);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchTerritoryNumberColumn, addedRowIndex, branch.TerritoryNumber);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchAdmissableCurrentColumn, addedRowIndex, branch.AdmissableCurrent);
            SetElementParameterValue(branchTable, RastrConstantNames.BranchEquipmentAdmissableCurrentColumn, addedRowIndex, branch.EquipmentAdmissableCurrent);
        }

        /// <summary>
        /// Add branches
        /// </summary>
        /// <param name="branches"></param>
        public static void AddBranches(List<Branch> branches)
        {
            ITable branchesTable = _rastr.Tables.Item(RastrConstantNames.BranchTable);

            foreach (var branch in branches)
            {
                AddBranch(branch, branchesTable);
            }
        }

        ///// <summary>
        ///// Delete branches
        ///// </summary>
        ///// <param name="branches"></param>
        public static void DeleteBranches(List<Branch> branches)
        {
            ITable branchesTable = _rastr.Tables.Item(RastrConstantNames.BranchTable);

            foreach(var branch in branches)
            {
                branchesTable.SetSel($"ip.ny={branch.BranchStartNode.Number}&iq.ny={branch.BranchEndNode.Number}");

                branchesTable.DelRowS();
            }
        }

        /// <summary>
        /// Delete blank branches
        /// </summary>
        public static void DeleteBlankBranches()
        {
            ITable branchesTable = _rastr.Tables.Item(RastrConstantNames.BranchTable);

            branchesTable.SetSel("ip.ny=0|iq.ny=0");

            branchesTable.DelRowS();
        }

        /// <summary>
        /// Get adjustment range
        /// </summary>
        /// <param name="adjustmentRangeIndex">Index of adjustment range in table</param>
        /// <param name="adjustmentRangeTable">Adjustment range table</param>
        /// <returns></returns>
        private static AdjustmentRange GetAdjustmentRangeByIndex(int adjustmentRangeIndex, 
                                                                 ITable adjustmentRangeTable)
        {
            var rangeNumber = GetElementParameterValue<int>(adjustmentRangeTable,
                                                            RastrConstantNames.AdjustmentRangeNumberColumn,
                                                            adjustmentRangeIndex);

            var rangeActivePower = GetElementParameterValue<double>(adjustmentRangeTable,
                                                                    RastrConstantNames.AdjustmentRangeActivePowerColumn,
                                                                    adjustmentRangeIndex);

            var rangeMinimumReactivePower = GetElementParameterValue<double>(adjustmentRangeTable,
                                                                             RastrConstantNames.AdjustmentRangeMinimumReactivePowerColumn,
                                                                             adjustmentRangeIndex);

            var rangeMaximumReactivePower = GetElementParameterValue<double>(adjustmentRangeTable,
                                                                             RastrConstantNames.AdjustmentRangeMaximumReactivePowerColumn,
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

            ITable adjustmentRangesTable = _rastr.Tables.Item(RastrConstantNames.AdjustmentRangeTable);

            for (var i = 0; i < GetAdjustmentRangesCount(); i++)
            {
                adjustmentRanges.Add(GetAdjustmentRangeByIndex(i, adjustmentRangesTable));
            }

            return adjustmentRanges;
        }

        /// <summary>
        /// Get adjustment ranges count
        /// </summary>
        /// <returns></returns>
        public static int GetAdjustmentRangesCount()
        {
            ITable adjustmentRangesTable = _rastr.Tables.Item(RastrConstantNames.AdjustmentRangeTable);
            return adjustmentRangesTable.Count;
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
        /// <param name="generatorsTable">Generators table</param>
        /// <returns></returns>
        private static Generator GetGeneratorByIndex(int generatorIndex, ITable generatorsTable)
        {
            var generatorNumber = GetElementParameterValue<int>(generatorsTable,
                                                                RastrConstantNames.GeneratorNumberColumn,
                                                                generatorIndex);

            var generatorName = GetElementParameterValue<string>(generatorsTable,
                                                                 RastrConstantNames.GeneratorNameColumn,
                                                                 generatorIndex);

            var generatorMaxActivePower = GetElementParameterValue<double>(generatorsTable,
                                                                           RastrConstantNames.GeneratorMaxActivePowerColumn,
                                                                           generatorIndex);

            return new Generator(generatorIndex,
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

            ITable generatorsTable = _rastr.Tables.Item(RastrConstantNames.GeneratorTable);

            for (var i = 0; i < GetGeneratorsCount(); i++)
            {
                var generator = GetGeneratorByIndex(i, generatorsTable);

                var generatorNodeNumber = GetElementParameterValue<int>(generatorsTable,
                                                                        RastrConstantNames.GeneratorNodeNumberColumn,
                                                                        i);

                var generatorPQDiagramNumber = GetElementParameterValue<int>(generatorsTable,
                                                                            RastrConstantNames.GeneratorPQDiagramNumberColumn,
                                                                            i);

                if (nodesDict.ContainsKey(generatorNodeNumber))
                {
                    generator.GeneratorNode = nodesDict[generatorNodeNumber];
                }

                if (pqDiagramsDict.ContainsKey(generatorPQDiagramNumber))
                {
                    generator.GeneratorPQDiagram = pqDiagramsDict[generatorPQDiagramNumber];
                }

                generators.Add(generator);
            }

            return generators;
        }

        /// <summary>
        /// Get generators count
        /// </summary>
        /// <returns></returns>
        public static int GetGeneratorsCount()
        {
            ITable generatorsTable = _rastr.Tables.Item(RastrConstantNames.GeneratorTable);
            return generatorsTable.Count;
        }

        /// <summary>
        /// Update generators nodes
        /// </summary>
        /// <param name="newGeneratorsNode">New generators node</param>
        /// <param name="generators">Generators</param>
        public static void UpdateGeneratorsNodes(Node newGeneratorsNode, List<Generator> generators)
        {
            ITable generatorsTable = _rastr.Tables.Item(RastrConstantNames.GeneratorTable);

            foreach (var generator in generators)
            {
                SetElementParameterValue(generatorsTable, 
                                         RastrConstantNames.GeneratorNodeNumberColumn, 
                                         generator.Index, 
                                         newGeneratorsNode.Number);
            }
        }
    }
}

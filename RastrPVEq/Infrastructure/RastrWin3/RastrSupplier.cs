using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASTRALib;
using RastrPVEq.Models.RastrWin3;

namespace RastrPVEq.Infrastructure.RastrWin3
{
    /// <summary>
    /// Rastr supply method provider class
    /// </summary>
    public static class RastrSupplier
    {
        /// <summary>
        /// Rastr com-object
        /// </summary>
        private static readonly Rastr _rastr = new Rastr();

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
        /// Delete row
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="table"></param>
        private static void DeleteRowByIndex(int rowIndex, ITable table)
        {
            table.DelRow(rowIndex);
        }

        /// <summary>
        /// Get node
        /// </summary>
        /// <param name="nodeIndex">Index of node in table</param>
        /// <returns></returns>
        private static Node GetNodeByIndex(int nodeIndex, ITable nodesTable)
        {
            var nodeNumber = GetElementParameterValue<int>(nodesTable,
                                                           RastrNames.NodeNumberColumn,
                                                           nodeIndex);

            var nodeName = GetElementParameterValue<string>(nodesTable,
                                                            RastrNames.NodeNameColumn,
                                                            nodeIndex);

            var nodeRatedVoltage = GetElementParameterValue<double>(nodesTable,
                                                                    RastrNames.NodeRatedVoltageColumn,
                                                                    nodeIndex);

            var nodeDistrictNumber = GetElementParameterValue<int>(nodesTable,
                                                                   RastrNames.NodeDistrictNumberColumn,
                                                                   nodeIndex);

            var nodeTerritoryNumber = GetElementParameterValue<int>(nodesTable,
                                                                    RastrNames.NodeTerritoryNumberColumn,
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

            ITable nodesTable = _rastr.Tables.Item(RastrNames.NodeTable);

            for (int i = 0; i < nodesTable.Count; i++)
            {
                nodes.Add(GetNodeByIndex(i, nodesTable));
            }

            return nodes;
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

            SetElementParameterValue(nodeTable, RastrNames.NodeNumberColumn, addedRowIndex, node.Number);
            SetElementParameterValue(nodeTable, RastrNames.NodeNameColumn, addedRowIndex, node.Name);
            SetElementParameterValue(nodeTable, RastrNames.NodeRatedVoltageColumn, addedRowIndex, node.RatedVoltage);
            SetElementParameterValue(nodeTable, RastrNames.NodeDistrictNumberColumn, addedRowIndex, node.DistrictNumber);
            SetElementParameterValue(nodeTable, RastrNames.NodeTerritoryNumberColumn, addedRowIndex, node.TerritoryNumber);
        }

        /// <summary>
        /// Add nodes
        /// </summary>
        /// <param name="nodes">List of nodes</param>
        public static void AddNodes(List<Node> nodes)
        {
            ITable nodesTable = _rastr.Tables.Item(RastrNames.NodeTable);

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
            var nodesNames = nodes.Select(n => n.Name).ToList();

            ITable nodesTable = _rastr.Tables.Item(RastrNames.NodeTable);

            foreach (var nodeName in nodesNames)
            {
                var rowsCount = nodesTable.Count;

                for (int i = 0; i < nodesTable.Count; i++)
                {
                    var name = GetElementParameterValue<string>(nodesTable, RastrNames.NodeNameColumn, i);

                    if (nodeName == name)
                    {
                        DeleteRowByIndex(i, nodesTable);
                    }
                }
            }  
        }

        /// <summary>
        /// Get branch
        /// </summary>
        /// <param name="branchIndex">Index of branch in table</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Branch GetBranchByIndex(int branchIndex, ITable branchesTable)
        {
            var branchType = GetElementParameterValue<int>(branchesTable,
                                                           RastrNames.BranchTypeColumn,
                                                           branchIndex) == 0 ? BranchType.Line 
                                                                             : BranchType.Transformer;

            var branchName = GetElementParameterValue<string>(branchesTable,
                                                              RastrNames.BranchNameColumn,
                                                              branchIndex);

            var branchResistance = GetElementParameterValue<double>(branchesTable,
                                                                    RastrNames.BranchResistanceColumn,
                                                                    branchIndex);

            var branchInductance = GetElementParameterValue<double>(branchesTable,
                                                                    RastrNames.BranchInductanceColumn,
                                                                    branchIndex);

            var branchCapacitance = GetElementParameterValue<double>(branchesTable,
                                                                     RastrNames.BranchCapacitanceColumn,
                                                                     branchIndex);

            var branchTranformationRatio = GetElementParameterValue<double>(branchesTable,
                                                                            RastrNames.BranchTransformationRatioColumn,
                                                                            branchIndex);

            var branchDistrictNumber = GetElementParameterValue<int>(branchesTable,
                                                                     RastrNames.BranchDistrictNumberColumn,
                                                                     branchIndex);

            var branchTerritoryNumber = GetElementParameterValue<int>(branchesTable,
                                                                      RastrNames.BranchTerritoryNumberColumn,
                                                                      branchIndex);

            var branchAdmissableCurrent = GetElementParameterValue<double>(branchesTable,
                                                                           RastrNames.BranchAdmissableCurrentColumn,
                                                                           branchIndex);

            var branchAdmissableEquipmentCurrent = GetElementParameterValue<double>(branchesTable,
                                                                                    RastrNames.BranchEquipmentAdmissableCurrentColumn,
                                                                                    branchIndex);

            return new Branch(branchIndex,
                              branchType,
                              branchName,
                              branchResistance,
                              branchInductance, 
                              branchCapacitance,
                              branchTranformationRatio,
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

            ITable branchesTable = _rastr.Tables.Item(RastrNames.BranchTable);

            for (int i = 0; i < branchesTable.Count; i++)
            {
                var branch = GetBranchByIndex(i, branchesTable);

                var branchStartNodeNumber = GetElementParameterValue<int>(branchesTable,
                                                                          RastrNames.BranchStartNodeColumn,
                                                                          i);

                var branchEndNodeNumber = GetElementParameterValue<int>(branchesTable,
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
        /// Add branch
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branchTable"></param>
        private static void AddBranch(Branch branch, ITable branchTable)
        {
            branchTable.AddRow();

            var addedRowIndex = branchTable.Count - 1;

            SetElementParameterValue(branchTable, RastrNames.BranchStartNodeColumn, addedRowIndex, branch.BranchStartNode.Number);
            SetElementParameterValue(branchTable, RastrNames.BranchEndNodeColumn, addedRowIndex, branch.BranchEndNode.Number);
            SetElementParameterValue(branchTable, RastrNames.BranchResistanceColumn, addedRowIndex, branch.Resistance);
            SetElementParameterValue(branchTable, RastrNames.BranchInductanceColumn, addedRowIndex, branch.Inductance);
            SetElementParameterValue(branchTable, RastrNames.BranchCapacitanceColumn, addedRowIndex, branch.Capacitance);
            SetElementParameterValue(branchTable, RastrNames.BranchTransformationRatioColumn, addedRowIndex, branch.TransformationRatio);
            SetElementParameterValue(branchTable, RastrNames.BranchDistrictNumberColumn, addedRowIndex, branch.DistrictNumber);
            SetElementParameterValue(branchTable, RastrNames.BranchTerritoryNumberColumn, addedRowIndex, branch.TerritoryNumber);
            SetElementParameterValue(branchTable, RastrNames.BranchAdmissableCurrentColumn, addedRowIndex, branch.AdmissableCurrent);
            SetElementParameterValue(branchTable, RastrNames.BranchEquipmentAdmissableCurrentColumn, addedRowIndex, branch.EquipmentAdmissableCurrent);
        }

        /// <summary>
        /// Add branches
        /// </summary>
        /// <param name="branches"></param>
        public static void AddBranches(List<Branch> branches)
        {
            ITable branchesTable = _rastr.Tables.Item(RastrNames.BranchTable);

            foreach (var branch in branches)
            {
                AddBranch(branch, branchesTable);
            }
        }

        /// <summary>
        /// Delete branches
        /// </summary>
        /// <param name="branches"></param>
        public static void DeleteBranches(List<Branch> branches)
        {
            var branchNames = branches.Select(b => b.Name).ToList();

            ITable branchesTable = _rastr.Tables.Item(RastrNames.BranchTable);

            foreach(var branchName in branchNames)
            {
                var rowsCount = branchesTable.Count;

                for (int i = 0; i < branchesTable.Count; i++)
                {
                    var name = GetElementParameterValue<string>(branchesTable, RastrNames.BranchNameColumn, i);

                    if (branchName == name)
                    {
                        DeleteRowByIndex(i, branchesTable);
                    }
                }
            }
        }

        public static void DeleteBlankBranches()
        {
            ITable branchesTable = _rastr.Tables.Item(RastrNames.BranchTable);

            branchesTable.SetSel("ip.ny=0|iq.ny=0");

            branchesTable.DelRowS();
        }

        /// <summary>
        /// Disable branch
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branchTable"></param>
        private static void DisableBranch(Branch branch, ITable branchTable)
        {
            SetElementParameterValue(branchTable, RastrNames.BranchStatusColumn, branch.Index, 1);
        }

        /// <summary>
        /// Disable branches
        /// </summary>
        /// <param name="branches"></param>
        public static void DisableBranches(List<Branch> branches)
        {
            ITable branchesTable = _rastr.Tables.Item(RastrNames.BranchTable);

            foreach (var branch in branches)
            {
                DisableBranch(branch, branchesTable);
            }
        }

        /// <summary>
        /// Get adjustment range
        /// </summary>
        /// <param name="adjustmentRangeIndex">Index of adjustment range in table</param>
        /// <returns></returns>
        private static AdjustmentRange GetAdjustmentRangeByIndex(int adjustmentRangeIndex, 
                                                                 ITable adjustmentRangeTable)
        {
            var rangeNumber = GetElementParameterValue<int>(adjustmentRangeTable,
                                                            RastrNames.AdjustmentRangeNumberColumn,
                                                            adjustmentRangeIndex);

            var rangeActivePower = GetElementParameterValue<double>(adjustmentRangeTable,
                                                                    RastrNames.AdjustmentRangeActivePowerColumn,
                                                                    adjustmentRangeIndex);

            var rangeMinimumReactivePower = GetElementParameterValue<double>(adjustmentRangeTable,
                                                                             RastrNames.AdjustmentRangeMinimumReactivePowerColumn,
                                                                             adjustmentRangeIndex);

            var rangeMaximumReactivePower = GetElementParameterValue<double>(adjustmentRangeTable,
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

            ITable adjustmentRangesTable = _rastr.Tables.Item(RastrNames.AdjustmentRangeTable);

            for (int i = 0; i < adjustmentRangesTable.Count; i++)
            {
                adjustmentRanges.Add(GetAdjustmentRangeByIndex(i, adjustmentRangesTable));
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
        private static Generator GetGeneratorByIndex(int generatorIndex, ITable generatorsTable)
        {
            var generatorNumber = GetElementParameterValue<int>(generatorsTable,
                                                                RastrNames.GeneratorNumberColumn,
                                                                generatorIndex);

            var generatorName = GetElementParameterValue<string>(generatorsTable,
                                                                 RastrNames.GeneratorNameColumn,
                                                                 generatorIndex);

            var generatorMaxActivePower = GetElementParameterValue<double>(generatorsTable,
                                                                           RastrNames.GeneratorMaxActivePowerColumn,
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

            ITable generatorsTable = _rastr.Tables.Item(RastrNames.GeneratorTable);

            for (int i = 0; i < generatorsTable.Count; i++)
            {
                var generator = GetGeneratorByIndex(i, generatorsTable);

                var generatorNodeNumber = GetElementParameterValue<int>(generatorsTable,
                                                                        RastrNames.GeneratorNodeNumberColumn,
                                                                        i);

                var generatorPQDiagramNuber = GetElementParameterValue<int>(generatorsTable,
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

        /// <summary>
        /// Update generators node
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="branchTable"></param>
        private static void UpdateGeneratorsNode(Generator generator, ITable generatorTable, Node newGeneratorsNode)
        {
            SetElementParameterValue(generatorTable, RastrNames.GeneratorNodeNumberColumn, generator.Index, newGeneratorsNode.Number);
        }

        /// <summary>
        /// Update generators nodes
        /// </summary>
        /// <param name="newGeneratorsNode"></param>
        /// <param name="generators"></param>
        public static void UpdateGeneratorsNodes(Node newGeneratorsNode, List<Generator> generators)
        {
            ITable generatorsTable = _rastr.Tables.Item(RastrNames.GeneratorTable);

            foreach (var generator in generators)
            {
                UpdateGeneratorsNode(generator, generatorsTable, newGeneratorsNode);
            }
        }
    }
}

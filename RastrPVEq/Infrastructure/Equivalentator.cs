using System;
using System.Collections.Generic;
using System.Linq;
using RastrPVEq.Models.PowerSystem;
using RastrPVEq.Models.Topology;
using RastrPVEq.ViewModels;

namespace RastrPVEq.Infrastructure
{
    /// <summary>
    /// Equivalentator class
    /// </summary>
    public static class Equivalentator
    {
        /// <summary>
        /// Get nodes of equivalence group
        /// </summary>
        /// <param name="equivalenceGroup"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Node> GetNodesOfEquivalenceGroup(EquivalenceGroupViewModel equivalenceGroup)
        {
            var nodesOfEquivalenceGroup = new List<Node>();

            var equivalenceBranches = equivalenceGroup.EquivalenceBranches;

            foreach (var equivalenceBranch in equivalenceBranches)
            {
                if (equivalenceBranch.BranchStartNode != null)
                {
                    nodesOfEquivalenceGroup.Add(equivalenceBranch.BranchStartNode);
                }
                else
                {
                    throw new NullReferenceException($"{equivalenceBranch.Name} haven't start node");
                }

                if (equivalenceBranch.BranchEndNode != null)
                {
                    nodesOfEquivalenceGroup.Add(equivalenceBranch.BranchEndNode);
                }
                else
                {
                    throw new NullReferenceException($"{equivalenceBranch.Name} haven't end node");
                }
            }

            return nodesOfEquivalenceGroup.Distinct().ToList();
        }

        /// <summary>
        /// Get generators of equivalence group
        /// </summary>
        /// <param name="nodesOfEquivalenceGroup"></param>
        /// <param name="generators"></param>
        /// <returns></returns>
        public static List<Generator> GetGeneratorsOfEquivalenceGroup(List<Node> nodesOfEquivalenceGroup,
                                                                      List<Generator> generators)
        {
            var generatorsOfEquivalenceGroup = new List<Generator>();

            foreach (var generator in generators)
            {
                if (generator.GeneratorNode == null) continue;

                if (nodesOfEquivalenceGroup.Contains(generator.GeneratorNode))
                {
                    generatorsOfEquivalenceGroup.Add(generator);
                }
            }

            return generatorsOfEquivalenceGroup;
        }

        /// <summary>
        /// Get Graph Of Equivalence Group
        /// </summary>
        /// <param name="equivalenceGroup">Equivalence group</param>
        /// <param name="nodesOfEquivalenceGroup">Nodes of equivalence group</param>
        /// <returns></returns>
        public static Graph<Node> GetGraphOfEquivalenceGroup(EquivalenceGroupViewModel equivalenceGroup, List<Node> nodesOfEquivalenceGroup)
        {
            var graph = new Graph<Node>();

            foreach (var node in nodesOfEquivalenceGroup)
            {
                graph.AddVertex(node);
            }

            foreach (var equivalenceBranch in equivalenceGroup.EquivalenceBranches)
            {
                var startNode = equivalenceBranch.BranchStartNode;
                var endNode = equivalenceBranch.BranchEndNode;

                graph.AddEdge(startNode, endNode, equivalenceBranch.Resistance);
                graph.AddEdge(endNode, startNode, equivalenceBranch.Resistance);
            }

            return graph;
        }

        /// <summary>
        /// Get equivalence branch to generators power (flowed) method
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceGroup"></param>
        /// <param name="generatorsOfEquivalenceGroup"></param>
        /// <param name="dijkstra"></param>
        /// <returns></returns>
        public static Dictionary<Branch, double> GetEquivalenceBranchToGeneratorsPower(EquivalenceNodeViewModel equivalenceNode,
                                                                                       EquivalenceGroupViewModel equivalenceGroup,
                                                                                       List<Generator> generatorsOfEquivalenceGroup,
                                                                                       Dijkstra<Node> dijkstra)
        {
            var branchToGeneratorsPower = new Dictionary<Branch, double>();

            var equivalenceBranches = equivalenceGroup.EquivalenceBranches;

            foreach (var branch in equivalenceBranches)
            {
                branchToGeneratorsPower.Add(branch, 0);
            }

            foreach (var generator in generatorsOfEquivalenceGroup)
            {
                var vertexPath = dijkstra.FindShortestPath(equivalenceNode.NodeElement, generator.GeneratorNode);

                if (vertexPath.Count > 1)
                {
                    for (var i = 0; i < vertexPath.Count - 1; i++)
                    {
                        var firstNode = vertexPath.ElementAt(i).VertexData;
                        var secondNode = vertexPath.ElementAt(i + 1).VertexData;

                        var branch = FindBranchInEquivalenceGroup(equivalenceGroup, firstNode, secondNode);

                        if (branchToGeneratorsPower.ContainsKey(branch))
                        {
                            branchToGeneratorsPower[branch] += generator.MaxActivePower;
                        }
                        else
                        {
                            throw new InvalidOperationException($"Group isn't contain {branch.Name}");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Can't find path to {generator.Name}");

                }
            }
            return branchToGeneratorsPower;
        }

        /// <summary>
        /// Find Branch in Equivalence Group method
        /// </summary>
        /// <param name="equivalenceGroup"></param>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Branch FindBranchInEquivalenceGroup(EquivalenceGroupViewModel equivalenceGroup,
                                                           Node firstNode,
                                                           Node secondNode)
        {
            foreach (var branch in equivalenceGroup.EquivalenceBranches)
            {
                if (branch.BranchStartNode == firstNode)
                {
                    if (branch.BranchEndNode == secondNode)
                    {
                        return branch;
                    }
                }
                else if (branch.BranchStartNode == secondNode)
                {
                    if (branch.BranchEndNode == firstNode)
                    {
                        return branch;
                    }
                }
            }

            throw new Exception("Group isn't contain branch with such nodes");
        }

        /// <summary>
        /// Get total generators power
        /// </summary>
        /// <param name="generatorsOfEquivalenceGroup"></param>
        /// <returns></returns>
        private static double GetTotalGeneratorsPower(List<Generator> generatorsOfEquivalenceGroup)
        {
            double totalGeneratorsPower = 0;

            foreach (var generator in generatorsOfEquivalenceGroup)
            {
                totalGeneratorsPower += generator.MaxActivePower;
            }

            return totalGeneratorsPower;
        }

        /// <summary>
        /// Equivalent branches
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceBranchToGeneratorsPower"></param>
        /// <param name="generatorsOfEquivalenceGroup"></param>
        /// <param name="equivalenceGroup"></param>
        /// <exception cref="Exception"></exception>
        public static void GetEquivalentBranches(EquivalenceNodeViewModel equivalenceNode,
                                                 Dictionary<Branch, double> equivalenceBranchToGeneratorsPower,
                                                 List<Generator> generatorsOfEquivalenceGroup,
                                                 EquivalenceGroupViewModel equivalenceGroup)
        {
            var totalGeneratorsPower = GetTotalGeneratorsPower(generatorsOfEquivalenceGroup);

            var groupedByTypeEquivalenceBranches = equivalenceBranchToGeneratorsPower.GroupBy(kvpair => kvpair.Key.BranchType);

            foreach (var branchType in groupedByTypeEquivalenceBranches)
            {
                double equivalentResistance = 0;
                double equivalentInductance = 0;
                double equivalentCapacitance = 0;
                double equivalentTransformerRatio = 0;
                var equivalentDistrictNumber = equivalenceNode.NodeElement.DistrictNumber;
                var equivalentTerritoryNumber = equivalenceNode.NodeElement.TerritoryNumber;
                double equivalentAdmissableCurrent = 0;
                double equivalentAdmissableEquipmentCurrent = 0;

                foreach (var kvpair in branchType)
                {
                    equivalentResistance += kvpair.Key.Resistance * Math.Pow(kvpair.Value, 2);
                    equivalentInductance += kvpair.Key.Inductance * Math.Pow(kvpair.Value, 2);
                    equivalentCapacitance += kvpair.Key.Capacitance;
                    equivalentTransformerRatio += kvpair.Key.TransformationRatio * kvpair.Value;
                    equivalentAdmissableCurrent += kvpair.Key.AdmissableCurrent;
                    equivalentAdmissableEquipmentCurrent += kvpair.Key.EquipmentAdmissableCurrent;
                }

                equivalentResistance /= Math.Pow(totalGeneratorsPower, 2);
                equivalentInductance /= Math.Pow(totalGeneratorsPower, 2);
                equivalentTransformerRatio /= totalGeneratorsPower;

                var equivalentBranchName = "Эквивалент";

                equivalentBranchName += branchType.Key is BranchType.Transformer ? " ТР" : " Л";

                var equivalentBranch = new Branch(branchType.Key,
                                                  $"{equivalentBranchName}",
                                                  equivalentResistance,
                                                  equivalentInductance,
                                                  equivalentCapacitance,
                                                  equivalentTransformerRatio,
                                                  equivalentDistrictNumber,
                                                  equivalentTerritoryNumber,
                                                  equivalentAdmissableCurrent,
                                                  equivalentAdmissableEquipmentCurrent);

                equivalenceGroup.EquivalentBranches.Add(equivalentBranch);
            }
        }

        /// <summary>
        /// Get intermediate equivalence node
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceGroup"></param>
        public static void GetIntermediateEquivalentNode(EquivalenceNodeViewModel equivalenceNode,
                                                        EquivalenceGroupViewModel equivalenceGroup)
        {
            foreach (var node in equivalenceGroup.EquivalenceNodes)
            {
                if (node == equivalenceNode.NodeElement) continue;

                var equivalentNodeVoltage = node.RatedVoltage;

                if (equivalentNodeVoltage != equivalenceNode.NodeElement.RatedVoltage) continue;

                var equivalentNodeNumber = node.Number;
                var equivalentNodeName = $"{equivalenceGroup.Name} : экв. СШ {equivalentNodeVoltage} кВ";
                var equivalentDistrictNumber = node.DistrictNumber;
                var equivalentTerritoryNumber = node.TerritoryNumber;

                equivalenceGroup.IntermedieteEquivalentNode = new Node(equivalentNodeNumber,
                    equivalentNodeName,
                    equivalentNodeVoltage,
                    equivalentDistrictNumber,
                    equivalentTerritoryNumber);
            }
        }

        /// <summary>
        /// Get Generator Equivalent Node
        /// </summary>
        /// <param name="equivalenceGroup"></param>
        public static void GetGeneratorEquivalentNode(EquivalenceGroupViewModel equivalenceGroup)
        {
            var generatorNode = equivalenceGroup.EquivalenceGenerators.First().GeneratorNode;

            var equivalentGeneratorNodeName = $"{equivalenceGroup.Name} : экв. СШ {generatorNode.RatedVoltage} кВ";

            equivalenceGroup.GeneratorEquivalentNode = new Node(generatorNode.Number,
                                                                 equivalentGeneratorNodeName,
                                                                 generatorNode.RatedVoltage,
                                                                 generatorNode.DistrictNumber,
                                                                 generatorNode.TerritoryNumber);
        }

        /// <summary>
        /// Set Equivalent Node To Equivalent Branch
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceGroup"></param>
        public static void SetEquivalentNodeToEquivalentBranch(EquivalenceNodeViewModel equivalenceNode,
                                                               EquivalenceGroupViewModel equivalenceGroup)
        {
            foreach (var equivalentBranch in equivalenceGroup.EquivalentBranches)
            {
                switch (equivalentBranch.BranchType)
                {
                    case BranchType.Line:
                        equivalentBranch.BranchStartNode = equivalenceNode.NodeElement;
                        equivalentBranch.BranchEndNode = equivalenceGroup.IntermedieteEquivalentNode;
                        break;
                    case BranchType.Transformer:
                        equivalentBranch.BranchStartNode = equivalenceGroup.IntermedieteEquivalentNode;
                        equivalentBranch.BranchEndNode = equivalenceGroup.GeneratorEquivalentNode;
                        break;
                    default:
                        throw new ArgumentException($"Unexpected branch type");
                }
            }
        }

        /// <summary>
        /// Check is has equivalence branches duplicates
        /// </summary>
        /// <param name="equivalenceGroup">Equivalence group</param>
        /// <returns></returns>
        public static bool IsHasEquivalenceBranchesDuplicates(EquivalenceGroupViewModel equivalenceGroup)
        {
            var equivalenceBranches = equivalenceGroup.EquivalenceBranches;

            var totalBranches = equivalenceBranches.Count();
            var uniqueBranches = equivalenceBranches.Distinct().Count();

            return totalBranches != uniqueBranches;
        }

        /// <summary>
        /// Check one generators rated voltage level
        /// </summary>
        /// <param name="generatorsOfEquivalenceGroup">Generators of equivalence group</param>
        /// <returns></returns>
        public static bool IsOneGeneratorsRatedVoltageLevel(List<Generator> generatorsOfEquivalenceGroup)
        {
            var generatorsRatedVoltages = new List<double>();

            foreach (var generator in generatorsOfEquivalenceGroup)
            {
                var ratedVoltage = generator.GeneratorNode.RatedVoltage;

                generatorsRatedVoltages.Add(ratedVoltage);
            }

            var uniqueGeneratorsRatedVoltages = generatorsRatedVoltages.Distinct().Count();

            return uniqueGeneratorsRatedVoltages == 1;
        }
    }
}

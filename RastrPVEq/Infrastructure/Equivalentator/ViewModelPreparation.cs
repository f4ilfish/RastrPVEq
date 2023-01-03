using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RastrPVEq.Models;
using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.ViewModels;

namespace RastrPVEq.Infrastructure.Equivalentator
{
    public class ViewModelPreparation
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
        public static List<Generator> GetGeneratorsOfEquvialenceGroup(List<Node> nodesOfEquivalenceGroup,
                                                                      List<Generator> generators)
        {
            var generatorsOfEquivalenceGroup = new List<Generator>();

            foreach (var generator in generators)
            {
                if (generator.GeneratorNode != null)
                {
                    var generatorNode = generator.GeneratorNode;

                    if (nodesOfEquivalenceGroup.Contains(generator.GeneratorNode))
                    {
                        generatorsOfEquivalenceGroup.Add(generator);
                    }
                }
                else
                {
                    throw new NullReferenceException($"{generator.Name} не привязан к узлу");
                }
            }

            return generatorsOfEquivalenceGroup;
        }

        // <summary>
        /// Get Graph Of Equivalence Group
        /// </summary>
        /// <param name="equivalenceGroupNodes"></param>
        /// <param name="equivalenceGroupViewModel"></param>
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
        /// Get Equivalence Branch to Generators Power (flowed) method
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
                    for (int i = 0; i < vertexPath.Count - 1; i++)
                    {
                        var firstNode = vertexPath.ElementAt(i).Data;
                        var secondNode = vertexPath.ElementAt(i + 1).Data;

                        var branch = FindBranchInEquivalenceGroup(equivalenceGroup, firstNode, secondNode);

                        if (branchToGeneratorsPower.ContainsKey(branch))
                        {
                            branchToGeneratorsPower[branch] += generator.MaxActivePower;
                        }
                        else
                        {
                            throw new InvalidOperationException("Ветвь в словаре не найдена");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Невозможно найти путь");

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

            throw new Exception("Нет ветвей с такой парой узлов");
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

        public static void EquivalentBranches(Dictionary<Branch, double> equivalenceBranchToGeneratorsPower,
                                              List<Generator> generatorsOfEquivalenceGroup,
                                              EquivalenceGroupViewModel equivalenceGroup)
        {
            var totalGeneratorsPower = GetTotalGeneratorsPower(generatorsOfEquivalenceGroup);

            var groupedByTypeEquivalenceBranches = equivalenceBranchToGeneratorsPower.GroupBy(kvpair => kvpair.Key.BranchType);

            foreach (var branchType in groupedByTypeEquivalenceBranches)
            {
                double equivalentResistance = 0;
                double equivalentInductance = 0;
                double equivalentTranformerRatio = 0;

                foreach (var kvpair in branchType)
                {
                    equivalentResistance += kvpair.Key.Resistance * Math.Pow(kvpair.Value, 2);
                    equivalentInductance += kvpair.Key.Inductance * Math.Pow(kvpair.Value, 2);
                    equivalentTranformerRatio += kvpair.Key.TransformationRatio * kvpair.Value;
                }

                equivalentResistance /= Math.Pow(totalGeneratorsPower, 2);
                equivalentInductance /= Math.Pow(totalGeneratorsPower, 2);
                equivalentTranformerRatio /= totalGeneratorsPower;

                var equivalentBranchName = "Эквивалент";

                if (branchType.Key is BranchType.Line)
                {
                    equivalentBranchName += " Л";
                }
                else if (branchType.Key is BranchType.Transformer)
                {
                    equivalentBranchName += " ТР";
                }
                else
                {
                    equivalentBranchName += " ?";
                }

                var equivalentBranch = new Branch(ElementStatus.Enable,
                                                  branchType.Key,
                                                  $"{equivalentBranchName}",
                                                  equivalentResistance,
                                                  equivalentInductance,
                                                  equivalentTranformerRatio);

                equivalenceGroup.EquivalentBranches.Add(equivalentBranch);
            }
        }
    }
}

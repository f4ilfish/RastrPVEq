using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

using RastrPVEq.Infrastructure;
using RastrPVEq.Infrastructure.RastrWin3;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.Views;



namespace RastrPVEq.ViewModels
{
    /// <summary>
    /// Main Window View Model class
    /// </summary>
    [INotifyPropertyChanged]
    public partial class MainWindowViewModel
    {

        /// <summary>
        /// Nodes
        /// </summary>
        [ObservableProperty]
        private List<Node> _nodes = new();

        /// <summary>
        /// Branches
        /// </summary>
        [ObservableProperty]
        private List<Branch> _branches = new();

        /// <summary>
        /// Generators
        /// </summary>
        [ObservableProperty]
        private List<Generator> _generators = new();

        /// <summary>
        /// PQ Diagrams
        /// </summary>
        [ObservableProperty]
        private List<PQDiagram> _pqDiagrams = new();

        /// <summary>
        /// Download file command
        /// </summary>
        [RelayCommand]
        private async void DownloadFile()
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Открыть файл режима",
                Filter = "Файл режима (*.rg2)|*.rg2"
            };

            bool? response = openFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                var templatePath = "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2";
                RastrSupplier.LoadFileByTemplate(openFileDialog.FileName, templatePath);

                var nodesTask = RastrSupplierAsync.GetNodesAsync();
                var pqDiagramsTask = RastrSupplierAsync.GetPQDiagramsAsync();

                await nodesTask;
                Nodes = nodesTask.Result;
                var branchesTask = RastrSupplierAsync.GetBranchesAsync(Nodes);

                await pqDiagramsTask;
                PqDiagrams = pqDiagramsTask.Result;
                var generatorsTask = RastrSupplierAsync.GetGeneratorsAsync(Nodes, PqDiagrams);

                await Task.WhenAll(branchesTask, generatorsTask);
                Branches = branchesTask.Result;
                Generators = generatorsTask.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        /// <summary>
        /// Equivalence Nodes
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EquivalenceNodeViewModel> _equivalenceNodes = new();

        /// <summary>
        /// Selected Node
        /// </summary>
        [ObservableProperty]
        private Node _selectedNode;

        /// <summary>
        /// Selected Equivalence Node
        /// </summary>
        [ObservableProperty]
        private EquivalenceNodeViewModel _selectedEquivalenceNode;

        /// <summary>
        /// Selected Equivalence Group
        /// </summary>
        [ObservableProperty]
        private EquivalenceGroupViewModel _selectedEquivalenceGroup;

        /// <summary>
        /// Selected Branch
        /// </summary>
        [ObservableProperty]
        private Branch _selectedBranch;

        /// <summary>
        /// Selected Equivalence Branch
        /// </summary>
        [ObservableProperty]
        private Branch _selectedEquivalenceBranch;

        /// <summary>
        /// Add Node to Equivalence Nodes command
        /// </summary>
        [RelayCommand]
        private void AddNodeToEquivalenceNodes()
        {
            if (SelectedNode != null)
            {
                EquivalenceNodes.Add(new EquivalenceNodeViewModel(SelectedNode));
            }
        }

        /// <summary>
        /// Remode Node from Equivalence Nodes command
        /// </summary>
        [RelayCommand]
        private void RemoveNodeFromEquivalenceNodes()
        {
            if (SelectedEquivalenceNode != null)
            {
                EquivalenceNodes.Remove(SelectedEquivalenceNode);
            }
        }

        /// <summary>
        /// Add Equivalence Group to Equivalence Node command
        /// </summary>
        [RelayCommand]
        private void AddEquivalenceGroupToEquivalenceNode()
        {
            if (SelectedEquivalenceNode != null)
            {
                if (SelectedEquivalenceNode.EquivalenceGroups.Count != 0)
                {
                    var newGroupId = SelectedEquivalenceNode.EquivalenceGroups
                                     .Max(group => group.Id) + 1;

                    SelectedEquivalenceNode.EquivalenceGroups
                        .Add(new EquivalenceGroupViewModel(newGroupId, $"Группа {newGroupId}"));

                    return;
                }

                SelectedEquivalenceNode.EquivalenceGroups
                        .Add(new EquivalenceGroupViewModel(1, "Группа 1"));
            }
        }

        /// <summary>
        /// Delete Equivalence Group from Equivalence Node command
        /// </summary>
        [RelayCommand]
        private void DeleteEquivalenceGroupFromEquivalenceNode()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedEquivalenceNode != null)
            {
                SelectedEquivalenceNode.EquivalenceGroups
                    .Remove(SelectedEquivalenceGroup);
            }
        }

        /// <summary>
        /// Add Branch To Equivalence Group command
        /// </summary>
        [RelayCommand]
        private void AddBranchToEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedBranch != null)
            {
                SelectedEquivalenceGroup.EquivalenceBranches
                    .Add(SelectedBranch);
            }
        }

        /// <summary>
        /// Remove Branch from Equivalence Group command
        /// </summary>
        [RelayCommand]
        private void RemoveBranchFromEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedEquivalenceBranch != null)
            {
                SelectedEquivalenceGroup.EquivalenceBranches
                    .Remove(SelectedEquivalenceBranch);
            }
        }

        /// <summary>
        /// Calculate equivalent command
        /// </summary>
        [RelayCommand]
        private void CalculateEquivalent()
        {
            foreach (var equivalenceNode in EquivalenceNodes)
            {
                foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                {
                    var nodesOfEquivalenceGroup = GetNodeOfEquivalenceGroup(equivalenceGroup);

                    if (!IsNodesOfEquivalenceGroupContainEquivalenceGroup(equivalenceNode, nodesOfEquivalenceGroup))
                    {
                        MessageBox.Show($"{equivalenceGroup.Name} не содержит {equivalenceNode.NodeElement.Name}");
                        continue;
                    }

                    var generatorsOfEquivalenceGroup = GetGeneratorsOfEquivalenceGroup(nodesOfEquivalenceGroup, Generators);

                    var graphOfEquivalenceGroup = GetGraphOfEquivalenceGroup(nodesOfEquivalenceGroup, equivalenceGroup);
                    var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                    var equivalenceBranchToGeneratorsPower = GetEquivalenceBranchToGeneratorsPower(equivalenceNode,
                                                                                                   equivalenceGroup,
                                                                                                   generatorsOfEquivalenceGroup,
                                                                                                   dijkstraGraph);

                    /// расчет эквивалента
                    double totalGeneratorsPower = 0;

                    foreach (var generator in generatorsOfEquivalenceGroup)
                    {
                        totalGeneratorsPower += generator.Key.MaxActivePower;
                    }

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

        /// <summary>
        /// Get Equivalence Branch to Generators Power (flowed) method
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceGroup"></param>
        /// <param name="equivalenceGroupGenerators"></param>
        /// <param name="dijkstra"></param>
        /// <returns></returns>
        private Dictionary<Branch, double> GetEquivalenceBranchToGeneratorsPower(EquivalenceNodeViewModel equivalenceNode,
                                                                      EquivalenceGroupViewModel equivalenceGroup,
                                                                      Dictionary<Generator, Node> equivalenceGroupGenerators,
                                                                      Dijkstra<Node> dijkstra)
        {
            var branchToGeneratorsPower = new Dictionary<Branch, double>();

            foreach (var branch in equivalenceGroup.EquivalenceBranches)
            {
                if (!branchToGeneratorsPower.TryAdd(branch, 0))
                {
                    MessageBox.Show($"{equivalenceGroup.Name} имеет дубликаты для эквивалентирования");
                };
            }

            foreach (var generator in equivalenceGroupGenerators)
            {
                var nodePath = dijkstra.FindShortestPath(equivalenceNode.NodeElement, generator.Value);

                if (nodePath.Count > 1)
                {
                    for (int i = 0; i < nodePath.Count - 1; i++)
                    {
                        var firstNode = nodePath.ElementAt(i).Data;
                        var secondNode = nodePath.ElementAt(i + 1).Data;

                        var branch = FindBranchInEquivalenceGroup(equivalenceGroup, firstNode, secondNode);

                        if (branchToGeneratorsPower.ContainsKey(branch))
                        {
                            branchToGeneratorsPower[branch] += generator.Key.MaxActivePower;
                        }
                        else
                        {
                            MessageBox.Show($"{branch.Name} не содержится в {equivalenceGroup.Name}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Путь между{equivalenceNode.NodeElement.Name} и {generator.Value.Name} не найден");
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
        private Branch FindBranchInEquivalenceGroup(EquivalenceGroupViewModel equivalenceGroup,
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
        /// Get Node of Equivalence Group method
        /// </summary>
        /// <param name="equivalenceGroupViewModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        private List<Node> GetNodeOfEquivalenceGroup(EquivalenceGroupViewModel equivalenceGroupViewModel)
        {
            var nodes = new List<Node>();

            var equivalenceBranches = equivalenceGroupViewModel.EquivalenceBranches;

            if (equivalenceBranches != null)
            {
                foreach (var equivalenceBranch in equivalenceBranches)
                {

                    if (equivalenceBranch.BranchStartNode != null)
                    {
                        nodes.Add(equivalenceBranch.BranchStartNode);
                    }

                    if (equivalenceBranch.BranchEndNode != null)
                    {
                        nodes.Add(equivalenceBranch.BranchEndNode);
                    }
                }
            }
            else
            {
                throw new Exception("Branch callection is empty");
            }

            return nodes.Distinct().ToList();
        }

        /// <summary>
        /// Is Nodes of Equivalence Group contain Equivalence Group
        /// </summary>
        /// <param name="equivalenceNodeViewModel"></param>
        /// <param name="equivalenceGroupNodes"></param>
        /// <returns></returns>
        private bool IsNodesOfEquivalenceGroupContainEquivalenceGroup(EquivalenceNodeViewModel equivalenceNodeViewModel,
                                                              List<Node> equivalenceGroupNodes)
        {
            return equivalenceGroupNodes.Contains(equivalenceNodeViewModel.NodeElement);
        }

        /// <summary>
        /// Get Generators of Equivalence Group method
        /// </summary>
        /// <param name="equivalenceGroupNodes"></param>
        /// <param name="generators"></param>
        /// <returns></returns>
        private Dictionary<Generator, Node> GetGeneratorsOfEquivalenceGroup(List<Node> equivalenceGroupNodes, List<Generator> generators)
        {
            var equivalenceGroupGenerators = new Dictionary<Generator, Node>();

            foreach (var generator in generators)
            {
                if (generator.GeneratorNode != null)
                {
                    var generatorNode = generator.GeneratorNode;

                    if (equivalenceGroupNodes.Contains(generatorNode))
                    {
                        equivalenceGroupGenerators[generator] = generatorNode;
                    }
                }
            }

            return equivalenceGroupGenerators;
        }

        /// <summary>
        /// Get Graph Of Equivalence Group
        /// </summary>
        /// <param name="equivalenceGroupNodes"></param>
        /// <param name="equivalenceGroupViewModel"></param>
        /// <returns></returns>
        private Graph<Node> GetGraphOfEquivalenceGroup(List<Node> equivalenceGroupNodes,
                                                      EquivalenceGroupViewModel equivalenceGroupViewModel)
        {
            var graph = new Graph<Node>();

            foreach (var node in equivalenceGroupNodes)
            {
                graph.AddVertex(node);
            }

            foreach (var equivalenceBranch in equivalenceGroupViewModel.EquivalenceBranches)
            {
                var startNode = equivalenceBranch.BranchStartNode;
                var endNode = equivalenceBranch.BranchEndNode;

                graph.AddEdge(startNode, endNode, equivalenceBranch.Resistance);
                graph.AddEdge(endNode, startNode, equivalenceBranch.Resistance);
            }

            return graph;
        }

        /// NOT USED
        /// <summary>
        /// Open Node Selection Window command 
        /// </summary>
        [RelayCommand]
        public void OpenNodeSelectionWindow()
        {
            var nodeSelectionWindow = new NodeSelectionWindow(this);
            nodeSelectionWindow.Show();
        }

        /// <summary>
        /// Main Window View Model default constructor
        /// </summary>
        public MainWindowViewModel() { }
    }
}

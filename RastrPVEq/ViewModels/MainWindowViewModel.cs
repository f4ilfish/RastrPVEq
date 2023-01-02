using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.Infrastructure;
using RastrPVEq.Infrastructure.RastrWin3;
using RastrPVEq.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    public partial class MainWindowViewModel
    {
        #region Загрузка модели

        [ObservableProperty]
        private List<Node> _nodes = new();

        [ObservableProperty]
        private List<Branch> _branches = new();

        [ObservableProperty]
        private List<Generator> _generators = new();

        [ObservableProperty]
        private List<PQDiagram> _pqDiagrams = new();

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

        #endregion

        #region Подготовка модели эквивалентирования

        [ObservableProperty]
        private ObservableCollection<EquivalenceNodeViewModel> _equivalenceNodesCollection = new();

        [ObservableProperty]
        private Node _selectedNode;

        [ObservableProperty]
        private EquivalenceNodeViewModel _selectedEquivalenceNode;

        [ObservableProperty]
        private EquivalenceGroupViewModel _selectedEquivalenceGroup;

        [ObservableProperty]
        private Branch _selectedBranch;

        [ObservableProperty]
        private EquivalenceBranchViewModel _selectedEquivalenceBranch;

        [RelayCommand]
        private void AddNodeToEquivalenceNodes()
        {
            if (SelectedNode != null)
            {
                EquivalenceNodesCollection
                    .Add(new EquivalenceNodeViewModel(SelectedNode));
            }
        }

        [RelayCommand]
        private void RemoveNodeFromEquivalenceNodes()
        {
            if (SelectedEquivalenceNode != null)
            {
                EquivalenceNodesCollection
                    .Remove(SelectedEquivalenceNode);
            }
        }

        [RelayCommand]
        private void AddEquivalenceGroupToEquivalenceNode()
        {
            if (SelectedEquivalenceNode != null)
            {
                if (SelectedEquivalenceNode.EquivalenceGroupCollection.Count != 0)
                {
                    var newGroupId = SelectedEquivalenceNode.EquivalenceGroupCollection
                                     .Max(group => group.Id) + 1;

                    SelectedEquivalenceNode.EquivalenceGroupCollection
                        .Add(new EquivalenceGroupViewModel(newGroupId, $"Группа {newGroupId}"));

                    return;
                }

                SelectedEquivalenceNode.EquivalenceGroupCollection
                        .Add(new EquivalenceGroupViewModel(1, "Группа 1"));
            }
        }

        [RelayCommand]
        private void RemoveEquivalenceGroupFromEquivalenceNode()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedEquivalenceNode != null)
            {
                SelectedEquivalenceNode.EquivalenceGroupCollection
                    .Remove(SelectedEquivalenceGroup);
            }
        }

        [RelayCommand]
        private void AddBranchToEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedBranch != null)
            {
                SelectedEquivalenceGroup.EquivalenceBranchCollection
                    .Add(new EquivalenceBranchViewModel(SelectedBranch));
            }
        }

        [RelayCommand]
        private void RemoveEquivalenceBranchFromEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup != null
                && SelectedEquivalenceBranch != null)
            {
                SelectedEquivalenceGroup.EquivalenceBranchCollection
                    .Remove(SelectedEquivalenceBranch);
            }
        }

        #endregion

        #region Тест поиска пути

        [ObservableProperty]
        private List<Node> _equivalenceGroupNodes = new();

        [ObservableProperty]
        private bool _isEquivalenceGroupNodesContainEquivalenceNode;

        [ObservableProperty]
        private Dictionary<Generator, Node> _dictOfGenerators = new();

        [ObservableProperty]
        private Graph<Node> graph = new();

        [ObservableProperty]
        private Dijkstra<Node> dijkstra = new();

        [ObservableProperty]
        private List<GraphVertex<Node>> nodePath = new();

        [RelayCommand]
        private void Calculate()
        {
            foreach (var equivalenceNode in EquivalenceNodesCollection)
            {
                foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroupCollection)
                {
                    var equivalenceGroupNodes = GetEquivalenceGroupNodes(equivalenceGroup);

                    if (!IsEquivalenceNodeInEquivalenceGroupNodes(equivalenceNode, equivalenceGroupNodes))
                    {
                        MessageBox.Show($"{equivalenceGroup.Name} не содержит {equivalenceNode.NodeElement.Name}");
                        continue;
                    }

                    var equivalenceGroupGenerators = GetEquivalenceGroupGenerators(equivalenceGroupNodes, Generators);

                    graph = GetGraphToEquivalentation(equivalenceGroupNodes, equivalenceGroup);
                    dijkstra = new Dijkstra<Node>(graph);

                    equivalenceGroup.EquivalenceBranchToGeneratorsPower = SetBranchToGeneratorsPower(equivalenceNode,
                                                                                                     equivalenceGroup,
                                                                                                     equivalenceGroupGenerators,
                                                                                                     dijkstra);

                    var equivalenceBranchToGeneratorsPower = equivalenceGroup.EquivalenceBranchToGeneratorsPower;

                    /// расчет эквивалента
                    double totalGeneratorsPower = 0;

                    foreach (var generator in equivalenceGroupGenerators)
                    {
                        totalGeneratorsPower += generator.Key.MaxActivePower;
                    }

                    //foreach (var kvpair in equivalenceBranchToGeneratorsPower)
                    //{
                    //    equivalenceBranchToGeneratorsPower[kvpair.Key] = Math.Pow(kvpair.Value, 2);
                    //}

                    var groupedEquivalenceBranchToGeneratorsPower = equivalenceBranchToGeneratorsPower.GroupBy(o => o.Key.BranchType);

                    foreach (var branchType in groupedEquivalenceBranchToGeneratorsPower)
                    {
                        double eqResistance = 0;
                        double eqInductance = 0;
                        double eqTranformerRatio = 0;

                        foreach (var kvpair in branchType)
                        {
                            eqResistance += kvpair.Key.Resistance * Math.Pow(kvpair.Value, 2);
                            eqInductance += kvpair.Key.Inductance * Math.Pow(kvpair.Value, 2);
                            eqTranformerRatio += kvpair.Key.TransformationRatio * kvpair.Value;
                        }

                        eqResistance /= Math.Pow(totalGeneratorsPower, 2);
                        eqInductance /= Math.Pow(totalGeneratorsPower, 2);
                        eqTranformerRatio /= totalGeneratorsPower;

                        var newBranch = new Branch(1,
                                                   ElementStatus.Enable,
                                                   branchType.Key,
                                                   $"{equivalenceGroup.Name} {branchType}",
                                                   eqResistance,
                                                   eqInductance,
                                                   eqTranformerRatio);

                        equivalenceGroup.EquivalentBranches.Add(newBranch);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Проставить веса (переток мощности) ветвям
        /// </summary>
        /// <param name="equivalenceNode"></param>
        /// <param name="equivalenceGroup"></param>
        /// <param name="equivalenceGroupGenerators"></param>
        /// <param name="dijkstra"></param>
        /// <returns></returns>
        private Dictionary<Branch, double> SetBranchToGeneratorsPower(EquivalenceNodeViewModel equivalenceNode,
                                                                      EquivalenceGroupViewModel equivalenceGroup,
                                                                      Dictionary<Generator, Node> equivalenceGroupGenerators,
                                                                      Dijkstra<Node> dijkstra)
        {
            var branchToGeneratorsPower = new Dictionary<Branch, double>();

            foreach (var branch in equivalenceGroup.EquivalenceBranchCollection)
            {
                if (!branchToGeneratorsPower.TryAdd(branch.BranchElement, 0))
                {
                    MessageBox.Show($"{equivalenceGroup.Name} имеет дубликаты для эквивалентирования");
                };
            }

            foreach (var generator in equivalenceGroupGenerators)
            {
                nodePath = dijkstra.FindShortestPath(equivalenceNode.NodeElement, generator.Value);

                if (nodePath.Count > 1)
                {
                    for (int i = 0; i < nodePath.Count - 1; i++)
                    {
                        var firstNode = nodePath.ElementAt(i).Data;
                        var secondNode = nodePath.ElementAt(i + 1).Data;

                        var branch = FindBranchInEquivalenceBranchGroup(equivalenceGroup, firstNode, secondNode);

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
        /// Найти ветвь группы эквивалентирования по узлам
        /// </summary>
        /// <param name="equivalenceGroup"></param>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Branch FindBranchInEquivalenceBranchGroup(EquivalenceGroupViewModel equivalenceGroup,
                                                          Node firstNode,
                                                          Node secondNode)
        {
            foreach (var branch in equivalenceGroup.EquivalenceBranchCollection)
            {
                if (branch.BranchElement.BranchStartNode == firstNode)
                {
                    if (branch.BranchElement.BranchEndNode == secondNode)
                    {
                        return branch.BranchElement;
                    }
                }
                else if (branch.BranchElement.BranchStartNode == secondNode)
                {
                    if (branch.BranchElement.BranchEndNode == firstNode)
                    {
                        return branch.BranchElement;
                    }
                }
            }

            throw new Exception("Нет ветвей с такой парой узлов");
        }

        /// <summary>
        /// Получить узлы ветвей эквивалентируемой группы
        /// </summary>
        /// <param name="equivalenceGroupViewModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        private List<Node> GetEquivalenceGroupNodes(EquivalenceGroupViewModel equivalenceGroupViewModel)
        {
            var nodes = new List<Node>();

            var equivalenceBranchCollection = equivalenceGroupViewModel.EquivalenceBranchCollection;

            if (equivalenceBranchCollection != null)
            {
                foreach (var equivalenceBranchViewModel in equivalenceBranchCollection)
                {
                    var eqivalenceBranch = equivalenceBranchViewModel.BranchElement;

                    if (eqivalenceBranch.BranchStartNode != null)
                    {
                        nodes.Add(eqivalenceBranch.BranchStartNode);
                    }

                    if (eqivalenceBranch.BranchEndNode != null)
                    {
                        nodes.Add(eqivalenceBranch.BranchEndNode);
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
        /// Узлы ветвей эквивалентируемой группы содержат узел-вершину
        /// </summary>
        /// <param name="equivalenceNodeViewModel"></param>
        /// <param name="equivalenceGroupNodes"></param>
        /// <returns></returns>
        private bool IsEquivalenceNodeInEquivalenceGroupNodes(EquivalenceNodeViewModel equivalenceNodeViewModel,
                                                              List<Node> equivalenceGroupNodes)
        {
            return equivalenceGroupNodes.Contains(equivalenceNodeViewModel.NodeElement);
        }

        /// <summary>
        /// Получить генераторы узлов ветвей эквивалентируемой группы
        /// </summary>
        /// <param name="equivalenceGroupNodes"></param>
        /// <param name="generators"></param>
        /// <returns></returns>
        private Dictionary<Generator, Node> GetEquivalenceGroupGenerators(List<Node> equivalenceGroupNodes, List<Generator> generators)
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
        /// Восстановить граф сети
        /// </summary>
        /// <param name="equivalenceGroupNodes"></param>
        /// <param name="equivalenceGroupViewModel"></param>
        /// <returns></returns>
        private Graph<Node> GetGraphToEquivalentation(List<Node> equivalenceGroupNodes,
                                                      EquivalenceGroupViewModel equivalenceGroupViewModel)
        {
            var graph = new Graph<Node>();

            foreach (var node in equivalenceGroupNodes)
            {
                graph.AddVertex(node);
            }

            foreach (var branchViewModel in equivalenceGroupViewModel.EquivalenceBranchCollection)
            {
                var startNode = branchViewModel.BranchElement.BranchStartNode;
                var endNode = branchViewModel.BranchElement.BranchEndNode;

                graph.AddEdge(startNode, endNode, branchViewModel.BranchElement.Resistance);
                graph.AddEdge(endNode, startNode, branchViewModel.BranchElement.Resistance);
            }

            return graph;
        }

        [RelayCommand]
        public void OpenNodeSelectionWindow()
        {
            var nodeSelectionWindow = new NodeSelectionWindow(this);
            nodeSelectionWindow.Show();
        }

        public MainWindowViewModel() { }
    }
}

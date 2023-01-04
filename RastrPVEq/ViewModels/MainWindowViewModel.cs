using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.Infrastructure.Equivalentator;
using RastrPVEq.Infrastructure.RastrWin3;
using System.ComponentModel.DataAnnotations;

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

        [RelayCommand]
        private void SaveFile()
        {
            SaveFileDialog saveFileDialog = new()
            {
                Title = "Сохранить файл режима",
                Filter = "Файл режима (*.rg2)|*.rg2"
            };

            bool? response = saveFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                var tmpListNodes = new List<Node>() 
                {
                    new Node(1, 1, "Тест 1", 500, 0, 0),
                    new Node(2, 2, "Тест 2", 500, 0, 0),
                };

                RastrSupplier.AddNodes(tmpListNodes);

                var templatePath = "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2";
                RastrSupplier.SaveFileByTemplate(saveFileDialog.FileName, templatePath);
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
        /// Exceptions
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Exception> _validateErrors = new();

        /// <summary>
        /// Validate Model command
        /// </summary>
        [RelayCommand]
        private void ValidateModel()
        {
            ValidateErrors.Clear();

            /// Проверка узлов эквивалентирования
            if (EquivalenceNodes.Count != 0)
            {
                foreach (var equivalenceNode in EquivalenceNodes)
                {
                    var nodeNumber = equivalenceNode.NodeElement.Number;
                    var nodeName = equivalenceNode.NodeElement.Name;

                    /// Проверка наличия групп эквивалентирования
                    if (equivalenceNode.EquivalenceGroups.Count != 0)
                    {
                        foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                        {
                            var groupName = equivalenceGroup.Name;

                            /// Проверка наличия ветвей в группах
                            if (equivalenceGroup.EquivalenceBranches.Count != 0)
                            {

                                /// Проверка наличия дублирующихся ветвей
                                if (ViewModelValidation.IsHasEquivalenceBranchesDuplicates(equivalenceGroup))
                                {
                                    ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Дубликаты ветвей"));
                                }

                                var nodesOfEquivalenceGroup = ViewModelPreparation.GetNodesOfEquivalenceGroup(equivalenceGroup);

                                /// Проверка связи узла с группой
                                if (!nodesOfEquivalenceGroup.Contains(equivalenceNode.NodeElement))
                                {
                                    ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствуют связь узла с группой"));
                                }

                                var generatorsOfEquivalenceGroup = ViewModelPreparation.GetGeneratorsOfEquvialenceGroup(nodesOfEquivalenceGroup, Generators);

                                /// Проверка наличия генераторов в группе
                                if (generatorsOfEquivalenceGroup.Count != 0)
                                {
                                    /// Проверка равности Uном генераторов в группе
                                    if (ViewModelValidation.IsOneGeneratorsRatedVoltageLevel(generatorsOfEquivalenceGroup))
                                    {
                                        /// Проверка наличия связи между генератором и узлом-вершиной
                                        
                                        // TODO: DFS алгоритм
                                        var graphOfEquivalenceGroup = ViewModelPreparation.GetGraphOfEquivalenceGroup(equivalenceGroup, nodesOfEquivalenceGroup);
                                        var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                                        foreach (var generator in generatorsOfEquivalenceGroup)
                                        {
                                            var vertexPath = dijkstraGraph.FindShortestPath(equivalenceNode.NodeElement, generator.GeneratorNode);

                                            var nodesPath = new List<Node>();

                                            foreach (var vertex in vertexPath)
                                            {
                                                nodesPath.Add(vertex.Data);
                                            }

                                            var generatorNodeName = generator.GeneratorNode.Name;
                                            var generatorNodeNumber = generator.GeneratorNode.Number;

                                            if (!nodesPath.Contains(equivalenceNode.NodeElement)
                                                || !nodesPath.Contains(generator.GeneratorNode))
                                            {
                                                ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствует связь узла с генератором (в узле {generatorNodeNumber} {generatorNodeName})"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Генераторы имеют разный Uном"));
                                    }
                                }
                                else
                                {
                                    ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Группа не содержит генераторов"));
                                }
                            }
                            else
                            {
                                ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствуют ветви"));
                            }
                        }
                    }
                    else
                    {
                        ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | Отсутствуют группы"));
                    }
                }
            }
            else
            {
                MessageBox.Show("Отсутствуют узлы эквивалентирования");
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
                    var nodesOfEquivalenceGroup = ViewModelPreparation.GetNodesOfEquivalenceGroup(equivalenceGroup);

                    var generatorsOfEquivalenceGroup = ViewModelPreparation.GetGeneratorsOfEquvialenceGroup(nodesOfEquivalenceGroup, Generators);

                    var graphOfEquivalenceGroup = ViewModelPreparation.GetGraphOfEquivalenceGroup(equivalenceGroup, nodesOfEquivalenceGroup);
                    var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                    var equivalenceBranchToGeneratorsPower = ViewModelPreparation.GetEquivalenceBranchToGeneratorsPower(equivalenceNode,
                                                                                                                       equivalenceGroup,
                                                                                                                       generatorsOfEquivalenceGroup,
                                                                                                                       dijkstraGraph);

                    ViewModelPreparation.EquivalentBranches(equivalenceNode, 
                                                            equivalenceBranchToGeneratorsPower,
                                                            generatorsOfEquivalenceGroup,
                                                            equivalenceGroup);
                }
            }
        }

        ///// <summary>
        ///// Open Node Selection Window command 
        ///// </summary>
        //[RelayCommand]
        //public void OpenNodeSelectionWindow()
        //{
        //    var nodeSelectionWindow = new NodeSelectionWindow(this);
        //    nodeSelectionWindow.Show();
        //}

        /// <summary>
        /// Main Window View Model default constructor
        /// </summary>
        public MainWindowViewModel() { }
    }
}

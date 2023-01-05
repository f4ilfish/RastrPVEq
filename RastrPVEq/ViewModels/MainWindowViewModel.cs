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
        private ObservableCollection<Node> _nodes = new();

        /// <summary>
        /// Branches
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _branches = new();

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
        /// Is file downloaded
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
        [NotifyCanExecuteChangedFor(nameof(ValidateModelCommand))]
        private bool _isFileDownloaded;

        /// <summary>
        /// Is model changed
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CalculateEquivalentCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
        private bool _isModelChanged = true;

        /// <summary>
        /// Validate errors
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CalculateEquivalentCommand))]
        [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
        private ObservableCollection<Exception> _validateErrors = new();

        /// <summary>
        /// Is calculated equivalent
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
        private bool _isCalculatedEquivalent = false;

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
                /// костыль на оповещение об изменениях в модели
                IsFileDownloaded = false;
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

                EquivalenceNodes.Clear();
                
                var templatePath = "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2";
                RastrSupplier.LoadFileByTemplate(openFileDialog.FileName, templatePath);

                var nodesTask = RastrSupplierAsync.GetNodesAsync();
                var pqDiagramsTask = RastrSupplierAsync.GetPQDiagramsAsync();

                await nodesTask;
                Nodes = new ObservableCollection<Node>(nodesTask.Result);
                var branchesTask = RastrSupplierAsync.GetBranchesAsync(Nodes.ToList());

                await pqDiagramsTask;
                PqDiagrams = pqDiagramsTask.Result;
                var generatorsTask = RastrSupplierAsync.GetGeneratorsAsync(Nodes.ToList(), PqDiagrams);

                await Task.WhenAll(branchesTask, generatorsTask);
                Branches = new ObservableCollection<Branch>(branchesTask.Result);
                Generators = generatorsTask.Result;

                IsFileDownloaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        /// <summary>
        /// Save file command
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSaveFile))]
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
                foreach (var equivalenceNode in EquivalenceNodes)
                {
                    foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                    {
                        var branchesToDelete = new List<Branch>(equivalenceGroup.EquivalenceBranches);

                        RastrSupplier.DeleteBranches(branchesToDelete);

                        var nodesToDelete = new List<Node>(equivalenceGroup.EquivalenceNodes);
                        nodesToDelete.Remove(equivalenceNode.NodeElement);

                        RastrSupplier.DeleteNodes(nodesToDelete);

                        var nodesToAdd = new List<Node>
                        {
                            equivalenceGroup.IntermedietEquivalentNode,
                            equivalenceGroup.GeneratorEquivalentNode
                        };

                        RastrSupplier.DeleteBlankBranches();

                        RastrSupplier.AddNodes(nodesToAdd);

                        var branchesToAdd = new List<Branch>(equivalenceGroup.EquivalentBranches);

                        RastrSupplier.AddBranches(branchesToAdd);

                        var generatorsToUpdate = new List<Generator>(equivalenceGroup.EquivalenceGenerators);
                        RastrSupplier.UpdateGeneratorsNodes(equivalenceGroup.GeneratorEquivalentNode, generatorsToUpdate);
                    }
                }

                var templatePath = "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2";
                RastrSupplier.SaveFileByTemplate(saveFileDialog.FileName, templatePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }

        /// <summary>
        /// Can save file
        /// </summary>
        /// <returns></returns>
        private bool CanSaveFile()
        {
            return IsFileDownloaded
                    && !IsModelChanged
                    && ValidateErrors.Count == 0
                    && IsCalculatedEquivalent;
        }

        /// <summary>
        /// Close application command
        /// </summary>
        [RelayCommand]
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Add Node to Equivalence Nodes command
        /// </summary>
        [RelayCommand]
        private void AddNodeToEquivalenceNodes()
        {
            if (SelectedNode != null)
            {
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

                EquivalenceNodes.Add(new EquivalenceNodeViewModel(SelectedNode));
                Nodes.Remove(SelectedNode);
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
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

                Nodes.Add(SelectedEquivalenceNode.NodeElement);
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
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

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
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

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
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

                SelectedEquivalenceGroup.EquivalenceBranches
                    .Add(SelectedBranch);
                Branches.Remove(SelectedBranch);
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
                /// костыль на оповещение об изменениях в модели
                IsModelChanged = true;
                IsCalculatedEquivalent = false;

                Branches.Add(SelectedEquivalenceBranch);
                SelectedEquivalenceGroup.EquivalenceBranches
                    .Remove(SelectedEquivalenceBranch);
            }
        }

        /// <summary>
        /// Validate model command
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanValidateModel))]
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

                /// костыль на оповещение об изменении в моделях
                if (ValidateErrors.Count == 0)
                {
                    IsModelChanged = false;
                }
            }
            else
            {
                MessageBox.Show("Отсутствуют узлы эквивалентирования");
            }

        }

        /// <summary>
        /// Can validate model
        /// </summary>
        /// <returns></returns>
        private bool CanValidateModel()
        {
            return IsFileDownloaded;
        }

        /// <summary>
        /// Calculate equivalent command
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanCalulcateEquivalent))]
        private void CalculateEquivalent()
        {
            foreach (var equivalenceNode in EquivalenceNodes)
            {
                foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                {
                    equivalenceGroup.EquivalenceNodes = ViewModelPreparation.GetNodesOfEquivalenceGroup(equivalenceGroup);

                    equivalenceGroup.EquivalenceGenerators = ViewModelPreparation.GetGeneratorsOfEquvialenceGroup(equivalenceGroup.EquivalenceNodes, Generators);

                    var graphOfEquivalenceGroup = ViewModelPreparation.GetGraphOfEquivalenceGroup(equivalenceGroup, equivalenceGroup.EquivalenceNodes);
                    var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                    var equivalenceBranchToGeneratorsPower = ViewModelPreparation.GetEquivalenceBranchToGeneratorsPower(equivalenceNode,
                                                                                                                        equivalenceGroup,
                                                                                                                        equivalenceGroup.EquivalenceGenerators,
                                                                                                                        dijkstraGraph);
                    ViewModelPreparation.GetEquivalentBranches(equivalenceNode, 
                                                               equivalenceBranchToGeneratorsPower,
                                                               equivalenceGroup.EquivalenceGenerators,
                                                               equivalenceGroup);

                    ViewModelPreparation.GetIntermedietEquivalentNode(equivalenceNode, equivalenceGroup);

                    ViewModelPreparation.GetGeneratorEquivalentNode(equivalenceGroup);

                    ViewModelPreparation.SetEquivalentNodeToEquivalentBranch(equivalenceNode,
                                                                             equivalenceGroup);
                }
            }

            IsCalculatedEquivalent = true;
        }

        /// <summary>
        /// Can calculate equivalent
        /// </summary>
        /// <returns></returns>
        private bool CanCalulcateEquivalent()
        {
            return !IsModelChanged
                    && ValidateErrors.Count == 0;
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

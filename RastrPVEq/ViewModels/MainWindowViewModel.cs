using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RastrPVEq.Models.PowerSystem;
using RastrPVEq.Models.Topology;
using RastrPVEq.Infrastructure.RastrSupplier;
using RastrPVEq.Infrastructure;

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
        private ObservableCollection<Node> _nodes;

        /// <summary>
        /// Branches
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _branches;

        /// <summary>
        /// Generators
        /// </summary>
        [ObservableProperty]
        private List<Generator> _generators;

        /// <summary>
        /// Equivalence Nodes
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EquivalenceNodeViewModel> _equivalenceNodes;

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
        /// Is file downloading
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CancelDownloadingFileCommand))]
        private bool _isFileDownloading;

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
        private ObservableCollection<Exception> _validateErrors;

        /// <summary>
        /// Is calculated equivalent
        /// </summary>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
        private bool _isCalculatedEquivalent;

        [ObservableProperty]
        private double _maxStatusBarValue;

        [ObservableProperty]
        private double _currentStatusBarValue;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DownloadFileCommand))]
        private CancellationToken _token;

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

            var response = openFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                var templatePath = $"{AppDomain.CurrentDomain.BaseDirectory}Properties\\режим.rg2";

                RastrProvider.LoadFileByTemplate(openFileDialog.FileName, templatePath);

                /// костыль на оповещение об изменениях в модели
                IsFileDownloaded = false;
                IsModelChanged = true;
                IsCalculatedEquivalent = false;
                IsFileDownloading = true;

                /// костыль для статус бара
                var nodesCount = RastrProvider.GetNodesCount();
                var branchesCount = RastrProvider.GetBranchesCount();
                var generatorsCount = RastrProvider.GetGeneratorsCount();

                MaxStatusBarValue += nodesCount;
                MaxStatusBarValue += branchesCount;
                MaxStatusBarValue += generatorsCount;

                CurrentStatusBarValue = 0;

                /// очистка перед загрузкой
                EquivalenceNodes.Clear();
                
                var nodesTask = RastrProviderAsync.GetNodesAsync(Token);
                
                await nodesTask;
                Nodes = new ObservableCollection<Node>(nodesTask.Result);
                CurrentStatusBarValue += nodesCount;
                
                var branchesTask = RastrProviderAsync.GetBranchesAsync(Nodes.ToList(), Token);
                var generatorsTask = RastrProviderAsync.GetGeneratorsAsync(Nodes.ToList(), Token);

                await generatorsTask;
                Generators = generatorsTask.Result;
                CurrentStatusBarValue += generatorsCount;

                await branchesTask;
                Branches = new ObservableCollection<Branch>(branchesTask.Result);
                CurrentStatusBarValue += branchesCount; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }

            MaxStatusBarValue = 0;
            CurrentStatusBarValue = 0;
            IsFileDownloaded = true;
            IsFileDownloading = false;
        }

        /// <summary>
        /// Cancel downloading file
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanCancelDownloadingFile))]
        private void CancelDownloadingFile()
        {
            var cancelTokenSource = new CancellationTokenSource();
            Token = cancelTokenSource.Token;
            cancelTokenSource.Cancel();
            //Token.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Can cancel downloading file
        /// </summary>
        /// <returns></returns>
        private bool CanCancelDownloadingFile()
        {
            return IsFileDownloading;
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

            var response = saveFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                MaxStatusBarValue = 100;
                CurrentStatusBarValue = 0;

                var nodeValueIncrement = MaxStatusBarValue / EquivalenceNodes.Count;

                foreach (var equivalenceNode in EquivalenceNodes)
                {
                    var groupValueIncrement = nodeValueIncrement / equivalenceNode.EquivalenceGroups.Count;

                    foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                    {
                        var branchesToDelete = new List<Branch>(equivalenceGroup.EquivalenceBranches);

                        RastrProvider.DeleteBranches(branchesToDelete);

                        var nodesToDelete = new List<Node>(equivalenceGroup.EquivalenceNodes);
                        nodesToDelete.Remove(equivalenceNode.NodeElement);

                        RastrProvider.DeleteNodes(nodesToDelete);

                        var nodesToAdd = new List<Node>
                        {
                            equivalenceGroup.IntermediateEquivalentNode,
                            equivalenceGroup.GeneratorEquivalentNode
                        };

                        RastrProvider.DeleteBlankBranches();

                        RastrProvider.AddNodes(nodesToAdd);

                        var branchesToAdd = new List<Branch>(equivalenceGroup.EquivalentBranches);

                        RastrProvider.AddBranches(branchesToAdd);

                        var generatorsToUpdate = new List<Generator>(equivalenceGroup.EquivalenceGenerators);
                        RastrProvider.UpdateGeneratorsNodes(equivalenceGroup.GeneratorEquivalentNode, generatorsToUpdate);

                        CurrentStatusBarValue += groupValueIncrement;
                    }
                }

                var templatePath = $"{AppDomain.CurrentDomain.BaseDirectory}Properties\\режим.rg2";

                RastrProvider.SaveFileByTemplate(saveFileDialog.FileName, templatePath);

                CurrentStatusBarValue = 100;
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
        private static void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Add Node to Equivalence Nodes command
        /// </summary>
        [RelayCommand]
        private void AddNodeToEquivalenceNodes()
        {
            if (SelectedNode == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            EquivalenceNodes.Add(new EquivalenceNodeViewModel(SelectedNode));
            Nodes.Remove(SelectedNode);
        }

        /// <summary>
        /// Remove Node from Equivalence Nodes command
        /// </summary>
        [RelayCommand]
        private void RemoveNodeFromEquivalenceNodes()
        {
            if (SelectedEquivalenceNode == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            Nodes.Add(SelectedEquivalenceNode.NodeElement);
            EquivalenceNodes.Remove(SelectedEquivalenceNode);
        }

        /// <summary>
        /// Add Equivalence Group to Equivalence Node command
        /// </summary>
        [RelayCommand]
        private void AddEquivalenceGroupToEquivalenceNode()
        {
            if (SelectedEquivalenceNode == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            if (SelectedEquivalenceNode.EquivalenceGroups.Count != 0)
            {
                var newGroupId = SelectedEquivalenceNode.EquivalenceGroups
                    .Max(group => @group.Id) + 1;

                SelectedEquivalenceNode.EquivalenceGroups
                    .Add(new EquivalenceGroupViewModel(newGroupId, $"Группа {newGroupId}"));

                return;
            }

            SelectedEquivalenceNode.EquivalenceGroups
                .Add(new EquivalenceGroupViewModel(1, "Группа 1"));
        }

        /// <summary>
        /// Delete Equivalence Group from Equivalence Node command
        /// </summary>
        [RelayCommand]
        private void DeleteEquivalenceGroupFromEquivalenceNode()
        {
            if (SelectedEquivalenceGroup == null 
                || SelectedEquivalenceNode == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            if(SelectedEquivalenceGroup.EquivalenceBranches.Count != 0)
            {
                foreach(var branch in SelectedEquivalenceGroup.EquivalenceBranches)
                {
                    Branches.Add(branch);
                }
            }

            SelectedEquivalenceNode.EquivalenceGroups
                .Remove(SelectedEquivalenceGroup);
        }

        /// <summary>
        /// Add Branch To Equivalence Group command
        /// </summary>
        [RelayCommand]
        private void AddBranchToEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup == null 
                || SelectedBranch == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            SelectedEquivalenceGroup.EquivalenceBranches
                .Add(SelectedBranch);
            Branches.Remove(SelectedBranch);
        }

        /// <summary>
        /// Remove Branch from Equivalence Group command
        /// </summary>
        [RelayCommand]
        private void RemoveBranchFromEquivalenceGroup()
        {
            if (SelectedEquivalenceGroup == null 
                || SelectedEquivalenceBranch == null) return;
            
            /// костыль на оповещение об изменениях в модели
            IsModelChanged = true;
            IsCalculatedEquivalent = false;

            Branches.Add(SelectedEquivalenceBranch);
            SelectedEquivalenceGroup.EquivalenceBranches
                .Remove(SelectedEquivalenceBranch);
        }

        /// <summary>
        /// Validate model command
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanValidateModel))]
        private void ValidateModel()
        {
            ValidateErrors.Clear();

            MaxStatusBarValue = 100;
            CurrentStatusBarValue = 0;

            /// Проверка узлов эквивалентирования
            if (EquivalenceNodes.Count != 0)
            {
                var nodeValueIncrement = MaxStatusBarValue / EquivalenceNodes.Count;

                foreach (var equivalenceNode in EquivalenceNodes)
                {
                    var nodeNumber = equivalenceNode.NodeElement.Number;
                    var nodeName = equivalenceNode.NodeElement.Name;

                    /// Проверка наличия групп эквивалентирования
                    if (equivalenceNode.EquivalenceGroups.Count != 0)
                    {
                        var groupValueIncrement = nodeValueIncrement / equivalenceNode.EquivalenceGroups.Count;

                        foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                        {
                            var groupName = equivalenceGroup.Name;

                            /// Проверка наличия ветвей в группах
                            if (equivalenceGroup.EquivalenceBranches.Count != 0)
                            {

                                /// Проверка наличия дублирующихся ветвей
                                if (Equivalentator.IsHasEquivalenceBranchesDuplicates(equivalenceGroup))
                                {
                                    ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Дубликаты ветвей"));
                                }

                                var nodesOfEquivalenceGroup = Equivalentator.GetNodesOfEquivalenceGroup(equivalenceGroup);

                                /// Проверка связи узла с группой
                                if (!nodesOfEquivalenceGroup.Contains(equivalenceNode.NodeElement))
                                {
                                    ValidateErrors.Add(new Exception($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствуют связь узла с группой"));
                                }

                                var generatorsOfEquivalenceGroup = Equivalentator.GetGeneratorsOfEquivalenceGroup(nodesOfEquivalenceGroup, Generators);

                                /// Проверка наличия генераторов в группе
                                if (generatorsOfEquivalenceGroup.Count != 0)
                                {
                                    /// Проверка равности Uном генераторов в группе
                                    if (Equivalentator.IsOneGeneratorsRatedVoltageLevel(generatorsOfEquivalenceGroup))
                                    {
                                        /// Проверка наличия связи между генератором и узлом-вершиной
                                        
                                        var graphOfEquivalenceGroup = Equivalentator.GetGraphOfEquivalenceGroup(equivalenceGroup, nodesOfEquivalenceGroup);
                                        var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                                        foreach (var generator in generatorsOfEquivalenceGroup)
                                        {
                                            var vertexPath = dijkstraGraph.FindShortestPath(equivalenceNode.NodeElement, generator.GeneratorNode);

                                            var nodesPath = new List<Node>();

                                            foreach (var vertex in vertexPath)
                                            {
                                                nodesPath.Add(vertex.VertexData);
                                            }

                                            var generatorNodeName = generator.GeneratorNode.Name;
                                            var generatorNodeNumber = generator.GeneratorNode.Number;

                                            if (!nodesPath.Contains(equivalenceNode.NodeElement)
                                                || !nodesPath.Contains(generator.GeneratorNode))
                                            {
                                                ValidateErrors.Add(new InvalidOperationException($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствует связь узла с генератором (в узле {generatorNodeNumber} {generatorNodeName})"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ValidateErrors.Add(new InvalidOperationException($"Узел {nodeNumber} {nodeName} | {groupName} | Генераторы имеют разный Uном"));
                                    }
                                }
                                else
                                {
                                    ValidateErrors.Add(new InvalidOperationException($"Узел {nodeNumber} {nodeName} | {groupName} | Группа не содержит генераторов"));
                                }
                            }
                            else
                            {
                                ValidateErrors.Add(new InvalidOperationException($"Узел {nodeNumber} {nodeName} | {groupName} | Отсутствуют ветви"));
                            }

                            CurrentStatusBarValue += groupValueIncrement;
                        }
                    }
                    else
                    {
                        ValidateErrors.Add(new InvalidOperationException($"Узел {nodeNumber} {nodeName} | Отсутствуют группы"));
                    }
                }

                /// костыль на оповещение об изменении в моделях
                if (ValidateErrors.Count == 0)
                {
                    IsModelChanged = false;
                    MessageBox.Show("Модель успешно подготовлена к эквивалентированию");
                }
            }
            else
            {
                ValidateErrors.Add(new InvalidOperationException($"Узлы-вершины для эквивалентирования не заданы"));
            }

            CurrentStatusBarValue = 100;
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
        [RelayCommand(CanExecute = nameof(CanCalculateEquivalent))]
        private void CalculateEquivalent()
        {
            MaxStatusBarValue = 100;
            CurrentStatusBarValue = 0;
            
            foreach (var equivalenceNode in EquivalenceNodes)
            {
                var nodeValueIncrement = MaxStatusBarValue / EquivalenceNodes.Count;

                foreach (var equivalenceGroup in equivalenceNode.EquivalenceGroups)
                {
                    var groupValueIncrement = nodeValueIncrement / equivalenceNode.EquivalenceGroups.Count;

                    equivalenceGroup.EquivalenceNodes.Clear();
                    equivalenceGroup.EquivalenceNodes = Equivalentator.GetNodesOfEquivalenceGroup(equivalenceGroup);

                    equivalenceGroup.EquivalenceGenerators.Clear();
                    equivalenceGroup.EquivalenceGenerators = Equivalentator.GetGeneratorsOfEquivalenceGroup(equivalenceGroup.EquivalenceNodes, Generators);

                    var graphOfEquivalenceGroup = Equivalentator.GetGraphOfEquivalenceGroup(equivalenceGroup, equivalenceGroup.EquivalenceNodes);
                    var dijkstraGraph = new Dijkstra<Node>(graphOfEquivalenceGroup);

                    var equivalenceBranchToGeneratorsPower = Equivalentator.GetEquivalenceBranchToGeneratorsPower(equivalenceNode,
                                                                                                                        equivalenceGroup,
                                                                                                                        equivalenceGroup.EquivalenceGenerators,
                                                                                                                        dijkstraGraph);
                    equivalenceGroup.EquivalentBranches.Clear();
                    Equivalentator.GetEquivalentBranches(equivalenceNode, 
                                                               equivalenceBranchToGeneratorsPower,
                                                               equivalenceGroup.EquivalenceGenerators,
                                                               equivalenceGroup);

                    Equivalentator.GetIntermediateEquivalentNode(equivalenceNode, equivalenceGroup);

                    Equivalentator.GetGeneratorEquivalentNode(equivalenceGroup);

                    Equivalentator.SetEquivalentNodeToEquivalentBranch(equivalenceNode,
                                                                             equivalenceGroup);

                    CurrentStatusBarValue += groupValueIncrement;
                }
            }

            CurrentStatusBarValue = 100;
            IsCalculatedEquivalent = true;
            MessageBox.Show("Эквивалентирование выполнено");
        }

        /// <summary>
        /// Can calculate equivalent
        /// </summary>
        /// <returns></returns>
        private bool CanCalculateEquivalent()
        {
            return !IsModelChanged
                    && ValidateErrors.Count == 0;
        }

        /// <summary>
        /// Main Window View Model default constructor
        /// </summary>
        public MainWindowViewModel()
        {
            Nodes = new ObservableCollection<Node>();
            Branches = new ObservableCollection<Branch>();
            Generators = new List<Generator>();
            EquivalenceNodes = new ObservableCollection<EquivalenceNodeViewModel>();
            ValidateErrors = new ObservableCollection<Exception>();
        }
    }
}

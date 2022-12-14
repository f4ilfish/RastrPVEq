using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Infrastructure.RastrWin3;
using RastrPVEq.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class MainWindowViewModel
    {
        #region Загрузка модели
        
        [ObservableProperty]
        private List<Node> _nodes = new List<Node>();

        [ObservableProperty]
        private List<Branch> _branches = new List<Branch>();

        [ObservableProperty]
        private List<Generator> _generators = new List<Generator>();

        [ObservableProperty]
        private List<PQDiagram> _pqDiagrams = new List<PQDiagram>();

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
                if(SelectedEquivalenceNode.EquivalenceGroupCollection.Count != 0)
                {
                    var newGroupId = SelectedEquivalenceNode.EquivalenceGroupCollection
                                     .Max(group => group.Id) + 1;
                    
                    SelectedEquivalenceNode.EquivalenceGroupCollection
                        .Add(new EquivalenceGroupViewModel(newGroupId, $"Группа {newGroupId}"));
                }

                SelectedEquivalenceNode.EquivalenceGroupCollection
                        .Add(new EquivalenceGroupViewModel(1, "Группа 1"));
            }
        }

        [RelayCommand]
        private void RemoveEquivalenceGroupFromEquivalenceNode()
        {
            if (SelectedEquivalenceGroup!= null 
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

        [RelayCommand]
        private void OpenAddNodeWindow()
        {
            var addNodeWindow = new AddNodeWindow();
            addNodeWindow.Show();
        }

        public MainWindowViewModel() { }
    }
}

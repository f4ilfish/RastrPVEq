﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;

using RastrPVEq.Infrastructure.RastrWin3;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using RastrPVEq.Views;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class MainWindowViewModel
    {
        [ObservableProperty]
        private List<Node> _nodes = new();

        [ObservableProperty]
        private Node _pickedNode;

        [ObservableProperty]
        private List<Branch> _branches = new();

        [ObservableProperty]
        private List<Generator> _generators = new();

        [ObservableProperty]
        private List<PQDiagram> _pqDiagrams = new();

        [ObservableProperty]
        private ObservableCollection<NodeViewModel> nodesCollection = new();

        [ObservableProperty]
        private object _selectedItem;

        [RelayCommand]
        private void AddNode()
        {
            Node firstNode = new(1, ElementStatus.Enable, 1, "First node", 10);
            Node secondNode = new(2, ElementStatus.Disable, 2, "Second node", 20);
            Node thirdNode = new(3, ElementStatus.Enable, 3, "Third node", 30);

            NodeViewModel firstViewNode = new(firstNode);
            NodeViewModel secondViewNode = new(secondNode);
            NodeViewModel thirdViewNode = new(thirdNode);

            Branch firstBranch = new(1, ElementStatus.Enable, BranchType.Switch, "First branch", 0, 0, 0);
            Branch secondBranch = new(2, ElementStatus.Disable, BranchType.Line, "Second branch", 2, 2, 0);
            Branch thirdBranch = new(3, ElementStatus.Enable, BranchType.Transformer, "Third branch", 3, 3, 0.33);

            BranchViewModel firstViewBranch = new(firstBranch);
            BranchViewModel secondViewBranch = new(secondBranch);
            BranchViewModel thirdViewBranch = new(thirdBranch);

            firstViewNode.BranchCollection.Add(firstViewBranch);

            secondViewNode.BranchCollection.Add(firstViewBranch);
            secondViewNode.BranchCollection.Add(secondViewBranch);

            thirdViewNode.BranchCollection.Add(firstViewBranch);
            thirdViewNode.BranchCollection.Add(secondViewBranch);
            thirdViewNode.BranchCollection.Add(thirdViewBranch);

            nodesCollection.Add(firstViewNode);
            nodesCollection.Add(secondViewNode);
            nodesCollection.Add(thirdViewNode);
        }

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
        /// MainWindowViewModel instance constructor
        /// </summary>
        public MainWindowViewModel()
        {

        }
    }
}
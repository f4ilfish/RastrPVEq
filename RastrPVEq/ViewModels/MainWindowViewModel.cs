using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.Infrastructure.Topology;
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
                if(SelectedEquivalenceNode.EquivalenceGroupCollection.Count != 0)
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

        #region Тест поиска пути

        [ObservableProperty]
        private List<Node> _equivalenceGroupNodes = new();

        [ObservableProperty]
        private bool _isEquivalenceGroupNodesContainEquivalenceNode;

        [ObservableProperty]
        private List<Generator> _listOfGenerators = new();

        [ObservableProperty]
        private IGraph<Node> _graphToEquivalentation = new Graph<Node>();

        [ObservableProperty]
        private Dictionary<Generator, IVertex<Node>> _generatorsToVertexDict = new();

        [ObservableProperty]
        private IVertex<Node> _equivalenceNodeVertex = new Vertex<Node>();

        [ObservableProperty]
        private List<IVertex<Node>> _traversedVerices = new();

        [RelayCommand]
        private void Calculate()
        {
            EquivalenceGroupNodes = GetEquivalenceGroupNodes(SelectedEquivalenceGroup);
            
            IsEquivalenceGroupNodesContainEquivalenceNode = IsEquivalenceNodeInEquivalenceGroupNodes(SelectedEquivalenceNode,
                                                                                                     EquivalenceGroupNodes);

            ListOfGenerators = GetEquivalenceGroupGenerators(EquivalenceGroupNodes, Generators);

            GraphToEquivalentation = GetGraphToEquivalentation(EquivalenceGroupNodes, SelectedEquivalenceGroup);

            GeneratorsToVertexDict = GetGeneratorToVertexDict(GraphToEquivalentation, ListOfGenerators);

            var equivalenceNodeVertex = GetEquivalenceNodeVertex(SelectedEquivalenceNode, GraphToEquivalentation);

            TraversedVerices = GetTraversedVerices(equivalenceNodeVertex,
                                                   GeneratorsToVertexDict.ElementAt(1).Value,
                                                   GraphToEquivalentation);
        }

        #endregion

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

            return nodes.Distinct().ToList();
        }

        private bool IsEquivalenceNodeInEquivalenceGroupNodes(EquivalenceNodeViewModel equivalenceNodeViewModel,
                                                               List<Node> equivalenceGroupNodes)
        {
            return equivalenceGroupNodes.Contains(equivalenceNodeViewModel.NodeElement);
        }

        private List<Generator> GetEquivalenceGroupGenerators(List<Node> equivalenceGroupNodes, List<Generator> generators)
        {
            var equivalenceGroupGenerators = new List<Generator>();

            foreach(var generator in generators)
            {
                if (generator.GeneratorNode != null)
                {
                    if (equivalenceGroupNodes.Contains(generator.GeneratorNode))
                    {
                        equivalenceGroupGenerators.Add(generator);
                    }
                }
            }

            return equivalenceGroupGenerators.Distinct().ToList();
        }

        private IGraph<Node> GetGraphToEquivalentation(List<Node> equivalenceGroupNodes, 
                                                      EquivalenceGroupViewModel equivalenceGroupViewModel)
        {
            IGraph<Node> graph = new Graph<Node>();

            Dictionary<Node, IVertex<Node>> vertexToNode = new();

            foreach (var node in equivalenceGroupNodes)
            {
                var vertex = graph.AddVertex(node);
                vertexToNode[node] = vertex;
            }

            foreach(var branchViewModel in equivalenceGroupViewModel.EquivalenceBranchCollection)
            {
                var startNode = branchViewModel.BranchElement.BranchStartNode;
                var endNode = branchViewModel.BranchElement.BranchEndNode;

                var startVertex = vertexToNode[startNode];
                var endVertex = vertexToNode[endNode];

                graph.AddEdge(startVertex, endVertex);
                graph.AddEdge(endVertex, startVertex);
            }

            return graph;
        }

        private Dictionary<Generator, IVertex<Node>> GetGeneratorToVertexDict(IGraph<Node> graph, List<Generator> equivalenceGroupGenerators)
        {
            var generatorsVertexes = new Dictionary<Generator, IVertex<Node>>();

            foreach(var vertex in graph.Vertices)
            {
                foreach(var generator in equivalenceGroupGenerators)
                {
                    if (vertex.Data == generator.GeneratorNode)
                    {
                        generatorsVertexes[generator] = vertex;
                    }
                }
            }

            return generatorsVertexes;
        }

        private IVertex<Node> GetEquivalenceNodeVertex(EquivalenceNodeViewModel equivalenceNodeViewModel, IGraph<Node> graph)
        {
            foreach (var vertex in graph.Vertices)
            {
                if (vertex.Data == equivalenceNodeViewModel.NodeElement)
                {
                    return vertex;
                }
            }

            throw new Exception("Graph dosnt contain equivalence node vertex");
        }

        private List<IVertex<Node>> GetTraversedVerices(IVertex<Node> equivalenceNodeVertex, 
                                                       IVertex<Node> generatorVertex, 
                                                       IGraph<Node> graph)
        {
            var traversal = DepthFirstTraversal<Node>.DepthFirstIterative(graph, equivalenceNodeVertex, generatorVertex);

            var traversedVertices = new List<IVertex<Node>>();

            foreach(var vertex in traversal)
            {
                traversedVertices.Add(vertex);
            }

            return traversedVertices;
        }

        private bool IsHasPath(List<IVertex<Node>> traversedVertices, IVertex<Node> equivalenceNodeVertex, IVertex<Node> generatorVertex)
        {
            return traversedVertices.Contains(equivalenceNodeVertex) 
                   && traversedVertices.Contains(generatorVertex);
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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RastrPVEq.Models.RastrWin3;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    public partial class EquivalenceGroupViewModel
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private ObservableCollection<EquivalenceBranchViewModel> _equivalenceBranchCollection = new();

        //отработка логики эквивалентирования
        [ObservableProperty]
        private List<Node> _nodePath = new();

        [ObservableProperty]
        private Dictionary<Branch, double> _equivalenceBranchToGeneratorsPower = new();

        [ObservableProperty]
        private ObservableCollection<Branch> _equivalentBranches = new();

        public EquivalenceGroupViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

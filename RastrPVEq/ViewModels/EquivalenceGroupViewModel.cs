using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class EquivalenceGroupViewModel
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private ObservableCollection<EquivalenceBranchViewModel> _equivalenceBranchCollection = new();

        public EquivalenceGroupViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

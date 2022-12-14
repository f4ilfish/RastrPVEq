using System.Collections.ObjectModel;
using RastrPVEq.Models.RastrWin3;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class EquivalenceNodeViewModel
    {
        public Node NodeElement { get; set; }

        [ObservableProperty]
        private ObservableCollection<EquivalenceGroupViewModel> _equivalenceGroupCollection = new();

        public EquivalenceNodeViewModel(Node node)
        {
            NodeElement = node;
        }
    }
}

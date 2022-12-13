using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RastrPVEq.Models.RastrWin3;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class EquivalenceNodeViewModel
    {
        public Node NodeElement { get; set; }

        [ObservableProperty]
        private ObservableCollection<EquivalenceGroupBranchViewModel> _groupBranchCollection = new ObservableCollection<EquivalenceGroupBranchViewModel>();

        public EquivalenceNodeViewModel(Node node)
        {
            NodeElement = node;
        }
    }
}

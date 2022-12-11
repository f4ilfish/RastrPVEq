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
    internal partial class NodeViewModel
    {
        public Node NodeElement { get; set; }

        [ObservableProperty]
        private string _nodeName;

        [ObservableProperty]
        private ObservableCollection<BranchViewModel> _branchCollection = new();

        [ObservableProperty]
        private bool _isChecked;

        public NodeViewModel(Node node)
        {
            NodeElement = node;
            NodeName = node.Name;
            IsChecked = false;
        }
    }
}

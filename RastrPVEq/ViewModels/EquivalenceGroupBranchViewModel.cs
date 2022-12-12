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
    internal partial class EquivalenceGroupBranchViewModel
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private ObservableCollection<EquivalenceBranchViewModel> _branchCollection = new();

        public EquivalenceGroupBranchViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

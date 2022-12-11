using RastrPVEq.Models.RastrWin3;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RastrPVEq.ViewModels
{

    [INotifyPropertyChanged]
    internal partial class BranchViewModel
    {
        private Branch BranchElement { get; set; }

        [ObservableProperty]
        private string _branchName;

        [ObservableProperty]
        private bool _isChecked;

        public BranchViewModel(Branch branch)
        {
            BranchElement = branch;
            BranchName = branch.Name;
            IsChecked = false;
        }
    }
}

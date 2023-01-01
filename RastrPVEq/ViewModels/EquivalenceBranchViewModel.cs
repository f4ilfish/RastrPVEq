using RastrPVEq.Models.RastrWin3;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RastrPVEq.ViewModels
{
    [INotifyPropertyChanged]
    public partial class EquivalenceBranchViewModel
    {
        public Branch BranchElement { get; set; }

        public EquivalenceBranchViewModel(Branch branch)
        {
            BranchElement = branch;
        }
    }
}

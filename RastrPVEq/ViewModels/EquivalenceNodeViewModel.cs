using System.Collections.ObjectModel;
using RastrPVEq.Models.RastrWin3;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RastrPVEq.ViewModels
{
    /// <summary>
    /// Equivalence Node View Model class
    /// </summary>
    [INotifyPropertyChanged]
    public partial class EquivalenceNodeViewModel
    {
        /// <summary>
        /// Gets or sets node
        /// </summary>
        public Node NodeElement { get; set; }

        /// <summary>
        /// Equvalence group collection
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<EquivalenceGroupViewModel> _equivalenceGroups = new();

        /// <summary>
        /// Equivalence Node View Model instance constructor
        /// </summary>
        /// <param name="node">Equivalence Node node element</param>
        public EquivalenceNodeViewModel(Node node)
        {
            NodeElement = node;
        }
    }
}

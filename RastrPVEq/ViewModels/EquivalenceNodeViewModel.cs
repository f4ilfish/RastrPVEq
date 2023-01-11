using System.Collections.ObjectModel;
using RastrPVEq.Models.PowerSystem;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;

namespace RastrPVEq.ViewModels
{
    /// <summary>
    /// Equivalence node view model class
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
        /// Equivalence node view model class constructor
        /// </summary>
        /// <param name="node">Equivalence Node node element</param>
        public EquivalenceNodeViewModel(Node node)
        {
            NodeElement = node;
        }
    }
}

using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RastrPVEq.Models.RastrWin3;

namespace RastrPVEq.ViewModels
{
    /// <summary>
    /// Equivalence Group View Model class
    /// </summary>
    [INotifyPropertyChanged]
    public partial class EquivalenceGroupViewModel
    {
        /// <summary>
        /// Index
        /// </summary>
        [ObservableProperty]
        private int _id;

        /// <summary>
        /// Name
        /// </summary>
        [ObservableProperty]
        private string _name;

        /// <summary>
        /// Equivalence Branch Collection
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _equivalenceBranches = new();

        /// <summary>
        /// Equivalent Branches Collection
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _equivalentBranches = new();

        /// <summary>
        /// Equivalence Group View Model class instance constructor
        /// </summary>
        /// <param name="id">Equivalence Group index</param>
        /// <param name="name">Equivalence Group name</param>
        public EquivalenceGroupViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

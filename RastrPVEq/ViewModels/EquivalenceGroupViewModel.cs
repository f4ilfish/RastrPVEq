using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RastrPVEq.Models.PowerSystem;

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
        /// Equivalence branches
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _equivalenceBranches;

        /// <summary>
        /// Gets or sets equivalence nodes
        /// </summary>
        public List<Node> EquivalenceNodes { get; set; }

        /// <summary>
        /// Gets or set equivalence generators
        /// </summary>
        public List<Generator> EquivalenceGenerators { get; set; }

        /// <summary>
        /// Gets or sets intermediate equivalent node
        /// </summary>
        public Node IntermediateEquivalentNode { get; set; }

        /// <summary>
        /// Gets or sets generator equivalent node
        /// </summary>
        public Node GeneratorEquivalentNode { get; set; }

        /// <summary>
        /// Equivalent Branches Collection
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _equivalentBranches;

        /// <summary>
        /// Equivalence group view model class constructor
        /// </summary>
        /// <param name="id">Equivalence Group index</param>
        /// <param name="name">Equivalence Group name</param>
        public EquivalenceGroupViewModel(int id, string name)
        {
            Id = id;
            Name = name;
            EquivalenceBranches = new ObservableCollection<Branch>();
            EquivalenceNodes = new List<Node>();
            EquivalenceGenerators = new List<Generator>();
            EquivalentBranches = new ObservableCollection<Branch>();
        }
    }
}

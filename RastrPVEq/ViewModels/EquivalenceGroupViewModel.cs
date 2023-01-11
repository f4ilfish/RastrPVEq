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
        private ObservableCollection<Branch> _equivalenceBranches = new();

        /// <summary>
        /// Gets or sets equivalence nodes
        /// </summary>
        public List<Node> EquivalenceNodes { get; set; } = new();

        /// <summary>
        /// Gets or set equivalence generators
        /// </summary>
        public List<Generator> EquivalenceGenerators { get; set; } = new();

        /// <summary>
        /// Gets or sets intermediete equivalent node
        /// </summary>
        public Node IntermedieteEquivalentNode { get; set; }

        /// <summary>
        /// Gets or sets generator equivalent node
        /// </summary>
        public Node GeneratorEquivalentNode { get; set; }

        /// <summary>
        /// Equivalent Branches Collection
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Branch> _equivalentBranches = new();

        /// <summary>
        /// Equivalence group view model class constructor
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

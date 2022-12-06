using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models.Rastr
{
    /// <summary>
    /// Branch class
    /// </summary>
    public class Branch
    {
        /// <summary>
        /// Index
        /// </summary>
        private int _index;

        /// <summary>
        /// Gets index
        /// </summary>
        public int Index
        {
            get => _index;
            private set
            {
                ValueValidation.CheckIsNegative(value);
                _index = value;
            }
        }

        /// <summary>
        /// Gets branch status
        /// </summary>
        public ElementStatus BranchStatus { get; private set; }

        /// <summary>
        /// Gets branch type
        /// </summary>
        public BranchType BranchType { get; private set; }

        /// <summary>
        /// Gets or sets branch start node
        /// </summary>
        public Node? BranchStartNode { get; set; }

        /// <summary>
        /// Gets or sets branch end node
        /// </summary>
        public Node? BranchEndNode { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        private string _name;

        /// <summary>
        /// Gets name
        /// </summary>
        public string Name
        {
            get => _name;
            private set
            {
                ValueValidation.CheckNotNullOrEmptyString(value);
                _name = value;
            }
        }

        /// <summary>
        /// Resistance
        /// </summary>
        private double _resistance;

        /// <summary>
        /// Gets resistance
        /// </summary>
        public double Resistance
        {
            get => _resistance;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNegative(value);
                _resistance = value;
            }
        }

        /// <summary>
        /// Inductance
        /// </summary>
        private double _inductance;

        /// <summary>
        /// Gets inductance
        /// </summary>
        public double Inductance
        {
            get => _inductance;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                _inductance = value;
            }
        }

        /// <summary>
        /// Transformation ratio
        /// </summary>
        private double _transformationRatio;

        /// <summary>
        /// Gets transformation ratio
        /// </summary>
        public double TransformationRation
        {
            get => _transformationRatio;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNegative(value);
                ValueValidation.CheckIsBiggerThanOne(value);
                _transformationRatio = value;
            }
        }

        /// <summary>
        /// Branch class instance constructor
        /// </summary>
        /// <param name="index">Branch index</param>
        /// <param name="branchStatus">Branch status</param>
        /// <param name="branchType">Branch type</param>
        /// <param name="name">Branch name</param>
        /// <param name="resistance">Branch resistance</param>
        /// <param name="inductance">Branch inductance</param>
        /// <param name="tranformationRatio">Transformation ratio</param>
        public Branch(int index,
                      ElementStatus branchStatus,
                      BranchType branchType,
                      string name,
                      double resistance,
                      double inductance,
                      double tranformationRatio)
        {
            Index = index;
            BranchStatus = branchStatus;
            BranchType = branchType;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            TransformationRation = tranformationRatio;
        }
    }
}

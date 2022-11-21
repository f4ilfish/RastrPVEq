using RastrPVEqConsole.Validation;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Branch class
    /// </summary>
    public class Branch : ElementBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override ElementStatus ElementStatus { get; set; }

        /// <summary>
        /// Branch type's property
        /// </summary>
        public BranchType BranchType { get; set; }

        /// <summary>
        /// Branch start node's property
        /// </summary>
        public Node StartNode { get; set; }

        /// <summary>
        /// Branch end node's property
        /// </summary>
        public Node EndNode { get; set; }

        private int _parellelBranchNumber;

        /// <summary>
        /// Parallel brnch number's property
        /// </summary>
        public int ParallelBranchNumber 
        { 
            get => _parellelBranchNumber;
            set
            {
                ValueValidation.CheckPositiveOrZero(value);
                _parellelBranchNumber = value;
            } 
        }

        /// <summary>
        /// Branch name's field
        /// </summary>
        private string _name;

        /// <summary>
        /// Branch name's property
        /// </summary>
        public override string Name 
        { 
            get => _name; 
            set
            {
                ValueValidation.CheckNotNullOrEmptyString(value);
                _name = value;
            } 
        }

        /// <summary>
        /// Branch resistance's field
        /// </summary>
        private double _resistance;

        /// <summary>
        /// Branch resistance's property
        /// </summary>
        public double Resistance
        {
            get => _resistance;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckPositiveOrZero(value);
                _resistance = value;
            }
        }

        /// <summary>
        /// Branch inductance's field
        /// </summary>
        private double _inductance;

        /// <summary>
        /// Branch inductance's property
        /// </summary>
        public double Inductance
        {
            get => _inductance;
            set
            {
                ValueValidation.CheckNotNaN(value);
                _inductance = value;
            }
        }

        /// <summary>
        /// Branch capacitance's field
        /// </summary>
        private double _capacitance;

        /// <summary>
        /// Branch capacitance's property
        /// </summary>
        public double Capacitance
        {
            get => _capacitance;
            set
            {
                ValueValidation.CheckNotNaN(value);
                _capacitance = value;
            }
        }

        /// <summary>
        /// Branch (transformer) ratio's field
        /// </summary>
        private double _transformerRatio;

        /// <summary>
        /// Branch (transformer) ratio's property
        /// </summary>
        public double TransformerRatio
        {
            get => _transformerRatio;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckPositiveOrZero(value);
                ValueValidation.CheckOneOrLess(value);
                _transformerRatio = value;
            }
        }

        /// <summary>
        /// Branch class instance constructor
        /// </summary>
        /// <param name="elementStatus">Element status</param>
        /// <param name="branchType">Branch type</param>
        /// <param name="startNode">Start node</param>
        /// <param name="endNode">End node</param>
        /// <param name="parallelBranchNumber">Branch number</param>
        /// <param name="name">Name</param>
        /// <param name="resistance">Resistance</param>
        /// <param name="inductance">Inductance</param>
        /// <param name="capacitance">Capacitance</param>
        /// <param name="tranformerRatio">Transformer ratio</param>
        public Branch(ElementStatus elementStatus, 
                      BranchType branchType, 
                      Node startNode, 
                      Node endNode, 
                      int parallelBranchNumber, 
                      string name,
                      double resistance,
                      double inductance,
                      double capacitance,
                      double tranformerRatio)
        {
            ElementStatus = elementStatus;
            BranchType = branchType;
            StartNode = startNode;
            EndNode = endNode;
            ParallelBranchNumber = parallelBranchNumber;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            Capacitance = capacitance;
            TransformerRatio = tranformerRatio;
        }
    }
}

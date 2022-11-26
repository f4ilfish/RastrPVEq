using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Branch class
    /// </summary>
    public class Branch : ElementBase
    {
        /// <summary>
        /// Branch's index field
        /// </summary>
        private int _index;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override int Index
        {
            get => _index;
            set
            {
                ValueValidation.CheckIsNegative(value);
                _index = value;
            }
        }

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
                ValueValidation.CheckIsNegative(value);
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
                ValueValidation.CheckIsNegative(value);
                ValueValidation.CheckIsBiggerThanOne(value);
                _transformerRatio = value;
            }
        }

        /// <summary>
        /// Branch class instance constructor
        /// </summary>
        /// <param name="index">Branch index</param>
        /// <param name="elementStatus">Branch status</param>
        /// <param name="branchType">Branch type</param>
        /// <param name="name">Branch name</param>
        /// <param name="resistance">Branch resistance</param>
        /// <param name="inductance">Branch inductance</param>
        /// <param name="capacitance">Branch capacitance</param>
        /// <param name="tranformerRatio">Transformer ratio</param>
        public Branch(int index,
                      ElementStatus elementStatus,
                      BranchType branchType,
                      string name,
                      double resistance,
                      double inductance,
                      double capacitance,
                      double tranformerRatio)
        {
            Index = index;
            ElementStatus = elementStatus;
            BranchType = branchType;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            Capacitance = capacitance;
            TransformerRatio = tranformerRatio;
        }

        /// <summary>
        /// Branch class instance constructor
        /// </summary>
        /// <param name="index">Branch index</param>
        /// <param name="elementStatus">Branch status</param>
        /// <param name="branchType">Branch type</param>
        /// <param name="startNode">Branch start node</param>
        /// <param name="endNode">Branch dnd node</param>
        /// <param name="name">Branch name</param>
        /// <param name="resistance">Branch resistance</param>
        /// <param name="inductance">Branch inductance</param>
        /// <param name="capacitance">Branch capacitance</param>
        /// <param name="tranformerRatio">Transformer ratio</param>
        public Branch(int index,
                      ElementStatus elementStatus, 
                      BranchType branchType, 
                      Node startNode, 
                      Node endNode, 
                      string name,
                      double resistance,
                      double inductance,
                      double capacitance,
                      double tranformerRatio)
        {
            Index = index;
            ElementStatus = elementStatus;
            BranchType = branchType;
            StartNode = startNode;
            EndNode = endNode;
            Name = name;
            Resistance = resistance;
            Inductance = inductance;
            Capacitance = capacitance;
            TransformerRatio = tranformerRatio;
        }
    }
}

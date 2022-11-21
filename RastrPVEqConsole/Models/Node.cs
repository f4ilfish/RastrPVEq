using RastrPVEqConsole.Validation;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Node class
    /// </summary>
    public class Node : ElementBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override ElementStatus ElementStatus { get; set; }
        
        /// <summary>
        /// Node's type property
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        /// Node's number field
        /// </summary>
        private int _nodeNumber;

        /// <summary>
        /// Node's number property
        /// </summary>
        public int NodeNumber 
        { 
            get => _nodeNumber;
            set
            {
                ValueValidation.CheckPositive(value);
                _nodeNumber = value;
            }
        }

        /// <summary>
        /// Node's name field
        /// </summary>
        private string _name;

        /// <summary>
        /// Node's name property
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
        /// Node's rated voltage field
        /// </summary>
        private double _ratedVoltage;

        /// <summary>
        /// Node's rated voltage property
        /// </summary>
        public double RatedVoltage 
        { 
            get => _ratedVoltage;
            set 
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckPositive(value);
                _ratedVoltage = value;
            } 
        }

        /// <summary>
        /// Node's class instance constructor
        /// </summary>
        /// <param name="elementStatus">Element status</param>
        /// <param name="nodeType">Node type</param>
        /// <param name="nodeNumber">Node number</param>
        /// <param name="name">Name</param>
        /// <param name="ratedVoltage">Rated voltage</param>
        public Node(ElementStatus elementStatus, NodeType nodeType, int nodeNumber, string name, double ratedVoltage)
        {
            ElementStatus = elementStatus;
            NodeType = nodeType;
            NodeNumber = nodeNumber;
            Name = name;
            RatedVoltage = ratedVoltage;
        }
    }
}

using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Generator class
    /// </summary>
    public class Generator : ElementBase
    {
        /// <summary>
        /// Generator's index field
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
        /// Generator's number field
        /// </summary>
        private int _generatorNumber;

        /// <summary>
        /// Generator's number property
        /// </summary>
        public int GeneratorNumber 
        { 
            get => _generatorNumber; 
            set
            {
                ValueValidation.CheckIsNotPositive(value);
                _generatorNumber = value;
            }
        }

        /// <summary>
        /// Generator's name field
        /// </summary>
        private string _name;

        /// <summary>
        /// Generator's name property
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
        /// Generator's node
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Generator's maximum active power field
        /// </summary>
        private double _maxActivePower;

        /// <summary>
        /// Generator's maximum active power property
        /// </summary>
        public double MaxActivePower 
        { 
            get => _maxActivePower;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNotPositive(value);
                _maxActivePower = value;
            } 
        }

        /// <summary>
        /// Generator's PQ diagram
        /// </summary>
        public PQDiagram PQDiagram { get; set; }

        /// <summary>
        /// Generator class instance constructor
        /// </summary>
        /// <param name="index">Generator index</param>
        /// <param name="elementStatus">Generator status</param>
        /// <param name="generatorNumber">Generator number</param>
        /// <param name="name">Generator name</param>
        /// <param name="node">Generator node</param>
        /// <param name="maxActivePower">Generator max active power</param>
        /// <param name="pqDiagram">Generator PQ diagram</param>
        public Generator(int index,
                         ElementStatus elementStatus, 
                         int generatorNumber, 
                         string name, 
                         Node node, 
                         double maxActivePower, 
                         PQDiagram pqDiagram)
        {
            Index = index;
            ElementStatus = elementStatus;
            GeneratorNumber = generatorNumber;
            Name = name;
            Node = node;
            MaxActivePower = maxActivePower;
            PQDiagram = pqDiagram;
        }
    }
}

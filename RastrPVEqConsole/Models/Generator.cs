using RastrPVEqConsole.Validation;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Generator class
    /// </summary>
    public class Generator : ElementBase
    {
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
                ValueValidation.CheckPositive(value);
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
        private double _maxP;

        /// <summary>
        /// Generator's maximum active power property
        /// </summary>
        public double MaxP 
        { 
            get => _maxP;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckPositive(value);
                _maxP = value;
            } 
        }

        /// <summary>
        /// Generator's PQ diagram
        /// </summary>
        public PQDiagram PQDiagram { get; set; }

        /// <summary>
        /// Generator class instance constructor
        /// </summary>
        /// <param name="elementStatus"></param>
        /// <param name="generatorNumber"></param>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <param name="maxP"></param>
        /// <param name="pQDiagram"></param>
        public Generator(ElementStatus elementStatus, 
                        int generatorNumber, 
                        string name, 
                        Node node, 
                        double maxP, 
                        PQDiagram pQDiagram)
        {
            ElementStatus = elementStatus;
            GeneratorNumber = generatorNumber;
            Name = name;
            Node = node;
            MaxP = maxP;
            PQDiagram = pQDiagram;
        }
    }
}

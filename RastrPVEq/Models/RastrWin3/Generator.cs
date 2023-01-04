using RastrPVEq.Infrastructure;

namespace RastrPVEq.Models.RastrWin3
{
    /// <summary>
    /// Generator class
    /// </summary>
    public class Generator
    {
        /// <summary>
        /// Index field
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
                ValueValidation.IsZeroOrPositive(value);
                _index = value;
            }
        }

        /// <summary>
        /// Number
        /// </summary>
        private int _number;

        /// <summary>
        /// Gets number
        /// </summary>
        public int Number
        {
            get => _number;
            private set
            {
                ValueValidation.IsPositive(value);
                _number = value;
            }
        }

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
                ValueValidation.IsNotNullOrEmptyString(value);
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets generator node
        /// </summary>
        public Node? GeneratorNode { get; set; }

        /// <summary>
        /// Adjusted maximum active power
        /// </summary>
        private double _maxActivePower;

        /// <summary>
        /// Gets adjusted maximum active power
        /// </summary>
        public double MaxActivePower
        {
            get => _maxActivePower;
            private set
            {
                ValueValidation.IsNotNaN(value);
                ValueValidation.IsPositive(value);
                _maxActivePower = value;
            }
        }

        /// <summary>
        /// Gets or sets generator PQ diagram
        /// </summary>
        public PQDiagram? GeneratorPQDiagram { get; set; }

        /// <summary>
        /// Generator class instance constructor
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="generatorStatus">Status</param>
        /// <param name="number">Number</param>
        /// <param name="name">Name</param>
        /// <param name="maxActivePower">Adjusted max active power</param>
        public Generator(int index,
                         int number,
                         string name,
                         double maxActivePower)
        {
            Index = index;
            Number = number;
            Name = name;
            MaxActivePower = maxActivePower;
        }
    }
}

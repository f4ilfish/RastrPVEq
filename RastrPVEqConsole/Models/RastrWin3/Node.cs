using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models.RastrWin3
{
    /// <summary>
    /// Node class
    /// </summary>
    public class Node
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
        /// Gets node status
        /// </summary>
        public ElementStatus NodeStatus { get; private set; }

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
                ValueValidation.CheckIsNotPositive(value);
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
                ValueValidation.CheckNotNullOrEmptyString(value);
                _name = value;
            }
        }

        /// <summary>
        /// Rated voltage
        /// </summary>
        private double _ratedVoltage;

        /// <summary>
        /// Gets rated voltage
        /// </summary>
        public double RatedVoltage
        {
            get => _ratedVoltage;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNotPositive(value);
                _ratedVoltage = value;
            }
        }

        /// <summary>
        /// Node class instance constructor
        /// </summary>
        /// <param name="index">Node index</param>
        /// <param name="nodeStatus">Node status</param>
        /// <param name="number">Node number</param>
        /// <param name="name">Node name</param>
        /// <param name="ratedVoltage">Node rated voltage</param>
        public Node(int index,
                    ElementStatus nodeStatus,
                    int number,
                    string name,
                    double ratedVoltage)
        {
            Index = index;
            NodeStatus = nodeStatus;
            Number = number;
            Name = name;
            RatedVoltage = ratedVoltage;
        }
    }
}

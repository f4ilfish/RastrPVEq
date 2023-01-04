using RastrPVEq.Infrastructure;

namespace RastrPVEq.Models.RastrWin3
{
    /// <summary>
    /// Node class
    /// </summary>
    public class Node
    {
        // TODO: убрать Index / Status

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
                ValueValidation.IsNotNaN(value);
                ValueValidation.IsPositive(value);
                _ratedVoltage = value;
            }
        }

        /// <summary>
        /// District number
        /// </summary>
        private int _districtNumber;

        /// <summary>
        /// Gets district number
        /// </summary>
        public int DistrictNumber
        {
            get => _districtNumber;
            private set
            {
                ValueValidation.IsZeroOrPositive(value);
                _districtNumber = value;
            }
        }

        /// <summary>
        /// Territory number
        /// </summary>
        private int _territoryNumber;

        /// <summary>
        /// Gets territory number
        /// </summary>
        public int TerritoryNumber
        {
            get => _territoryNumber;
            private set
            {
                ValueValidation.IsZeroOrPositive(value);
                _territoryNumber = value;
            }
        }

        /// <summary>
        /// Node class instance constructor
        /// </summary>
        /// <param name="index">Node index</param>
        /// <param name="number">Node number</param>
        /// <param name="name">Node name</param>
        /// <param name="ratedVoltage">Node rated voltage</param>
        /// <param name="districtNumber">Node district number</param>
        /// <param name="territoryNumber">Node territory number</param>
        public Node(int index,
                    int number,
                    string name,
                    double ratedVoltage,
                    int districtNumber,
                    int territoryNumber)
        {
            Index = index;
            Number = number;
            Name = name;
            RatedVoltage = ratedVoltage;
            DistrictNumber = districtNumber;
            TerritoryNumber = territoryNumber;
        }
    }
}

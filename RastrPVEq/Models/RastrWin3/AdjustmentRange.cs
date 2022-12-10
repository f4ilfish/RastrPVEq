using System;
using RastrPVEq.Infrastructure;

namespace RastrPVEq.Models.RastrWin3
{
    /// <summary>
    /// Adjustment range class
    /// </summary>
    public class AdjustmentRange
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
        /// PQ diagram number
        /// </summary>
        private int _pqDiagramNumber;

        /// <summary>
        /// Gets PQ diagram number
        /// </summary>
        public int PQDiagramNumber
        {
            get => _pqDiagramNumber;
            private set
            {
                ValueValidation.CheckIsNotPositive(value);
                _pqDiagramNumber = value;
            }
        }

        /// <summary>
        /// Active power
        /// </summary>
        private double _activePower;

        /// <summary>
        /// Gets active power
        /// </summary>
        public double ActivePower
        {
            get => _activePower;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNegative(value);
                _activePower = value;
            }
        }

        /// <summary>
        /// Adjusted minimum of reactive power
        /// </summary>
        private double _minReactivePower;

        /// <summary>
        /// Gets adjusted minimum of reactive power
        /// </summary>
        public double MinReactivePower
        {
            get => _minReactivePower;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                _minReactivePower = value;
            }
        }

        /// <summary>
        /// Adjusted maximum of reactive power
        /// </summary>
        private double _maxReactivePower;

        /// <summary>
        /// Gets adjusted maximum of reactive power
        /// </summary>
        public double MaxReactivePower
        {
            get => _maxReactivePower;
            private set
            {
                ValueValidation.CheckNotNaN(value);
                _maxReactivePower = value;
            }
        }

        /// <summary>
        /// Adjustment range class instance constructor
        /// </summary>
        /// <param name="activePower">Active power</param>
        /// <param name="pqDiagramNumber">PQ diagram number</param>
        /// <param name="minReactivePower">Adjusted minimum reactive power</param>
        /// <param name="maxReactivePower">Adjusted reactive power</param>
        public AdjustmentRange(int index,
                               int pqDiagramNumber,
                               double activePower,
                               double minReactivePower,
                               double maxReactivePower)
        {
            if (maxReactivePower < minReactivePower)
                throw new ArgumentException("Max reactive power " +
                                            "must be greater " +
                                            "than min reactive power");

            Index = index;
            PQDiagramNumber = pqDiagramNumber;
            ActivePower = activePower;
            MinReactivePower = minReactivePower;
            MaxReactivePower = maxReactivePower;
        }
    }
}

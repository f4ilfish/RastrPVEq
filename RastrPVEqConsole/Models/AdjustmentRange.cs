using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Adjustment range
    /// </summary>
    public class AdjustmentRange
    {
        /// <summary>
        /// Adjustment range's index field
        /// </summary>
        private int _index;

        /// <summary>
        /// Adjustment range's index property
        /// </summary>
        public int Index
        {
            get => _index;
            set
            {
                ValueValidation.CheckIsNegative(value);
                _index = value;
            }
        }

        /// <summary>
        /// AdjustmentRange's diagram number field
        /// </summary>
        private int _diagramNumber;

        /// <summary>
        /// AdjustmentRange's diagram number property
        /// </summary>
        public int DiagramNumber
        {
            get => _diagramNumber;
            set
            {
                ValueValidation.CheckIsNotPositive(value);
                _diagramNumber = value;
            }
        }

        /// <summary>
        /// Active power's field
        /// </summary>
        private double _activePower;

        /// <summary>
        /// Active power's property
        /// </summary>
        public double ActivePower 
        { 
            get => _activePower;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckIsNegative(value);
                _activePower = value;
            } 
        }

        /// <summary>
        /// Adjusted minimum of reactive power for active power field
        /// </summary>
        private double _minReactivePower;

        /// <summary>
        /// Adjusted minimum of reactive power for active power property
        /// </summary>
        public double MinReactivePower 
        { 
            get => _minReactivePower;
            set
            {
                ValueValidation.CheckNotNaN(value);
                _minReactivePower = value;
            } 
        }

        /// <summary>
        /// Adjusted maximum of reactive power for active power field
        /// </summary>
        private double _maxReactivePower;

        /// <summary>
        /// Adjusted maximum of reactive power for active power property
        /// </summary>
        public double MaxReactivePower 
        { 
            get => _maxReactivePower;
            set
            {
                ValueValidation.CheckNotNaN(value);
                
                if (value < MinReactivePower) throw new ArgumentException("Max reactive power " +
                                                                            "must be greater " +
                                                                            "than min reactive power");
         
                _maxReactivePower = value;
            } 
        }

        /// <summary>
        /// AdjustmentRange's class instance constructor
        /// </summary>
        /// <param name="activePower">Active power</param>
        /// <param name="diagramNumber">AdjustmentRange's diagram number</param>
        /// <param name="minReactivePower">Minimum reactive power</param>
        /// <param name="maxReactivePower">Maximum reactive power</param>
        public AdjustmentRange(int index,
                               int diagramNumber,
                               double activePower,
                               double minReactivePower,
                               double maxReactivePower)
        {
            Index = index;
            DiagramNumber = diagramNumber;
            ActivePower = activePower;
            MinReactivePower = minReactivePower; 
            MaxReactivePower = maxReactivePower; 
        }
    }
}

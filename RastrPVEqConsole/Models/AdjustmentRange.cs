using RastrPVEqConsole.Validation;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Adjustment range
    /// </summary>
    public class AdjustmentRange
    {
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
                ValueValidation.CheckPositiveOrZero(value);
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
                ValueValidation.CheckNegativeOrZero(value);
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
        public double MaxQ 
        { 
            get => _maxReactivePower;
            set
            {
                ValueValidation.CheckNotNaN(value);
                ValueValidation.CheckPositiveOrZero(value);
                _maxReactivePower = value;
            } 
        }

        /// <summary>
        /// AdjustmentRange's class instance constructor
        /// </summary>
        /// <param name="activePower">Active power</param>
        /// <param name="minReactivePower">Minimum reactive power</param>
        /// <param name="maxReactivePower">Maximum reactive power</param>
        public AdjustmentRange(double activePower, double minReactivePower, double maxReactivePower)
        {
            ActivePower = activePower;
            MinReactivePower = minReactivePower; 
            MaxQ = maxReactivePower; 
        }
    }
}

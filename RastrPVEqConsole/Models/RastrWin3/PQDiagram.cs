using RastrPVEqConsole.Infrastructure;

namespace RastrPVEqConsole.Models.RastrWin3
{
    /// <summary>
    /// PQ diagram class
    /// </summary>
    public class PQDiagram
    {
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
        /// Gets or sets list of adjustment ranges
        /// </summary>
        public List<AdjustmentRange> AdjustmentRanges { get; private set; }

        /// <summary>
        /// PQ diagram class instance constructor
        /// </summary>
        /// <param name="number">PQ diagram number</param>
        /// <param name="adjustmentRanges">List of adjustment ranges</param>
        public PQDiagram(int number, List<AdjustmentRange> adjustmentRanges)
        {
            Number = number;
            AdjustmentRanges = adjustmentRanges;
        }
    }
}

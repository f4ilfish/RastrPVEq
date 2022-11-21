using RastrPVEqConsole.Validation;

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// PQ diagram class
    /// </summary>
    public class PQDiagram
    {
        /// <summary>
        /// Diagram's number field
        /// </summary>
        private int _digramNumber;

        /// <summary>
        /// Diagram's number property
        /// </summary>
        public int DigramNumber 
        { 
            get => _digramNumber;
            set 
            {
                ValueValidation.CheckPositive(value);
                _digramNumber = value;
            } 
        }
        
        /// <summary>
        /// Total adjustment ranges
        /// </summary>
        public List<AdjustmentRange> AdjustmentRanges { get; set; }

        /// <summary>
        /// PQ diagram class instance constructor
        /// </summary>
        /// <param name="diagramNumber">Diagram number</param>
        /// <param name="adjustmentRanges">Adjustment ranges</param>
        public PQDiagram(int diagramNumber, List<AdjustmentRange> adjustmentRanges)
        {
            DigramNumber = diagramNumber;
            AdjustmentRanges = adjustmentRanges;
        }
    }
}

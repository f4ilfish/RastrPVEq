﻿namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Branch types
    /// </summary>
    public enum BranchType
    {
        /// <summary>
        /// Power line
        /// </summary>
        Line,
        
        /// <summary>
        /// Power tranformer
        /// </summary>
        Transformer,

        /// <summary>
        /// Switch (zero impedance)
        /// </summary>
        Switch
    }
}

namespace RastrPVEqConsole.Models
{
    /// <summary>
    /// Abstract class of model's element
    /// </summary>
    public abstract class ElementBase
    {
        /// <summary>
        /// Element's index property
        /// </summary>
        public abstract int Index { get; set; }

        /// <summary>
        /// Element's status property
        /// </summary>
        public abstract ElementStatus ElementStatus { get; set; }

        /// <summary>
        /// Element's name property
        /// </summary>
        public abstract string Name { get; set; }
    }
}

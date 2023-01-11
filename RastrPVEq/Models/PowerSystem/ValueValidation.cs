using System;

namespace RastrPVEq.Models.PowerSystem
{
    /// <summary>
    /// Value validation class
    /// </summary>
    public static class ValueValidation
    {
        /// <summary>
        /// Is not NaN constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNotNaN(double value)
        {
            if (double.IsNaN(value)) throw new ArgumentException($"Value must be not NaN");
        }

        /// <summary>
        /// Is positive constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsPositive(double value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                  "Value must be positive");
        }

        /// <summary>
        /// Is positive consraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsPositive(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                  "Value must positive");
        }

        /// <summary>
        /// Is zero or positive constraint
        /// </summary>
        /// <param name="value">Input value</param>
        public static void IsZeroOrPositive(double value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Value must be zero or positive");
        }

        /// <summary>
        /// Is zero or positive constraint
        /// </summary>
        /// <param name="value">Input value</param>
        public static void IsZeroOrPositive(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Value must be zero or positive");
        }

        /// <summary>
        /// Is null or empty string constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNotNullOrEmptyString(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException($"Expected not null or empty string");
        }

        /// <summary>
        /// Is one or less constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void IsOneOrLess(double value)
        {
            if (value > 1) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Value must be 1 or less");
        }
    }
}

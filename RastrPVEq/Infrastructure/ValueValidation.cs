using System;

namespace RastrPVEq.Infrastructure
{
    /// <summary>
    /// ValueValidation class
    /// </summary>
    public static class ValueValidation
    {
        /// <summary>
        /// Check non NaN constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentException"></exception>
        public static void CheckNotNaN(double value)
        {
            if (double.IsNaN(value)) throw new ArgumentException("Expected not NaN value");
        }

        /// <summary>
        /// Check is not positive constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckIsNotPositive(double value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                  "Value is not positive. Expected positive");
        }

        /// <summary>
        /// Check is not positive constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckIsNotPositive(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                  "Value is not positive. Expected positive");
        }

        /// <summary>
        /// Check is negative
        /// </summary>
        /// <param name="value">Input value</param>
        public static void CheckIsNegative(double value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Value is negative. Expected not negative");
        }

        /// <summary>
        /// Check is positive
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckIsPositive(double value)
        {
            if (value > 0) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Value is positive. Expected not positive");
        }

        /// <summary>
        /// Check not null or empty string
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentException"></exception>
        public static void CheckNotNullOrEmptyString(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException($"Expected not null or empty string");
        }

        /// <summary>
        /// Check one or less double
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckIsBiggerThanOne(double value)
        {
            if (value > 1) throw new ArgumentOutOfRangeException(nameof(value),
                                                                 "Expected [0...1] value");
        }
    }
}

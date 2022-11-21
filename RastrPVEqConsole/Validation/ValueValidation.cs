namespace RastrPVEqConsole.Validation
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
        /// Check positive double constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckPositive(double value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "Expected positive double");
        }

        /// <summary>
        /// Check positive int constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckPositive(int value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "Expected positive int");
        }

        /// <summary>
        /// Check positive or zero double constraint
        /// </summary>
        /// <param name="value">Input value</param>
        public static void CheckPositiveOrZero(double value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Expected positive or zero double");
        }

        /// <summary>
        /// Check negative or zero double constraint
        /// </summary>
        /// <param name="value">Input value</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void CheckNegativeOrZero(double value)
        {
            if (value > 0) throw new ArgumentOutOfRangeException(nameof(value), "Expected negative or zero double");
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
        public static void CheckOneOrLess(double value)
        {
            if (value >= 1) throw new ArgumentOutOfRangeException(nameof(value), "Expected 1 or less double");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj">Input object</param>
        ///// <exception cref="ArgumentNullException"></exception>
        //protected static void CheckNotNull(object obj)
        //{
        //    if (obj == null) throw new ArgumentNullException("Expected not null object");
        //}
    }
}

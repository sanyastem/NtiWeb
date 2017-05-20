using System;

namespace ASUVP.Core.Diagnostics
{
    /// <summary>
    ///     Contract verifier. Should be used instead of native <see cref="System.Diagnostics.Contracts.Contract" />
    ///     as it impacts compile time significantly.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        ///     Throws <see cref="ArgumentNullException" /> if <paramref name="o" /> is null.
        /// </summary>
        public static void RequiresParameterNotNull(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }
        }

        /// <summary>
        ///     Throws <see cref="ArgumentNullException" /> if <paramref name="o" /> is null.
        /// </summary>
        public static void RequiresParameterNotNull(object o, string paramName)
        {
            if (o == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        ///     Throws <see cref="ArgumentException" /> if <paramref name="o" /> is null.
        /// </summary>
        public static void RequiresNotNull(object o, string message = null, string paramName = null)
        {
            if (o == null)
            {
                throw new ArgumentException(message, paramName);
            }
        }

        /// <summary>
        ///     Throws <see cref="ArgumentException" /> if <paramref name="o" /> is null.
        /// </summary>
        public static void RequiresNotEmptyString(string o, string paramName = null)
        {
            if (string.IsNullOrWhiteSpace(o))
            {
                throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        ///     Verifies <paramref name="condition" /> is true.
        ///     If it's not throws <see cref="ArgumentException" /> with <paramref name="errorMessage" />.
        /// </summary>
        public static void ExpectArgument(bool condition, string errorMessage)
        {
            if (!condition)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        /// <summary>
        ///     Verifies <paramref name="condition" /> is true.
        ///     If it's not throws <see cref="InvalidOperationException" /> with <paramref name="errorMessage" />.
        /// </summary>
        public static void Expect(bool condition, string errorMessage)
        {
            if (!condition)
            {
                throw new InvalidOperationException(errorMessage);
            }
        }

        /// <summary>
        ///     Verifies <paramref name="condition" /> is true.
        ///     If it's not throws
        ///     <typeparam name="TException" />
        ///     Exception.
        /// </summary>
        public static void Expect<TException>(bool condition) where TException : Exception, new()
        {
            if (!condition)
            {
                throw new TException();
            }
        }
    }
}
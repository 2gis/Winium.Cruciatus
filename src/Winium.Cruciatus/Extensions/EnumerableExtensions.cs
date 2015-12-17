namespace Winium.Cruciatus.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions methods related to <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Wraps this <paramref name="item"/> instance into a <see cref="IEnumerable{T}"/>  consisting of a single item.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <paramref name="item"/>.</typeparam>
        /// <param name="item">The instance of <typeparamref name="T"/> that will be wrapped.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> consisting of the single <paramref name="item"/>, or an empty collection if <paramref name="item"/> is <c>null</c>.</returns>
        public static IEnumerable<T> AsSingletonEnumerable<T>(this T item)
        {
            if (item == null)
            {
                yield break;
            }

            yield return item;
        }
    }
}
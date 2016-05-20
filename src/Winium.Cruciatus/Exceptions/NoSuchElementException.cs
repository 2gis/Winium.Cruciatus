namespace Winium.Cruciatus.Exceptions
{
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Элемент не может быть найден.
    /// </summary>
    [Serializable]
    public class NoSuchElementException : CruciatusException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public NoSuchElementException()
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        public NoSuchElementException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        public NoSuchElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        protected NoSuchElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}

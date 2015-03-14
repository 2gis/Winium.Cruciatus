namespace Winium.Cruciatus.Exceptions
{
    #region using

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// Исключение фреймворка Cruciatus.
    /// </summary>
    [Serializable]
    public class CruciatusException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public CruciatusException()
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        public CruciatusException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        public CruciatusException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Параметризованный конструктор.
        /// </summary>
        protected CruciatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}

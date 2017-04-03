using System;
using System.Runtime.Serialization;

namespace Palmmedia.BackUp.Synchronization.FileSearch
{
    /// <summary>
    /// Occurs when ReparsePoint could not be created.
    /// </summary>
    [Serializable]
    public class ReparsePointException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReparsePointException" /> class.
        /// </summary>
        public ReparsePointException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReparsePointException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ReparsePointException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReparsePointException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner <see cref="Exception"/></param>
        public ReparsePointException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReparsePointException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/>.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/>.</param>
        protected ReparsePointException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
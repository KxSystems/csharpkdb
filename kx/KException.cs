using System;
using System.Runtime.Serialization;

namespace kx
{
    /// <summary>
    /// An exception that is thrown when in an error occurs in the <see cref="kx.c"/> class.
    /// </summary>
    [Serializable]
    public class KException : Exception
    {
        /// <summary>
        /// Initialises a new default instance of <see cref="KException"/>.
        /// </summary>
        public KException()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of <see cref="KException"/> with a specified 
        /// error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public KException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of <see cref="KException"/> with a specified 
        /// error message and the excception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public KException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of <see cref="KException"/> with serialised data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialised data of the exception.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected KException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

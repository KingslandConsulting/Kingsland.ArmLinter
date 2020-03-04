using System;
using System.Runtime.Serialization;

namespace Kingsland.ArmLinter
{
    [Serializable]
    internal class InvalidOperationExcpetion : Exception
    {
        public InvalidOperationExcpetion()
        {
        }

        public InvalidOperationExcpetion(string message) : base(message)
        {
        }

        public InvalidOperationExcpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidOperationExcpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
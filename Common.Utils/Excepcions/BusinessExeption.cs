namespace Common.Utils.Excepcions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class BusinessExeption : Exception
    {
        public BusinessExeption() : base()
        {
        }

        public BusinessExeption(string message) : base(message)
        {
        }

        public BusinessExeption(string message, Exception inner) : base(message, inner)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected BusinessExeption(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
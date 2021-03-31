using System;

namespace MRS.Common.Exceptions
{
    public class CommandParseException : Exception
    {
        public CommandParseException(string message) : base(message)
        {
        }
    }
}

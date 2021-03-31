using System;

namespace MRS.Common.Exceptions
{
    public class StartingPointOutOfRangeException : Exception
    {
        public StartingPointOutOfRangeException(string message) : base(message)
        {
        }
    }
}

using MRS.Common.Exceptions;
using System;
using System.Linq;

namespace MRS.Common.Helpers
{
    public static class UnsupportedCharactersHelper
    {
        public static void ValidateUnsupportedCharacters(this string value, Type type)
        {
            var inputSymbols = value.Trim().ToCharArray();
            var invalid = inputSymbols.Any(rs => !char.IsLetter(rs));

            if (invalid && type.Name == typeof(CommandParseException).Name)
                throw new CommandParseException($"Commands were not parsed correctly.");

            if (invalid && type.Name == typeof(StartingPointParseException).Name)
                throw new StartingPointParseException($"Starting point was not parsed correctly.");

            if (invalid)
            {
                throw new ArgumentException("Cannot parse characters");
            }
        }
    }
}

using MRS.Business.Models;
using System.Collections.Generic;

using Enums = MRS.Common.Enums;

namespace MRS.Services.Interfaces
{
    public interface ILocationService
    {
        IEnumerable<Enums.CommandType> ParseCommands(string routeCommands);

        Position ParseStartPoint(Plateau plateau, string startingPoint);

        Position ExecuteCommands(Plateau plateau, Position startingPoint, IEnumerable<Enums.CommandType> commands);
    }
}
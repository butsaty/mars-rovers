using System.Collections.Generic;
using MRS.Business.Models;
using MRS.Services.Interfaces;

using Enums = MRS.Common.Enums;

namespace MRS
{
    public class Rover : IRover
    {
        private readonly ILocationService _locationService;

        public Rover(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public Position CurrentPosition { get; set; }

        public Plateau Plateau { get; set; }

        public IEnumerable<Enums.CommandType> Commands { get; set; }

        public void Move()
        {
            CurrentPosition = _locationService.ExecuteCommands(Plateau, CurrentPosition, Commands);
            Commands = null;
        }
    }
}

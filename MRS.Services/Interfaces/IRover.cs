using MRS.Business.Models;
using System.Collections.Generic;

using Enums = MRS.Common.Enums;

namespace MRS.Services.Interfaces
{
    public interface IRover
    {
        Position CurrentPosition { get; set; }

        Plateau Plateau { set; }

        IEnumerable<Enums.CommandType> Commands { set; }

        void Move();
    }
}
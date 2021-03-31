using Xunit;
using MRS.Business.Models;
using MRS.Common.Exceptions;
using MRS.Services.Interfaces;

using Enums = MRS.Common.Enums;

namespace MRS.Services.Tests
{
    public class LocationServiceTests
    {
        private readonly ILocationService _locationService = new LocationService();
        private readonly Plateau _pateau = new Plateau { Width = 11, Height = 11 };

        [Theory]
        [InlineData("0 0 N", 0, 0, Enums.Direction.N)]
        [InlineData("0 1 S", 0, 1, Enums.Direction.S)]
        [InlineData("1 0 W", 1, 0, Enums.Direction.W)]
        [InlineData("1 1 E", 1, 1, Enums.Direction.E)]
        [InlineData("3 3 S", 3, 3, Enums.Direction.S)]
        [InlineData("5 5 E", 5, 5, Enums.Direction.E)]
        [InlineData("10 10 W", 10, 10, Enums.Direction.W)]
        [InlineData("1 2 N", 1, 2, Enums.Direction.N)]
        [InlineData("2 1 S", 2, 1, Enums.Direction.S)]
        [InlineData("3 5 E", 3, 5, Enums.Direction.E)]
        [InlineData("4 7 W", 4, 7, Enums.Direction.W)]
        public void CreateRouteStartingPoint_ConsoleInput_StartingPointHasBeenCreated(string startingPointParameters, int expectedX, int expectedY, Enums.Direction expectedDirection)
        {
            var startingPoint = _locationService.ParseStartPoint(_pateau, startingPointParameters);

            Assert.Equal(expectedX, startingPoint.X);
            Assert.Equal(expectedY, startingPoint.Y);
            Assert.Equal(expectedDirection, startingPoint.Direction);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1 ")]
        [InlineData(" 1")]
        [InlineData("1 1")]
        [InlineData("1 1 3")]
        [InlineData("1 1 N1")]
        [InlineData("3 3 S2")]
        [InlineData("5 5 WA1")]
        [InlineData("10 10 EE")]
        [InlineData("1 2 NY")]
        [InlineData("2 1 SR")]
        [InlineData("3 5 EE")]
        [InlineData("4 7 WW")]
        [InlineData("0 M N")]
        [InlineData("A -1 N")]
        [InlineData("-1.0 0.0 N")]
        [InlineData("-1 1D N")]
        [InlineData("A B C")]
        [InlineData("ABC")]
        [InlineData("a b c")]
        [InlineData("abc")]
        [InlineData("a bc d")]
        public void CreateRouteStartingPoint_ConsoleInput_StartingPointHasNotBeenCreated(string startingPointParameters)
        {
            Position CreateRouteStartingPoint() => _locationService.ParseStartPoint(_pateau, startingPointParameters);

            Assert.Throws<StartingPointParseException>(CreateRouteStartingPoint);
        }
    }
}




using MRS.Common.Enums;
using MRS.Common.Exceptions;
using MRS.Services.Interfaces;
using Xunit;

namespace MRS.Services.Tests
{
    public class RoverServiceTests
    {
        private IMapService _mapService = new MapService();
        private ILocationService _locationService = new LocationService();

        [Theory]
        [InlineData("5 5", "1 1 N", "MMMM", 1, 5, Direction.N)]
        [InlineData("5 5", "1 5 N", "BBBB", 1, 1, Direction.N)]
        [InlineData("5 5", "5 1 N", "LLLL", 5, 1, Direction.N)]
        [InlineData("5 5", "1 1 N", "RRRR", 1, 1, Direction.N)]
        [InlineData("5 5", "0 0 N", "MRMLMRM", 2, 2, Direction.E)]
        public void MoveTheRoverByTheSpecifiedRoute_RoverHasBeenMovedAndDidNotCrossBounderes(string map, string startingPoint, string commands, int expectedX, int expectedY, Direction expectedDirection)
        {
            // Arrange
            var plateau = _mapService.ParseMap(map);

            IRoverService rover = new RoverService(_locationService)
            {
                Plateau = plateau,
                CurrentPosition = _locationService.ParseStartPoint(plateau, startingPoint),
                Commands = _locationService.ParseCommands(commands)
            };

            // Act
            rover.Move();
            var currentPos = rover.CurrentPosition;

            // Assert
            Assert.NotNull(currentPos);
            Assert.Equal(expectedX, currentPos.X);
            Assert.Equal(expectedY, currentPos.Y);
            Assert.Equal(expectedDirection, currentPos.Direction);
        }

        [Theory]
        [InlineData("5 5", "0 0 N", "B")]
        [InlineData("5 5", "0 0 N", "LM")]
        [InlineData("5 5", "5 1 N", "RM")]
        [InlineData("5 5", "1 5 N", "M")]
        public void MoveTheRoverByTheSpecifiedRoute_RoverHasBeenMovedAndCrossBoundaries(string map, string startingPoint, string commands)
        {
            var plateau = _mapService.ParseMap(map);

            IRoverService rover = new RoverService(_locationService)
            {
                Plateau = plateau,
                CurrentPosition = _locationService.ParseStartPoint(plateau, startingPoint),
                Commands = _locationService.ParseCommands(commands)
            };

            Assert.Throws<EndPointOutOfMapException>(() => rover.Move());
        }

        [Theory]
        [InlineData("5 5", "1 2 N", "LMLMLMLMM", 1, 3, Direction.N)]
        [InlineData("5 5", "3 3 E", "MMRMMRMRRM", 5, 1, Direction.E)]
        [InlineData("3 3", "0 0 S", "LMMLM", 2, 1, Direction.N)]
        //[InlineData("3 3", "1 2 W", "LMLMRM", 1, 0, Direction.S)] // From example - expected result looks wrong
        public void MoveTheRoverByTheSpecifiedRoute_FromExample_RoverHasBeenMovedAndDidNotCrossBounderes(string map, string startingPoint, string commands, int expectedX, int expectedY, Direction expectedDirection)
        {
            // Arrange
            var plateau = _mapService.ParseMap(map);

            IRoverService rover = new RoverService(_locationService)
            {
                Plateau = plateau,
                CurrentPosition = _locationService.ParseStartPoint(plateau, startingPoint),
                Commands = _locationService.ParseCommands(commands)
            };

            // Act
            rover.Move();
            var currentPos = rover.CurrentPosition;

            // Assert
            Assert.NotNull(currentPos);
            Assert.Equal(expectedX, currentPos.X);
            Assert.Equal(expectedY, currentPos.Y);
            Assert.Equal(expectedDirection, currentPos.Direction);
        }
    }
}

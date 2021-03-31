using MRS.Business.Models;
using MRS.Common.Exceptions;
using MRS.Services.Interfaces;
using Xunit;

namespace MRS.Services.Tests
{
    public class MapServiceTests
    {
        private readonly IMapService _mapService = new MapService();

        [Theory]
        [InlineData("2 2", 2, 2)]
        [InlineData("3 3", 3, 3)]
        [InlineData("5 5", 5, 5)]
        [InlineData("10 10", 10, 10)]
        [InlineData("2 3", 2, 3)]
        [InlineData("4 5", 4, 5)]
        [InlineData("3 5", 3, 5)]
        [InlineData("4 7", 4, 7)]
        public void CreateMap_ConsoleInput_MapHasBeenCreated(string mapParameters, int expectedWidth, int expectedLength)
        {
            var pateau = _mapService.ParseMap(mapParameters);

            Assert.Equal(expectedWidth, pateau.Width);
            Assert.Equal(expectedLength, pateau.Height);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1 ")]
        [InlineData(" 1")]
        [InlineData("-1 M")]
        [InlineData("A -1")]
        [InlineData("1 0.0")]
        [InlineData("-1 1D")]
        [InlineData("1 2 3")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("a b")]
        public void CreateMap_ConsoleInput_MapHasNotBeenCreated(string mapParameters)
        {
            Plateau CreateMap() => _mapService.ParseMap(mapParameters);

            Assert.Throws<MapParametersParseException>(CreateMap);
        }

        [Theory]
        [InlineData("0 0")]
        [InlineData("0 1")]
        [InlineData("1 0")]
        [InlineData("1 1")]
        [InlineData("0 -1")]
        [InlineData("-1 0")]
        [InlineData("-1 -1")]
        [InlineData("-3 5")]
        [InlineData("-5 3")]
        public void CreateMap_ConsoleInput_MapHasNotBeenCreated_OutOfRange(string mapParameters)
        {
            Plateau CreateMap() => _mapService.ParseMap(mapParameters);

            Assert.Throws<MapParametersOutOfRangeException>(CreateMap);
        }
    }
}
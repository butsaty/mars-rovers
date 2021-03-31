using MRS.Business.Models;
using MRS.Common.Exceptions;
using MRS.Services.Interfaces;

using Enums = MRS.Common.Enums;

namespace MRS.Services
{
    public class MapService : IMapService
    {
        private const int _minWidth = 2;
        private const int _minHeigth = 2;

        public Plateau ParseMap(string parameters)
        {
            var areaArr = parameters?.Trim()?.Split(' ');

            if (areaArr?.Length != 2)
                throw new MapParametersParseException($"Map parameters were not properly specified.");

            TryParseArea(areaArr, Enums.AreaType.Width, out var width);
            TryParseArea(areaArr, Enums.AreaType.Height, out var height);

            ValidateMapParameters(width, height);

            var map = new Plateau
            {
                Width = width,
                Height = height
            };

            return map;
        }

        private bool TryParseArea(string[] input, Enums.AreaType areaType, out int value)
        {
            var param = input[(int)areaType];
            var isParsed = int.TryParse(param, out value);

            if (!isParsed)
                throw new MapParametersParseException($"The area parameter '{areaType}' was not parsed correctly.");

            return true;
        }

        private void ValidateMapParameters(int width, int length)
        {
            if (width < _minWidth)
                throw new MapParametersOutOfRangeException($"Plateau's width must be greater or equal {_minWidth}");

            if (length < _minHeigth)
                throw new MapParametersOutOfRangeException($"Plateau's height must be greater or equal {_minHeigth}");
        }
    }
}

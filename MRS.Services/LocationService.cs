using System;
using System.Collections.Generic;
using MRS.Business.Models;
using MRS.Common.Exceptions;
using MRS.Common.Helpers;
using MRS.Services.Interfaces;

using Enums = MRS.Common.Enums;

namespace MRS.Services
{
    public class LocationService : ILocationService
    {
        public Position ParseStartPoint(Plateau plateau, string parameters)
        {
            var startingPoint = ParseStartPointParams(parameters);
            var position = GetStartPoint(plateau, startingPoint.X, startingPoint.Y, startingPoint.Direction);

            return position;
        }

        public IEnumerable<Enums.CommandType> ParseCommands(string commandParams)
        {
            if (string.IsNullOrWhiteSpace(commandParams))
                throw new CommandParseException("Command was not specified.");

            var commands = new List<Enums.CommandType>();
            var commandStrArr = commandParams.Trim().ToCharArray();

            commandParams.ValidateUnsupportedCharacters(typeof(CommandParseException));

            foreach (var cmdStr in commandStrArr)
            {
                var cmd = cmdStr.ToString();
                var isParsed = Enum.TryParse(typeof(Enums.CommandType), cmd, true, out var command);

                if (!isParsed)
                {
                    throw new CommandParseException($"Unknown command '{cmd}'");
                }

                commands.Add((Enums.CommandType)command);
            }

            return commands;
        }

        private Position GetStartPoint(Plateau pateau, int x, int y, Enums.Direction direction)
        {
            ValidateStartPoint(pateau, x, y);

            var startingPoint = new Position
            {
                X = x,
                Y = y,
                Direction = direction
            };

            return startingPoint;
        }

        public Position ExecuteCommands(Plateau pateau, Position startPosition, IEnumerable<Enums.CommandType> commands)
        {
            Position nextPosition = startPosition;

            foreach (var command in commands)
            {
                nextPosition = GetNextPosition(nextPosition, command);

#if DEBUG
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"cmd {command}: { nextPosition.X } { nextPosition.Y } { nextPosition.Direction }");
                Console.ResetColor();
#endif
                ValidateBoundaries(pateau, nextPosition);
            }

            return nextPosition;
        }

        private Position ParseStartPointParams(string startPoints)
        {
            if (string.IsNullOrWhiteSpace(startPoints))
                throw new StartingPointParseException($"The starting point was not specified.");

            var startPositionStrArr = startPoints.Trim().Split(' ');

            if (startPositionStrArr.Length != Enum.GetNames(typeof(Enums.PositionType)).Length)
                throw new StartingPointParseException($"The starting point was not properly specified.");

            var position = new Position
            {
                X = ParseStartingPosition(startPositionStrArr, Enums.PositionType.X),
                Y = ParseStartingPosition(startPositionStrArr, Enums.PositionType.Y),
                Direction = (Enums.Direction)ParseStartingPosition(startPositionStrArr, Enums.PositionType.Direction)
            };

            return position;
        }

        private int ParseStartingPosition(string[] input, Enums.PositionType positionType)
        {
            object direction = null;
            var value = 0;
            var param = input[(int)positionType];

            if (positionType == Enums.PositionType.Direction)
                param.ValidateUnsupportedCharacters(typeof(StartingPointParseException));

            var isParsed = positionType == Enums.PositionType.Direction
                ? Enum.TryParse(typeof(Enums.Direction), param, true, out direction)
                : int.TryParse(param, out value);

            if (!isParsed)
                throw new StartingPointParseException($"The position type '{positionType}' was not parsed correctly.");

            if (positionType == Enums.PositionType.Direction && direction != null)
                return (int)direction;

            return value;
        }

        private Position GetNextPosition(Position currentPosition, Enums.CommandType command)
        {
            Position nextPosition = null;

            switch (command)
            {
                case Enums.CommandType.M:
                    {
                        nextPosition = MoveForward(currentPosition);
                        break;
                    }

                case Enums.CommandType.B:
                    {
                        nextPosition = MoveBackward(currentPosition);
                        break;
                    }

                case Enums.CommandType.L:
                    {
                        nextPosition = TurnLeft(currentPosition);
                        break;
                    }

                case Enums.CommandType.R:
                    {
                        nextPosition = TurnRight(currentPosition);
                        break;
                    }
            }

            return nextPosition;
        }

        private Position TurnRight(Position currentPosition)
        {
            var nextPosition = new Position
            {
                X = currentPosition.X,
                Y = currentPosition.Y
            };

            switch (currentPosition.Direction)
            {
                case Enums.Direction.N:
                    {
                        nextPosition.Direction = Enums.Direction.E;
                        break;
                    }

                case Enums.Direction.S:
                    {
                        nextPosition.Direction = Enums.Direction.W;
                        break;
                    }

                case Enums.Direction.W:
                    {
                        nextPosition.Direction = Enums.Direction.N;
                        break;
                    }

                case Enums.Direction.E:
                    {
                        nextPosition.Direction = Enums.Direction.S;
                        break;
                    }
            }

            return nextPosition;
        }

        private Position TurnLeft(Position currentPosition)
        {
            var nextPosition = new Position
            {
                X = currentPosition.X,
                Y = currentPosition.Y
            };

            switch (currentPosition.Direction)
            {
                case Enums.Direction.N:
                    {
                        nextPosition.Direction = Enums.Direction.W;
                        break;
                    }

                case Enums.Direction.S:
                    {
                        nextPosition.Direction = Enums.Direction.E;
                        break;
                    }

                case Enums.Direction.W:
                    {
                        nextPosition.Direction = Enums.Direction.S;
                        break;
                    }

                case Enums.Direction.E:
                    {
                        nextPosition.Direction = Enums.Direction.N;
                        break;
                    }
            }

            return nextPosition;
        }

        private Position MoveBackward(Position currentPosition)
        {
            var nextPosition = new Position
            {
                X = currentPosition.X,
                Y = currentPosition.Y,
                Direction = currentPosition.Direction
            };

            switch (currentPosition.Direction)
            {
                case Enums.Direction.N:
                    {
                        nextPosition.Y--;
                        break;
                    }

                case Enums.Direction.S:
                    {
                        nextPosition.Y++;
                        break;
                    }

                case Enums.Direction.W:
                    {
                        nextPosition.X--;
                        break;
                    }

                case Enums.Direction.E:
                    {
                        nextPosition.X++;
                        break;
                    }
            }

            return nextPosition;
        }

        private Position MoveForward(Position currentPosition)
        {
            var nextPosition = new Position
            {
                X = currentPosition.X,
                Y = currentPosition.Y,
                Direction = currentPosition.Direction
            };

            switch (currentPosition.Direction)
            {
                case Enums.Direction.N:
                    {
                        nextPosition.Y++;
                        break;
                    }

                case Enums.Direction.S:
                    {
                        nextPosition.Y--;
                        break;
                    }

                case Enums.Direction.W:
                    {
                        nextPosition.X--;
                        break;
                    }

                case Enums.Direction.E:
                    {
                        nextPosition.X++;
                        break;
                    }
            }

            return nextPosition;
        }

        private void ValidateStartPoint(Plateau pateau, int x, int y)
        {
            if (x < 0)
                throw new StartingPointOutOfRangeException("Starting point X coordinate should be greater or equal zero");

            if (y < 0)
                throw new StartingPointOutOfRangeException("Starting point Y coordinate should be greater or equal zero");

            if (x > pateau.Width)
                throw new StartingPointOutOfRangeException($"Starting point Y coordinate should be greater or equal {pateau.Width}");

            if (y > pateau.Height)
                throw new StartingPointOutOfRangeException($"Starting point Y coordinate should be greater or equal {pateau.Height}");
        }

        private void ValidateBoundaries(Plateau pateau, Position endPoint)
        {
            var outOfBoundaryMsg = "Rover out of the boundary of the";

            if (endPoint.Y > pateau.Height)
                throw new EndPointOutOfMapException($"{outOfBoundaryMsg} North side.");

            if (endPoint.Y < 0)
                throw new EndPointOutOfMapException($"{outOfBoundaryMsg} South side.");

            if (endPoint.X > pateau.Width)
                throw new EndPointOutOfMapException($"{outOfBoundaryMsg} East side.");

            if (endPoint.X < 0)
                throw new EndPointOutOfMapException($"{outOfBoundaryMsg} West side.");
        }
    }
}

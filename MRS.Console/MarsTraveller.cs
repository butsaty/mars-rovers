using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MRS.Business.Models;
using MRS.Services;
using MRS.Services.Interfaces;

using Enums = MRS.Common.Enums;

namespace MRS
{
    class MarsTraveller
    {
        private IMapService _mapService;
        private ILocationService _locationService;
        private const int _roverCount = 2;

        public MarsTraveller(IMapService mapService, ILocationService locationService)
        {
            _mapService = mapService;
            _locationService = locationService;
        }

        static void Main()
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<MarsTraveller>().Run();
        }

        private void Run()
        {
            var rovers = new List<IRover>();
            var plateau = CreatePlateau();

            while (rovers.Count < _roverCount)
            {
                Console.WriteLine($"Rover #{ rovers.Count + 1}");

                var currentPosition = CreateStartPoint(plateau);
                var commands = CreateCommands();
                var rover = CreateRover(plateau, currentPosition, commands);

                try
                {
                    rover.Move();
                    rovers.Add(rover);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine();
                    Console.WriteLine($"Simulation failed.");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"Try again");

                    Console.ResetColor();
                }
                finally
                {
                    Console.WriteLine();
                }
            }

            PrintRover(rovers);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private Plateau CreatePlateau()
        {
            Plateau plateau = null;

            while (plateau == null)
            {
                Console.WriteLine("Enter the pateau size:");

                try
                {
                    plateau = _mapService.ParseMap(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
                finally
                {
                    Console.WriteLine();
                }
            }

            return plateau;
        }

        private Position CreateStartPoint(Plateau plateau)
        {
            Position position = null;


            while (position == null)
            {
                Console.WriteLine("Enter starting point:");

                try
                {
                    position = _locationService.ParseStartPoint(plateau, Console.ReadLine());
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
                finally
                {
                    Console.WriteLine();
                }
            }

            return position;
        }

        private IEnumerable<Enums.CommandType> CreateCommands()
        {
            IEnumerable<Enums.CommandType> commands = null;

            while (commands == null)
            {
                Console.WriteLine("Enter commands:");

                try
                {
                    commands = _locationService.ParseCommands(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
                finally
                {
                    Console.WriteLine();
                }
            }

            return commands;
        }

        private IRover CreateRover(Plateau plateau, Position startPoint, IEnumerable<Enums.CommandType> commands) => new Rover(_locationService)
        {
            Plateau = plateau,
            CurrentPosition = startPoint,
            Commands = commands
        };

        private void PrintRover(IEnumerable<IRover> rovers)
        {
            Console.WriteLine("------------------");
            Console.WriteLine("Rover's positions:");

            int roverNumber = 1;

            foreach (var rover in rovers)
            {
                Console.WriteLine($"Rover #{roverNumber++ }: {rover.CurrentPosition.X} {rover.CurrentPosition.Y} {rover.CurrentPosition.Direction}");
            }

            Console.WriteLine("------------------");
        }

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.WriteLine();
            Console.ResetColor();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IMapService, MapService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<MarsTraveller>();

            return services;
        }
    }
}

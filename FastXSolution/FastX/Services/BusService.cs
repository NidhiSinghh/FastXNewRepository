﻿using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using FastX.Repositories;

namespace FastX.Services
{
    public class BusService : IBusService
    {
        //private readonly IBusRepository _busRepository;
        private readonly IRepository<int,Bus> _busRepository;
        private readonly ILogger<BusService> _logger;
        private IRepository<int, BusOperator> _busOperatorRepository;


        public BusService(
            //IBusRepository busRepository,
            IRepository<int,Bus> busRepository,
             IRepository<int, BusOperator> busOperatorRepository,
            ILogger<BusService> logger)
        {
            //_busRepository = busRepository;
            _busRepository = busRepository;
            _busOperatorRepository = busOperatorRepository;
            _logger=logger;
        }

        //public async Task<List<BusDto>> SearchBusesAsync(string origin, string destination, DateTime date)
        //{
        //    try
        //    {
        //        var bus = await _busRepository.GetAsync(id);
        //        if (bus == null )
        //        {
        //            throw new BusNotFoundException();

        //        }
        //        return bus;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        // You can throw a custom exception or return a default value based on your requirements
        //        throw new BusNotFoundException();

        //    }
            
        //        var busDtos = buses.Select(bus => new BusDto
        //        {
        //            BusId = bus.BusId,
        //            BusName = bus.BusName,
        //            BusType = bus.BusType,
        //            TotalSeats = bus.TotalSeats,
        //            Origin = bus.Origin,

        //            Destination = bus.Destination,
        //        }).ToList();

        public async Task<Bus> AddBus(string busName, string busType, int totalSeats, int busOperatorId)
        {
            try
            {
                // Check if the bus operator exists
                var busOperator = await _busOperatorRepository.GetAsync(busOperatorId);
                if (busOperator == null)
                {
                    throw new BusOperatorNotFoundException();
                }
                var bus = new Bus
                {
                    BusName = busName,
                    BusType = busType,
                    TotalSeats = totalSeats,
                    BusOperatorId = busOperatorId
                };

                return await _busRepository.Add(bus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }

    //    public async Task<List<BusDto>> SearchBusesAsync(string origin, string destination, DateTime date, string busType)
    //    {
    //        try
    //        {
    //            // Implement logic to filter available buses based on origin, destination, and travel date
    //            //var availableBuses = _busRepository.GetBusesByRoute(origin, destination);
    //            var buses = await _busRepository.GetAsync();
    //            var availableBuses = buses
    //.Where(b =>
    //    b.BusRoute != null &&
    //    b.BusRoute.Any(r =>
    //        r.Route != null &&
    //        r.Route.Origin == origin &&
    //        r.Route.Destination == destination &&
    //         r.Route.TravelDate == travelDate.Date))
    //.ToList();

    //            if (availableBuses == null || !availableBuses.Any())
    //            {
    //                throw new BusNotFoundException();
    //            }

    //            }
    //            var busDtos = buses.Select(bus => new BusDto
    //            {
    //                BusId = bus.BusId,
    //                BusName = bus.BusName,
    //                BusType = bus.BusType ,
    //                Origin = origin,
    //                Destination = destination
    //            }).ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError($"Error while getting available buses: {ex.Message}");
    //            throw;
    //        }
    //    }
        public async Task<Bus> DeleteBus(int busId)
        {
            try
            {
                return await _busRepository.Delete(busId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bus");
                throw; // Re-throw the exception for the caller to handle
            }
        }

        //public Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime travelDate)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<List<BusDTOForUser>> GetAvailableBuses(string origin, string destination, DateTime travelDate)
        {
            try
            {
                // Implement logic to filter available buses based on origin, destination, and travel date
                var buses = await _busRepository.GetAsync();
                var availableBuses = buses
                    .Where(b => b.BusRoute!=null && b.BusRoute.Any(r => r.Route!=null && r.Route.Origin == origin &&
                                                      r.Route.Destination == destination &&
                                                      r.Route.TravelDate == travelDate.Date))
                    .ToList();




                if (availableBuses == null)
                {
                    // Handle the case where availableBuses is null (e.g., return an empty list or throw an exception)
                    return new List<BusDTOForUser>();
                }

                if (!availableBuses.Any())
                {
                    throw new BusNotFoundException();
                }

                return availableBuses.Select(bus => new BusDTOForUser
                {
                    BusId = bus.BusId,
                    BusName = bus.BusName ?? "N/A",
                    BusType = bus.BusType ?? "N/A",
                    Origin = origin,
                    Destination = destination
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting available buses: {ex.Message}");
                throw new BusNotFoundException();
            }
        }

    }
}

   
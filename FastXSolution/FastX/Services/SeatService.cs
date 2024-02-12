﻿using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System;

namespace FastX.Services
{
    public class SeatService : ISeatService
    {
        //private readonly IRepository<int, Seat> _repository;
        //private readonly IRepository<int, Seat> _seatRepository;
        private readonly IRepository<int, Bus> _busRepository;
        private readonly IBookingRepository<int, Booking> _bookingRepository;
        private readonly IRepository<int, Routee> _routeRepository;
        private readonly IRepository<int, Ticket> _ticketRepository;
        private readonly ISeatRepository<int, Seat> _seatRepository;
        private readonly ILogger<BusService> _logger;

        public SeatService(
             //IBusRepository busRepository,
             //IRepository<int, Seat> repository,
             ISeatRepository<int, Seat> seatRepository,
             IRepository<int, Bus> busRepository,
             IRepository<int, Ticket> ticketRepository,
             IRepository<int, Routee> routeRepository,
             IBookingRepository<int, Booking> bookingRepository,
        ILogger<BusService> logger)
        {
            //_busRepository = busRepository;
            //_repository = repository;
            _seatRepository = seatRepository;
            _busRepository = busRepository;
            _routeRepository = routeRepository;
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        public Task<Seat> AddSeat(Seat seat)
        {
            throw new NotImplementedException();
        }

        public Task<Seat> DeleteSeat(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Seat> GetSeat(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Seat>> GetSeatList()
        {
            throw new NotImplementedException();
        }
        //public async Task<List<SeatDTOForUser>> GetAvailableSeats(int busId, int seatId)
        //{
        //    try
        //    {
        //        var seats = await _seatRepository.GetAsync(busId, seatId);
        //        if (buses == null)
        //        {
        //            throw new BusNotFoundException();
        //        }
        //        var availableSeats =
        //            buses.Seats.Where(s => s.IsAvailable == true).ToList();
        //        if (availableSeats == null)
        //        {
        //            throw new NoSeatsAvailableException();
        //        }

        //        return availableSeats.Select(seat => new SeatDTOForUser
        //        {
        //            SeatId = seat.SeatId,
        //            SeatPrice = seat.SeatPrice,
        //            IsAvailable = seat.IsAvailable
        //        }).ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error while fetching the seats:{ex.Message}");
        //        throw;
        //    }

        //}

        public async Task ChangeSeatAvailablityAsync(int id)
        {
            var seat = await _seatRepository.GetAsync(id);
            if (seat != null)
            {
                seat.IsAvailable = false;
                await _seatRepository.Update(seat);

            }

        }

        public async Task<List<SeatDTOForUser>> GetAvailableSeats(int busId)
        {
            try
            {
                var bus = await _busRepository.GetAsync(busId);
                if (bus == null)
                {
                    throw new BusNotFoundException();
                }
                //var availableSeats =
                //    //buses.Where(b=>b.Seats!=null && b.Seats.Any(s=>s.IsAvailable==true)).ToList();  
                //    buses.Seats.Where(s => s.IsAvailable == true).ToList();
                var availableSeats = bus?.Seats?.Where(s => s.IsAvailable == true)?.ToList();



                if (availableSeats == null)
                {
                    throw new NoSeatsAvailableException();
                }

                return availableSeats.Select(seat => new SeatDTOForUser
                {
                    SeatId = seat.SeatId,
                    SeatPrice = seat.SeatPrice,
                    IsAvailable = seat.IsAvailable
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching the seats:{ex.Message}");
                throw;
            }

        }

        

        public async Task<bool> CheckWhetherSeatIsAvailableForBooking(int busId, int seatId, DateTime date)
        {
            try
            {
                var doesBusExist =await _busRepository.GetAsync(busId);
                if (doesBusExist == null)
                {
                    throw new BusNotFoundException();
                }
                var seat = await _seatRepository.GetAsync(busId,seatId);

                if (seat != null && seat.IsAvailable == false)
                {
                    // The seat is available for booking
                    throw new NoSeatsAvailableException();
                }

                var tickets = await _ticketRepository.GetAsync();
                var isTicketAvailable = tickets.Any(t => t.SeatId == seatId && t.BusId == busId);

                _logger.LogInformation($"isTicketAvailable{isTicketAvailable}");
                if (isTicketAvailable == true)
                {



                    var isBookingComplete = tickets.Any(t => t.SeatId == seatId && t.BusId == busId &&
                                                 t.Booking != null &&
                                                 t.Booking.BusId == busId &&
                                                 t.Booking.BookedForWhichDate == date &&
                                                 t.Booking.Status == "complete");

                    _logger.LogInformation($"isBookingComplete{isBookingComplete}");
                    return !isBookingComplete;
                }
                else
                {
                    // throw new NoTicketsAvailableException();
                    return true;
                }
            }
            catch(Exception ex){
                _logger.LogError($"An error occurred in CheckWhetherSeatIsAvailableForBooking: {ex.Message}");
                throw;

            }

                  



        }

        public async Task<float> GetSeatPriceAsync(int seatId,int busId)
        {
            // Assuming _context is your Entity Framework DbContext
            var seat = await _seatRepository.GetAsync(busId, seatId);

            if (seat != null)
            {
                return seat.SeatPrice;
            }
            else
            {
                throw new NoSeatsAvailableException();
            }
        }




        ////DateTime date;
        ////var availableSeats = await GetAvailableSeats(busId);
        ////bool isSeatAvailable = availableSeats.Any(seat => seat.SeatId == seatId);
        ////_logger.LogInformation($"{isSeatAvailable}");
        //var tickets = await _ticketRepository.GetAsync();
        //var isTicketAvailable = tickets.Any(t => t.SeatId == seatId && t.BusId == busId);

        /////_logger.LogInformation($"isTicketAvailable{isTicketAvailable}");
        //if (isTicketAvailable == true)
        //{
        //    //var booking = await _bookingRepository.GetAsync(); 
        //    //var isBookingComplete = booking.Any(b => b.BusId == busId &&
        //    //                                         b.Tickets != null &&
        //    //                                         b.Tickets.Any(t => t.SeatId == seatId) &&
        //    //                                         b.Status == "complete");


        //    var isBookingComplete = tickets.Any(t => t.SeatId == seatId && t.BusId == busId &&
        //                                 t.Booking != null &&
        //                                 t.Booking.BusId == busId &&
        //                                 t.Booking.BookedForWhichDate == date &&
        //                                 t.Booking.Status == "complete");
        //    _logger.LogInformation($"isBookingComplete{isBookingComplete}");
        //    return !isBookingComplete;
        //}



        //else
        //{
        //    return true;
        //}


        public async Task ChangeJourneyStatus()
        {
            var currentDate = DateTime.Now;
            var routes = await _routeRepository.GetAsync();
            if (routes == null)
            {
                throw new NoSuchRouteeException();
            }
            var toChangeStatusFor = routes.Where(r => r.TravelDate != null && r.TravelDate < currentDate)
            .SelectMany(r => r.BusRoute.Select(br => br.Bus))
            .ToList();

            foreach (var bus in toChangeStatusFor)
            {
                foreach (var busRoute in bus.BusRoute)
                {
                    if (busRoute.Route.TravelDate != null && busRoute.Route.TravelDate < currentDate)
                    {
                        // Assuming you have a property in the BusRoute entity to update the JourneyStatus
                        busRoute.JourneyStatus = "Completed";
                    }
                }
            }
        }
    }
}

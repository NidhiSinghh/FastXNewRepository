using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using FastX.Repositories;
using System.Numerics;

namespace FastX.Services
{
    public class BookingService:IBookingService
    {
        //private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<int,Booking> _bookingRepository;
        private readonly IBookingRepository<int, Booking> _booking2Repository;
        //private readonly IPaymentRepository _paymentRepository;
        private readonly ISeatService _seatService;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            //  ITicketRepository ticketRepository,
            IRepository<int, Booking> bookingRepository,
           // IPaymentRepository paymentRepository,
           ISeatService seatService,
           IBookingRepository<int, Booking> booking2Repository,
        ILogger<BookingService> logger)
        {
            //_ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
            _booking2Repository = booking2Repository;
            //_paymentRepository = paymentRepository;
            _seatService = seatService;
            _logger = logger;
        }
        public async Task ChangeNoOfSeatsAsync(int id, int noOfSeats)
        {
            var booking = await _bookingRepository.GetAsync(id);
            if (booking != null)
            {
                booking.NumberOfSeats = noOfSeats;
                
            }
         
        }

        public async Task MakeBooking(int busId, int seatId, DateTime travelDate, int userId)
        {
            int noOfSeats = 0;
            var seatStatus=await _seatService.CheckWhetherSeatIsAvailableForBooking(busId, seatId, travelDate);
            if (seatStatus == false)
            {
                throw new NoSeatsAvailableException();
            }

            var ongoingBooking = await _booking2Repository.GetOngoingBookingAsync(busId, userId, travelDate);
            int bookingId = ongoingBooking.BookingId;
            int noOfSeatsPresent = ongoingBooking.NumberOfSeats;
            if (ongoingBooking == null)
            {
                var newBooking = new Booking
                {
                    BookingDate = DateTime.Now,
                    BookedForWhichDate = travelDate,
                    BusId = busId,
                    UserId = userId,
                    NumberOfSeats = noOfSeats + 1,
                    Status = "ongoing"


                };
       
            }
            else
            {
               await  ChangeNoOfSeatsAsync(bookingId, noOfSeatsPresent+1);
            }

            //var newBooking = new Booking
            //{
            //    BookingDate = DateTime.Now,
            //    BookedForWhichDate = travelDate,
            //    BusId = busId,
            //    UserId = userId,
            //    NumberOfSeats = noOfSeats+1,
            //    Status="ongoing"


            //};
            //var ongoingBooking = await _bookingRepository.Add(newBooking);
        }
    }
}

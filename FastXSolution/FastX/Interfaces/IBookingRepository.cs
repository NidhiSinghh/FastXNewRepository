using FastX.Models;

namespace FastX.Interfaces
{
    public interface IBookingRepository<K, T>
    {
        Task<Booking?> GetOngoingBookingAsync(int busId, int userId, DateTime travelDate);
        
    }
}

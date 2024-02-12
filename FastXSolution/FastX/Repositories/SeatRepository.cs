using FastX.Contexts;
<<<<<<< HEAD
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FastX.Repositories
{
    public class SeatRepository : IRepository<int, Seat>,ISeatRepository<int,Seat>
=======
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class SeatRepository : IRepository<int, Seat>
>>>>>>> b143ff912861d8a28c98506ceb6837f83c499230
    {
        private readonly FastXContext _context;

        public SeatRepository(FastXContext context)
        {
            _context = context;
        }
        public Task<Seat> Add(Seat item)
        {
            throw new NotImplementedException();
        }

        public Task<Seat> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Seat>> GetAsync()
        {
            var seats = await _context.Seats.ToListAsync();
<<<<<<< HEAD
            if (seats == null){
                throw new NoSeatsAvailableException();
            }
=======
>>>>>>> b143ff912861d8a28c98506ceb6837f83c499230
            return seats;
        }

        public Task<Seat> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

<<<<<<< HEAD
        public async Task<Seat> GetAsync(int key1, int key2)
        {
            var seats = await GetAsync();
            var seat = await _context.Seats
        .FirstOrDefaultAsync(e => e.BusId == key1 && e.SeatId==key2);

            if (seat != null)
                return seat;
            throw new NoSeatsAvailableException();
        }

=======
>>>>>>> b143ff912861d8a28c98506ceb6837f83c499230
        public Task<Seat> Update(Seat item)
        {
            throw new NotImplementedException();
        }
    }
}

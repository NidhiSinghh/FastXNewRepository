using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class RouteeRepository : IRepository<int, Routee>
    {
        private readonly FastXContext _context;
        public RouteeRepository(FastXContext context)
        {
            _context = context;
        }

        public async Task<Routee> Add(Routee item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Task<Routee> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Routee>> GetAsync()
        {
            var routes = await _context.Routees.Include(b => b.BusRoute).ToListAsync();
            if (routes == null)
            {
                throw new NoSuchRouteeException();
            }
            return routes;
        }

        public Task<Routee> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<Routee> Update(Routee item)
        {
            throw new NotImplementedException();
        }
    }
}
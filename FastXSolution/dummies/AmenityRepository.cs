using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class AmenityRepository : IRepository<int, Amenity>
    {
        private readonly FastXContext _context;
        public AmenityRepository(FastXContext context)
        {
            _context = context;
        }

        public async Task<Amenity> Add(Amenity item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Amenity> Delete(int key)
        {
            var amenity = await GetAsync(key);
            _context.Amenities.Remove(amenity);
            _context.SaveChanges();
            //_logger.LogInformation("Amenity deleted " + key);
            return amenity;
        }


        public async Task<Amenity> GetAsync(int key)
        {
            var amenitys = await GetAsync();
            var amenity = amenitys.SingleOrDefault(u => u.AmenityId == key);
            if (amenity != null)
            {
                return amenity;
            }
            throw new NoSuchAmenityException();

        }


        public async Task<List<Amenity>> GetAsync()
        {
            var amenity = _context.Amenities.ToList();
            return amenity;
        }


        public async Task<Amenity> Update(Amenity item)
        {
            var amenity = await GetAsync(item.AmenityId);
            _context.Entry<Amenity>(item).State = EntityState.Modified;
            _context.SaveChanges();
            // _logger.LogInformation("Amenity updated " + item.Id);
            return amenity;
        }
    }

}


using FastX.Models;

namespace FastX.Interfaces
{
    public interface IAmenityService
    {
        public Task<Amenity> AddAmenity(Amenity amenity);
        public Task<List<Amenity>> GetAmenityList();
        public Task<Amenity> GetAmenity(int id);
        public Task<Amenity> DeleteAmenity(int id);

    }
}

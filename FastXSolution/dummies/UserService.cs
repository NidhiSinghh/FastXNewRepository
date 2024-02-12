using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using System.Numerics;

namespace FastX.Services
{
    public class UserService : IUserService
    {
        private IRepository<int, User> _repo;
        //private readonly ILogger<RouteeService> _logger;

        public UserService(IRepository<int, User> repo)
        {
            _repo = repo;

        }
        public async Task<User> AddUser(User user)
        {
            user = await _repo.Add(user);
            return user;
        }
        public async Task<User> DeleteUser(int id)
        {
            var user = await GetUser(id);
            if (user != null)
            {
                user = await _repo.Delete(id);
                return user;
            }
            throw new NoSuchUserException();
            
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _repo.GetAsync(id);
            return user;

        }
        public async Task<List<User>> GetUserList()
        {
            var user = await _repo.GetAsync();
            return user;
        }


    }
}

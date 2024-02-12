using FastX.Contexts;
using FastX.Exceptions;
using FastX.Interfaces;
using FastX.Models;
using Microsoft.EntityFrameworkCore;

namespace FastX.Repositories
{
    public class UserRepository : IRepository<int, User>
    {
        private readonly FastXContext _context;
        public UserRepository(FastXContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<User> Delete(int key)
        {
            var user = await GetAsync(key);
            _context.Users.Remove(user);
            _context.SaveChanges();
            //_logger.LogInformation("User deleted " + key);
            return user;
        }


        public async Task<User> GetAsync(int key)
        {
            var users = await GetAsync();
            var user = users.SingleOrDefault(u => u.UserId == key);
            if (user!= null)
            {
                return user;
            }
            throw new NoSuchUserException();
            
        }


        public async Task<List<User>> GetAsync()
        {
            var user = _context.Users.ToList();
            return user;
        }


        public async Task<User> Update(User item)
        {
            var user = await GetAsync(item.UserId);
            _context.Entry<User>(item).State = EntityState.Modified;
            _context.SaveChanges();
           // _logger.LogInformation("Patient updated " + item.Id);
            return user;
        }
    }

}


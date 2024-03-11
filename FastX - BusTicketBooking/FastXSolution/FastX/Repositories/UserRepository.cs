﻿using FastX.Contexts;
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

        public Task<User> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users
        .Include(u => u.Bookings)                        // Include bookings
            .ThenInclude(b => b.Tickets)                // Include tickets within bookings
        //.Include(u => u.Bookings)                        // Include bookings again
        //    .ThenInclude(b => b.Bus).ThenInclude(b => b.BusRoute).ThenInclude(b => b.Route)                   // Include bus within bookings
        .ToListAsync();
            return users;
        }


        //public async Task<User> GetAsync(string key)
        //{
        //    var user = _context.Users.SingleOrDefault(u => u.Username == key);
        //    return user;

        //}

        public async Task<User> GetAsync(int key)
        {

            var users = await GetAsync();
            var user = users.SingleOrDefault(u => u.UserId == key);
            //if (user != null)
            //{
            //    return user;
            //}
            //throw new AmenitiesNotFoundException();


            //var user123 = await _context.Users
            //        .Include(u => u.Bookings)                        // Include bookings
            //            .ThenInclude(b => b.Tickets)                // Include tickets within bookings
            //        .Include(u => u.Bookings)                        // Include bookings again
            //            .ThenInclude(b => b.Bus).ThenInclude(b => b.BusRoute).ThenInclude(b => b.Route)                   // Include bus within bookings
            //        .ToListAsync();
            //return user123;
            return user;
        }

        public Task<User> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}

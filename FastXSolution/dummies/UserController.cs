using FastX.Interfaces;
using FastX.Models;
using FastX.Services;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace FastX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _adminService;
        public UserController(IUserService adminService)
        {
            _adminService = adminService;
        }
        
        [HttpPost]
        public async Task<User> Post(User User)
        {
            var addedUser = await _adminService.AddUser(User);
            return addedUser;
        }

        [HttpGet]
        public async Task<List<User>> GetAll()
        {
            var user = await _adminService.GetUserList();
            return user;
        }

        [Route("/GetById")]
        [HttpGet]
        public async Task<User> GetById(int id)
        {
            var user = await _adminService.GetUser(id);
            return user;
        }

        [HttpDelete]
        public async Task<User> Delete(int id)
        {
            var user = await _adminService.DeleteUser(id);
            return user;
        }


    }
}

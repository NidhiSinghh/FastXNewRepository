﻿using FastX.Interfaces;
using FastX.Mappers;
using FastX.Models.DTOs;
using FastX.Models;
using System.Security.Cryptography;
using System.Text;
using FastX.Repositories;
using FastX.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace FastX.Services
{
    public class AllUserService : IAllUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Admin> _adminRepository;
        private readonly IRepository<int, BusOperator> _busOperatorRepository;
        private readonly IRepository<string, AllUser> _alluserRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AllUserService> _logger;

        public AllUserService(IRepository<int, User> userRepository,
                            IRepository<int, Admin> adminRepository,
                            IRepository<int, BusOperator> busOperatorRepository,
                            IRepository<string, AllUser> alluserRepository,
                            ITokenService tokenService,
                            ILogger<AllUserService> logger)
        {
            _userRepository = userRepository;
            _alluserRepository = alluserRepository;
            _adminRepository = adminRepository;
            _busOperatorRepository = busOperatorRepository;
            _tokenService = tokenService;
            _logger = logger;

        }

        //public async Task<LoginUserDTO> Login(LoginUserInputDTO alluser)
        //{
        //    var myUSer = await _alluserRepository.GetAsync(alluser.Username);
        //    if (myUSer == null)
        //    {
        //        throw new InvlidUserException();
        //    }
        //    var userPassword = GetPasswordEncrypted(alluser.Password, myUSer.Key);
        //    var checkPasswordMatch = ComparePasswords(myUSer.Password, userPassword);
        //    //if (checkPasswordMatch)
        //    //{
        //    //    alluser.Password = "";
        //    //    //alluser.Role = myUSer.Role;
        //    //    alluser.Token = await _tokenService.GenerateToken(alluser);
        //    //    return alluser;
        //    //}
        //    
        //    throw new InvlidUserException();
        //}
        public async Task<LoginUserDTO> Login(LoginUserInputDTO userInput)
        {
            var myUser = await _alluserRepository.GetAsync(userInput.Username);
            if (myUser == null)
            {
                throw new InvlidUserException();
            }

            var userPassword = GetPasswordEncrypted(userInput.Password, myUser.Key);
            var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);

            if (checkPasswordMatch)
            {
                // Generate token
                var token = await _tokenService.GenerateToken(userInput.Username, myUser.Role);

                //var users = await _userRepository.GetAsync();
                //var filteredUsers = users.Where(u => u.Username == userInput.Username).ToList();
                //var userIds = filteredUsers.Select(u => u.UserId).ToList();

                // Return output DTO
                //return new LoginUserDTO
                //{
                //    Username = userInput.Username,
                //    Role = myUser.Role,
                //    Token = token,
                //    UserId = userIds[0],
                //    // If you want to include the password, you can assign it here
                //};

                if (myUser.Role == "user")
                {
                    var user = await _userRepository.GetAsync();
                    var filteredUsers = user.Where(u => u.Username == userInput.Username).ToList();
                    var userIds = filteredUsers.Select(u => u.UserId).ToList();
                    if (user != null)
                    {
                        // Return output DTO with user ID
                        return new LoginUserDTO
                        {
                            Username = userInput.Username,
                            Role = myUser.Role,
                            Token = token,
                            UserId = userIds[0],
                        };
                    }
                }

                else if (myUser.Role == "busoperator")
                {
                    var busOperator = await _busOperatorRepository.GetAsync();
                    var filteredUsers = busOperator.Where(u => u.Username == userInput.Username).ToList();
                    var busOperatorIds = filteredUsers.Select(u => u.BusOperatorId).ToList();
                    if (busOperator != null)
                    {
                        // Return output DTO with bus operator ID
                        return new LoginUserDTO
                        {
                            Username = userInput.Username,
                            Role = myUser.Role,
                            Token = token,
                            UserId = busOperatorIds[0],
                        };

                    }

                    throw new InvlidUserException();
                }

            }
            throw new InvlidUserException();

        }
        [ExcludeFromCodeCoverage]
        private bool ComparePasswords(byte[] password, byte[] userPassword)
            {
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] != userPassword[i])
                        return false;
                }
                return true;
            }

            [ExcludeFromCodeCoverage]
            private byte[] GetPasswordEncrypted(string password, byte[] key)
            {
                HMACSHA512 hmac = new HMACSHA512(key);
                var userpassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return userpassword;
            }

            public async Task<LoginUserDTO> Register(RegisterUserDTO user)
            {
                //exception for dup reg
                var existingUser = await _alluserRepository.GetAsync(user.Username);
                if (existingUser != null)
                {
                    throw new UserAlreadyExistsException();
                }
                //exc end

                AllUser myuser = new RegisterToAllUser(user).GetAllUser();
                myuser = await _alluserRepository.Add(myuser);

                LoginUserDTO result = new LoginUserDTO
                {
                    Username = myuser.Username,
                    Role = myuser.Role,
                };

                switch (user.Role.ToLower())
                {
                    case "admin":
                        Admin admin = new RegisterToAdmin(user).GetAdmin();
                        await _adminRepository.Add(admin);
                        break;
                    case "busoperator":
                        BusOperator busOperator = new RegisterToBusOperator(user).GetBusOperator();
                        await _busOperatorRepository.Add(busOperator);
                        break;
                    case "user":
                        User regularUser = new RegisterToUser(user).GetUser();
                        await _userRepository.Add(regularUser);
                        break;
                    default:
                        // Handle invalid role here (throw exception, log error, etc.)
                        break;
                }

                return result;

            }
    }
}




//using FastX.Exceptions;
//using FastX.Interfaces;
//using FastX.Models;

//namespace FastX.Services
//{
//    public class AdminService : IAdminService
//    {
//        private IAdminRepository<AllUser> _userRepository;
//        private readonly ILogger<RouteeService> _logger;

//        public AdminService(IAdminRepository<AllUser> userRepository, ILogger<RouteeService> logger)
//        {
//            _userRepository = userRepository;
//            _logger = logger;
//        }

//        public async Task DeleteUserAsync(string username)
//        {
//            var user = await _userRepository.GetByIdAsync(username);
//            if (user != null)
//            {
//                _userRepository.DeleteAsync(username);
//                await _userRepository.SaveChangesAsync();
//            }
//        }
//    }
//}

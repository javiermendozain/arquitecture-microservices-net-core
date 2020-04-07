using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Api.Users.Helpers;
using Api.Users.Interfaces;
using Api.Users.Models;

namespace Api.Users.Respositories
{
    public class FakeUsersRepository : IUsersRepository
    {
        private readonly List<UserModel> _repo = new List<UserModel>();
        private readonly ILogger<FakeUsersRepository> _logger;

        public FakeUsersRepository(ILogger<FakeUsersRepository> logger)
        {
            _logger = logger;
            _repo.Add(new UserModel()
            {
                Id = 1,
                Email = "example@mail.com",
                FirstName = "Javier",
                LastName = "Mendoza",
                UserName = "javiermendozain",
            });
            _repo.Add(new UserModel()
            {
                Id = 2,
                Email = "example2@mail.com",
                FirstName = "Eduardo",
                LastName = "Castillo",
                UserName = "javiermendozain",
            });
        }

        public Task<Response> AddAsync(UserModel user)
        {
            try
            {
                _logger?.LogInformation($"Add user with Id: {user.Id}");
                user.Id = _repo.Max(c => c.Id) + 1;
                _repo.Add(user);
                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = user
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> GetAllAsync(int page, int pageSize)
        {
            try
            {
                _logger?.LogInformation($"Querying Users with page:{page}, size:{pageSize}");
                var result = _repo.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Task.FromResult(new Response()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> GetByIdAsync(int id)
        {
            try
            {
                _logger?.LogInformation($"Querying user with Id: {id}");

                var result = _repo.FirstOrDefault(c => c.Id == id);
                if (result == null)
                {
                    _logger?.LogWarning($"User with Id: {id} was not found");
                    return Task.FromResult( new Response()
                   {
                       IsSuccess = false,
                       ErrorMessage = $"User with Id: {id} was not found"
                   });
                }
                return Task.FromResult( new Response()
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> UpdateAsync(int id, UserModel user)
        {
            try
            {
                _logger?.LogInformation($"Update user with Id: {id}");

                var userToUpdate = _repo.FirstOrDefault(c => c.Id == id);
                if (userToUpdate != null)
                {
                    userToUpdate.Email = user.Email;
                    userToUpdate.FirstName = user.FirstName;
                    userToUpdate.LastName = user.LastName;
                    userToUpdate.UserName = user.UserName;

                    return Task.FromResult(new Response()
                    {
                        IsSuccess = true,
                        Data = userToUpdate
                    });
                }
                else
                {
                    _logger?.LogWarning($"User with Id: {id} was not found");
                    return Task.FromResult(new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = $"User with Id: {id} was not found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        public Task<Response> DeleteAsync(int userId)
        {
            try
            {
                _logger?.LogInformation($"Deleting user with Id: {userId}");
                var userToDelete = _repo.FirstOrDefault(c => c.Id == userId);
                if (userToDelete == null)
                {
                    _logger?.LogWarning($"Resource not found");
                    return Task.FromResult(new Response()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Resource not found"
                    });
                }

                _repo.Remove(userToDelete);
                return Task.FromResult(new Response() { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
                return Task.FromResult(new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}

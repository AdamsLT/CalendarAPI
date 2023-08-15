using Calendarize.Core.Entities;
using Calendarize.Infrastructure.Repositories;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendarize.Core.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string name, string lastName, string email, string phoneNumber)
        {
            return await _userRepository.GetAllAsync(name, lastName, email, phoneNumber);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _userRepository.GetAsync(objectId);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = ObjectId.GenerateNewId();
            var dateTime = DateTime.UtcNow;
            user.CreatedAt = dateTime;
            user.UpdatedAt = dateTime;

            return await _userRepository.UpsertAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _userRepository.DeleteAsync(objectId);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var dateTime = DateTime.UtcNow;
            user.UpdatedAt = dateTime;

            return await _userRepository.UpsertAsync(user);
        }
    }
}

using Crud_Application.Models;
using Crud_Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crud_Application.Services
{
    // Represents a service for managing user-related operations
    public class UserService : IUserService
    {
        private readonly AppDBContext _dbContext;

        public UserService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Create a new user
        public void CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        // Retrieve a user by ID
        public User GetUserById(int id)
        {
            return _dbContext.Users.Find(id);
        }

        // Retrieve all users
        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        // Update an existing user
        public void UpdateUser(User user)
        {
            var existingUser = _dbContext.Users.Find(user.Id);
            if (existingUser != null)
            {
                existingUser.UpdatedAt = DateTime.Now;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;

                _dbContext.SaveChanges();
            }
        }

        // Delete a user by ID
        public void DeleteUser(int id)
        {
            var user = _dbContext.Users.Find(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        // Retrieve a user by email
        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        // Check if the provided email and password combination is valid
        public bool IsValidUser(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }
    }
}

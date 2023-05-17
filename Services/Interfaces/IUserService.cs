using Crud_Application.Models;

namespace Crud_Application.Services.Interfaces
{
    public interface IUserService
    {
        void CreateUser(User user);
        User GetUserById(int id);
        List<User> GetAllUsers();
        void UpdateUser(User user);
        void DeleteUser(int id);
        User GetUserByEmail(string email);
        bool IsValidUser(string email, string password);

    }
}

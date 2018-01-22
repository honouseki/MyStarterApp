using System.Collections.Generic;
using MyStarterApp.Models.Domain;
using MyStarterApp.Models.ViewModels;

namespace MyStarterApp.Services.Services
{
    public interface IUserService
    {
        List<User> AdminSelectAll();
        int CheckEmail(string email);
        int CheckUsername(string username);
        int Insert(LoginUser model);
        int Login(LoginUser model, bool remember);
        User AdminSelectById(int id);
        User SelectByUsername(string username);
    }
}
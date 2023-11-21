using dotnetapp.Models;
using System.Threading.Tasks;


namespace dotnetapp.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user);
        Task<string> LoginAsync(string username, string password);
    }
}

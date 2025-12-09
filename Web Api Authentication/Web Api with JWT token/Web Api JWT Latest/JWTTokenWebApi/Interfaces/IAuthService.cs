using JWTTokenWebApi.Models;
using JWTTokenWebApi.Request_Models;

namespace JWTTokenWebApi.Interfaces
{
    public interface IAuthService
    {
        user Adduser(user _user);
        string login (LoginRequest loginRequest);
    }
}

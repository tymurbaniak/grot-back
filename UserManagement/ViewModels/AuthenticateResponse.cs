using System.Text.Json.Serialization;
using UserManagement.Interfaces;

namespace UserManagement.ViewModels
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(IUser user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Username = user.Name;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}

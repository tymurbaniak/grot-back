using System.Collections.Generic;
using UserManagement.ViewModels;

namespace UserManagement.Interfaces
{
    public interface IUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}

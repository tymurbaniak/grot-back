using System;
using System.Collections.Generic;
using UserManagement.Interfaces;
using UserManagement.ViewModels;

namespace UserManagement.DBModels
{
    public class User : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public int? UserGroupId { get; set; }
        public string Password { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? LastUpdateDateTime { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}

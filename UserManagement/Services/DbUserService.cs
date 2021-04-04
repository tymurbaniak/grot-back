using System;
using System.Linq;
using UserManagement.DBContexts;
using UserManagement.DBModels;
using UserManagement.Interfaces;

namespace UserManagement.Services
{
    public class DbUserService
    {
        private readonly UsersDBContext _context;

        public DbUserService(UsersDBContext context)
        {
            this._context = context;
        }

        public IUser AddUserToDb(IUser user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = PasswordHelper.HashPassword(user.Email),
                Password = PasswordHelper.HashPassword(user.Password),
                CreationDateTime = DateTime.Now,
                LastUpdateDateTime = DateTime.Now
            };

            var result = this._context.Users.Add(newUser);
            this._context.SaveChanges();
            return result.Entity;
        }
    }
}

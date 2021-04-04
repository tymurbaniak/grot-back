using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.DBContexts
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UsersDBContext>
    {
        public UsersDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDBContext>();
            string mySqlConnectionStr = "server=localhost; port=3306; database=grot; user=grot; password=grot; Persist Security Info=False; Connect Timeout=300";
            optionsBuilder.UseMySql(mySqlConnectionStr);

            return new UsersDBContext(optionsBuilder.Options);
        }
    }
}

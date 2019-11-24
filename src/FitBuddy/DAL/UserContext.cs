using FitBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FitBuddy.DAL
{
    public class UsersContext : DbContext
    {
        public UsersContext() : base("Data Source=mssql6.webio.pl,2401;Database=cienkarenta_FitBuddy;Uid=cienkarenta_cienkarenta;Password=Apokalipsa1303!")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
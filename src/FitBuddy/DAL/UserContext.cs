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
        public UsersContext() : base("Data Source=DESKTOP-9TVPAS9\\SQLEXPRESS01;Initial Catalog=FitBuddy;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
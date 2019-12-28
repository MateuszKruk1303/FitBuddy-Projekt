using FitBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FitBuddy.DAL
{
    public class UserProgressContext : DbContext
    {
        public UserProgressContext() : base("Data Source=mssql6.webio.pl,2401;Database=cienkarenta_FitBuddy;Uid=cienkarenta_cienkarenta;Password=Apokalipsa1303!")
        {

        }

        public DbSet<UserProgress> Progress { get; set; }
    }
}
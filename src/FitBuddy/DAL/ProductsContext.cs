using FitBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FitBuddy.DAL
{
    public class ProductsContext : DbContext
    {
        public ProductsContext() : base("Data Source=mssql6.webio.pl,2401;Database=cienkarenta_FitBuddy;Uid=cienkarenta_cienkarenta;Password=Apokalipsa1303!")
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}


//base("Data Source=DESKTOP-9TVPAS9\\" +
//            "SQLEXPRESS01;Initial Catalog" +
//            "=FitBuddy;Integrated " +
//            "Security=True;MultipleActiveResu" +
//            "ltSets=True;Application Name=EntityFramework")
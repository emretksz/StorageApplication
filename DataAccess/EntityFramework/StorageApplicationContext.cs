using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Type = Entities.Concrete.Type;

namespace DataAccess.EntityFramework
{
    public class StorageApplicationContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Information> Informations { get; set; }
        public virtual DbSet<PhysicalInformation> PhysicalInformations { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.\;Database=StorageApplicationContext;Trusted_Connection=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

          
        }
    }
    }

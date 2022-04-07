using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToyAPI.Models;

namespace ToyAPI.Data
{
    //set up database class
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<LocationModel> Locations { get;set; }
        //constructor that allows us to use the database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}

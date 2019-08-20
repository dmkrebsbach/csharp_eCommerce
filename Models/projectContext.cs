using Microsoft.EntityFrameworkCore;

namespace projectName.Models //change projectName to the name of project
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users{get;set;}
        public DbSet<Product> Products{get;set;}
        public DbSet<Order> Orders{get;set;}
        public DbSet<Customer> Customers{get;set;}  
                                            
    }
}
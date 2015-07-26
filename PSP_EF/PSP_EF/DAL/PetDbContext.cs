using System.Data.Entity;

using PSP_EF.Models.Mappings;
using PSP_EF.Models;

namespace PSP_EF.DAL
{
    public class PetDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LostFound> LostFounds { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetClass> PetClass { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<LF_Comment> LF_Comment { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new LostFoundMap());
            modelBuilder.Configurations.Add(new PetClassMap());
            modelBuilder.Configurations.Add(new PetMap());
            modelBuilder.Configurations.Add(new RatingMap());
            modelBuilder.Configurations.Add(new ServiceMap());
            modelBuilder.Configurations.Add(new TransactionMap());
            modelBuilder.Configurations.Add(new LF_CommentMap());
            modelBuilder.Configurations.Add(new RecommendationMap());
        }
    }

}
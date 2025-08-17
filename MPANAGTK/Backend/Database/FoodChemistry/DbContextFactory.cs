using Microsoft.EntityFrameworkCore;

namespace MPANAGTK.Backend.Database.FoodChemistry
{
    public class DbContextFactory
    {
        public static FoodChemistryDbContext CreateDbContext()
        {
            var connectionString = "Data Source = FoodChemistry.db";
            var optionsBuilder = new DbContextOptionsBuilder<FoodChemistryDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            return new FoodChemistryDbContext(optionsBuilder.Options);
        }
    }
}

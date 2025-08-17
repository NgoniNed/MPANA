using Microsoft.EntityFrameworkCore;
using MPANAGTK.Models.FoodChemistry;

namespace MPANAGTK.Backend.Database.FoodChemistry
{
    public class FoodChemistryDbContext : DbContext
    {
        public FoodChemistryDbContext(DbContextOptions<FoodChemistryDbContext> options) : base(options)
        {

        }
        public DbSet<FoodTaxonomies> FoodTaxonomies
        {
            get;
            set;
        }

        public DbSet<Foods> Foods
        {
            get;
            set;
        }
        public DbSet<Nutrients> Nutrients
        {
            get;
            set;
        }
        public DbSet<Flavors> Flavors
        {
            get;
            set;
        }
        public DbSet<Enzymes> Enzymes
        {
            get;
            set;
        }
        public DbSet<HealthEffects> HealthEffects
        {
            get;
            set;
        }
        public DbSet<CompoundsHealthEffects> CompoundsHealthEffects
        {
            get;
            set;
        }
        public DbSet<CompoundsFlavors> CompoundsFlavors
        {
            get;
            set;
        }
        public DbSet<CompoundsEnzymes> CompoundsEnzymes
        {
            get;
            set;
        }
        public DbSet<CompoundSubstituents> CompoundSubstituents
        {
            get;
            set;
        }
        public DbSet<CompoundAlternateParent> CompoundAlternateParent
        {
            get;
            set;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var foodtaxonomiesEntity = modelBuilder.Entity<FoodTaxonomies>();
            foodtaxonomiesEntity.HasKey(ft_entity => ft_entity.ID);
            foodtaxonomiesEntity.Property(ft_entity => ft_entity.food_id);
            foodtaxonomiesEntity.Property(ft_entity => ft_entity.ncbi_taxonomy_id);
            foodtaxonomiesEntity.Property(ft_entity => ft_entity.classification_order);
            foodtaxonomiesEntity.Property(ft_entity => ft_entity.classification_name);
            var foodsEntity = modelBuilder.Entity<Foods>();
            foodsEntity.HasKey(fd_entity => fd_entity.ID);
            foodsEntity.Property(fd_entity => fd_entity.name);
            foodsEntity.Property(fd_entity => fd_entity.name_scientific);
            foodsEntity.Property(fd_entity => fd_entity.legacy_id);
            foodsEntity.Property(fd_entity => fd_entity.ncbi_taxonomy_id);
            foodsEntity.Property(fd_entity => fd_entity.public_id);
            foodsEntity.Property(fd_entity => fd_entity.food_type);
            foodsEntity.Property(fd_entity => fd_entity.food_subgroup);
            foodsEntity.Property(fd_entity => fd_entity.food_group);
            foodsEntity.Property(fd_entity => fd_entity.category);
            foodsEntity.Property(fd_entity => fd_entity.description);
            foodsEntity.Property(fd_entity => fd_entity.wikipedia_id);
            var nutrientsEntity = modelBuilder.Entity<Nutrients>();
            nutrientsEntity.HasKey(nutrie_entity => nutrie_entity.ID);
            nutrientsEntity.Property(nutrie_entity => nutrie_entity.legacy_id);
            nutrientsEntity.Property(nutrie_entity => nutrie_entity.public_id);
            nutrientsEntity.Property(nutrie_entity => nutrie_entity.name);

            var flavorsEntity = modelBuilder.Entity<Flavors>();
            flavorsEntity.HasKey(fl_entity => fl_entity.ID);
            flavorsEntity.Property(fl_entity => fl_entity.name);
            flavorsEntity.Property(fl_entity => fl_entity.flavor_group);
            flavorsEntity.Property(fl_entity => fl_entity.category);
            flavorsEntity.Property(fl_entity => fl_entity.flavor_id);
            var enzymesEntity = modelBuilder.Entity<Enzymes>();
            enzymesEntity.HasKey(enzy_entity => enzy_entity.ID);
            enzymesEntity.Property(enzy_entity => enzy_entity.enzyme_id);
            enzymesEntity.Property(enzy_entity => enzy_entity.name);
            enzymesEntity.Property(enzy_entity => enzy_entity.gene_name);
            enzymesEntity.Property(enzy_entity => enzy_entity.uniprot_id);
            var healtheffectsEntity = modelBuilder.Entity<HealthEffects>();
            healtheffectsEntity.HasKey(heff_entity => heff_entity.ID);
            healtheffectsEntity.Property(heff_entity => heff_entity.health_effect_id);
            healtheffectsEntity.Property(heff_entity => heff_entity.name);
            healtheffectsEntity.Property(heff_entity => heff_entity.description);
            var compoundshealtheffectsEntity = modelBuilder.Entity<CompoundsHealthEffects>();
            compoundshealtheffectsEntity.HasKey(compHE_entity => compHE_entity.ID);
            compoundshealtheffectsEntity.Property(compHE_entity => compHE_entity.compounds_HE_id);
            compoundshealtheffectsEntity.Property(compHE_entity => compHE_entity.compound_id);
            compoundshealtheffectsEntity.Property(compHE_entity => compHE_entity.health_effect_id);
            compoundshealtheffectsEntity.Property(compHE_entity => compHE_entity.original_health_effect_name);
            compoundshealtheffectsEntity.Property(compHE_entity => compHE_entity.original_compound_name);
            var compoundsflavorsEntity = modelBuilder.Entity<CompoundsFlavors>();
            compoundsflavorsEntity.HasKey(compFlavor_entity => compFlavor_entity.ID);
            compoundsflavorsEntity.Property(compFlavor_entity => compFlavor_entity.compounds_flavor_id);
            compoundsflavorsEntity.Property(compFlavor_entity => compFlavor_entity.compound_id);
            compoundsflavorsEntity.Property(compFlavor_entity => compFlavor_entity.flavor_id);
            var compoundsenzymesEntity = modelBuilder.Entity<CompoundsEnzymes>();
            compoundsenzymesEntity.HasKey(compEnzy_entity => compEnzy_entity.ID);
            compoundsenzymesEntity.Property(compEnzy_entity => compEnzy_entity.compounds_enzyme_id);
            compoundsenzymesEntity.Property(compEnzy_entity => compEnzy_entity.compound_id);
            compoundsenzymesEntity.Property(compEnzy_entity => compEnzy_entity.enzyme_id);
            var compoundsubstituentsEntity = modelBuilder.Entity<CompoundSubstituents>();
            compoundsubstituentsEntity.HasKey(compSubst_entity => compSubst_entity.ID);
            compoundsubstituentsEntity.Property(compSubst_entity => compSubst_entity.compounds_substituent_id);
            compoundsubstituentsEntity.Property(compSubst_entity => compSubst_entity.compound_id);
            compoundsubstituentsEntity.Property(compSubst_entity => compSubst_entity.name);
            var compoundalternateparentEntity = modelBuilder.Entity<CompoundAlternateParent>();
            compoundalternateparentEntity.HasKey(compAltPar_entity => compAltPar_entity.ID);
            compoundalternateparentEntity.Property(compAltPar_entity => compAltPar_entity.compounds_alternate_parent_id);
            compoundalternateparentEntity.Property(compAltPar_entity => compAltPar_entity.compound_id);
            compoundalternateparentEntity.Property(compAltPar_entity => compAltPar_entity.name);
        }

        public void MigrateDatabase()
        {
            Database.Migrate();
        }
    }
}
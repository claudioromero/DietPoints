using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using CloudX.DietPoints.Domain.Model;
using CloudX.DietPoints.Domain.Migrations;
using System;

namespace CloudX.DietPoints.Domain
{
    public class ModelDbContext : IdentityDbContext<ApplicationUser>
    {
        public ModelDbContext()
            : base("DefaultConnection", false)
        {
            Entries = Set<Entry>();
            FoodTypes = Set<FoodType>();
            DietPointRules = Set<DietPointRule>();

            if (!Database.Exists())
            {
                throw new Exception("Could not find database specified in connection. Please create it or check if there is a typo in the name.");
            }

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ModelDbContext, Configuration>());
        }

        public static ModelDbContext Create()
        {
            return new ModelDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }

        public IDbSet<Entry> Entries { get; protected set; }
        public IDbSet<FoodType> FoodTypes { get; protected set; }
        public IDbSet<DietPointRule> DietPointRules { get; protected set; }
    }
}
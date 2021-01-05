using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
           System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DailyPromoterProductSale> DailyPromoterProductSales { get; set; }
        public DbSet<DailyPromoterPlan> DailyPromoterPlans { get; set; }
        public DbSet<DailyPromoterPlanAttachment> DailyPromoterPlanAttachments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDetail> ProjectDetails { get; set; }
        public DbSet<ProjectAttachment> ProjectAttachments { get; set; }
        public DbSet<ProjectDetailPromoter> ProjectDetailPromoters { get; set; }
        public DbSet<ProjectProduct> ProjectProducts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<UserBankCard> UserBankCards { get; set; }
    }
}

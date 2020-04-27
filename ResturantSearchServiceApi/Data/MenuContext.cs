using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantSearchServiceApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResturantSearchServiceApi.Data
{
    public class MenuContext: DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuCategory> MenuCatagories { get; set; }
        public DbSet<MenuType> MenuTypes { get; set; }

        public MenuContext(DbContextOptions<MenuContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MenuItem>(ConfigureMenuItem);
            builder.Entity<MenuCategory>(ConfigureMenuCatagory);
            builder.Entity<MenuType>(ConfigureMenuType);
        }

        private void ConfigureMenuType(EntityTypeBuilder<MenuType> builder)
        {
            builder.ToTable("MenuType");

            builder.HasKey(ci => ci.id);

            builder.Property(ci => ci.id)
               .ForSqlServerUseSequenceHiLo("MenuType_hilo")
               .IsRequired();

            builder.Property(cb => cb.FoodType)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureMenuCatagory(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.ToTable("MenuCategory");

            builder.HasKey(ci => ci.id);

            builder.Property(ci => ci.id)
               .ForSqlServerUseSequenceHiLo("MenuCategory_hilo")
               .IsRequired();

            builder.Property(cb => cb.Category)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureMenuItem(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItem");

            builder.Property(ci => ci.id)
                .ForSqlServerUseSequenceHiLo("MenuItem_hilo")
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired(true)
                .HasMaxLength(100);
            builder.Property(ci => ci.Description)
              .IsRequired(false);

            builder.Property(ci => ci.Price)
                .IsRequired(true);
          

            builder.HasOne(ci => ci.MenuType)
                .WithMany()
                .HasForeignKey(ci => ci.MenuTypeID);

            builder.HasOne(ci => ci.MenuCategory)
                .WithMany()
                .HasForeignKey(ci => ci.MenuCategoryId);
        }
    }
}

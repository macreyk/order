using ResturantSearchServiceApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResturantSearchServiceApi.Data
{
    public class MenuDataSeed
    {
        public static async Task SeedAsync(MenuContext context)
        {
            if (!context.MenuTypes.Any())
            {
                context.MenuTypes.AddRange(GetPreconfiguredMenuTypes());
                await context.SaveChangesAsync();

            }
            if (!context.MenuCatagories.Any())
            {
                context.MenuCatagories.AddRange(GetPreconfiguredMenuCatagories());
                await context.SaveChangesAsync();

            }
            if (!context.MenuItems.Any())
            {
                context.MenuItems.AddRange(GetPreconfiguredMenuItems());
                await context.SaveChangesAsync();

            }

        }
        static IEnumerable<MenuType> GetPreconfiguredMenuTypes()
        {
            return new List<MenuType>()
            {
               new MenuType(){ FoodType="VEG"},
               new MenuType(){ FoodType="NON-VEG"}

            };
        }

        static IEnumerable<MenuCategory> GetPreconfiguredMenuCatagories()
        {
            return new List<MenuCategory>()
            {
                new MenuCategory() { Category = "INDIAN"},
                new MenuCategory() { Category = "CHINESE"},
                new MenuCategory() { Category = "CONTINENTAL"},

            };
        }

        static IEnumerable<MenuItem> GetPreconfiguredMenuItems()
        {
            return new List<MenuItem>()
            {
                new MenuItem() { Name = "Rice",Description = "White rice",  Price = 15,MenuTypeID=1, MenuCategoryId=1 },
                new MenuItem() { Name = "Dal",Description = "Plain Dal",  Price = 20,MenuTypeID=1,MenuCategoryId=1 },
                new MenuItem() { Name = "Curry",Description = "Mix Veg Curry",  Price = 50,MenuTypeID=1,MenuCategoryId=1 },
                new MenuItem() { Name = "ChickenMasala",Description = "Chicken curry 5 pc",  Price = 150,MenuTypeID=2,MenuCategoryId=1 },
                new MenuItem() { MenuTypeID=2,MenuCategoryId=1,Name = "MuttonMasala",Description = "Mutton 5 pc",  Price = 250 },
                new MenuItem() { MenuTypeID=1,MenuCategoryId=2,Name = "Veg Noodles",Description = "Veg Noodles",  Price = 80 },
                new MenuItem() { MenuTypeID=1,MenuCategoryId=2,Name = "Chilly Mushroom",Description = "Chilly Mushroom 10 pc",  Price = 150 },
                new MenuItem() { MenuTypeID=2,MenuCategoryId=2,Name = "Chilly chicken",Description = "Cilly chicken 8 pc",  Price = 250 },
                new MenuItem() { MenuTypeID=2,MenuCategoryId=2,Name = "Mixed non-veg noodles",Description = "Mixed nonveg noodles",  Price = 150 },
                new MenuItem() { MenuTypeID=1,MenuCategoryId=3,Name = "Continental Rice",Description = "Continental Rice",  Price = 150 },
                new MenuItem() { MenuTypeID=1,MenuCategoryId=3,Name = "Continental sizzler",Description = "Continental sizzler",  Price = 150 },
                new MenuItem() { MenuTypeID=2,MenuCategoryId=3,Name = "Chicken jalpeno",Description = "Chicken jalpeno",  Price = 300 },
                new MenuItem() { MenuTypeID=2,MenuCategoryId=3,Name = "Fish jalpeno",Description = "Fish jalpeno",  Price = 300 },

            };
        }
    }
}

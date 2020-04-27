using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResturantSearchServiceApi.Domain
{
    public class MenuItem
    {
        public int id { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MenuTypeID { get; set; }
        public int MenuCategoryId { get; set; }
        public MenuType MenuType { get; set; }
        public MenuCategory MenuCategory { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ResturantSearchServiceApi.Data;
using ResturantSearchServiceApi.Domain;
using ResturantSearchServiceApi.ViewModels;

namespace ResturantSearchServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuContext _menuContext;
        private readonly IOptionsSnapshot<MenuSettings> _settings;
        public MenuController(MenuContext menuContext,IOptionsSnapshot<MenuSettings> settings)
        {
            _menuContext = menuContext;
            _settings = settings;
            string url = settings.Value.externalBaseurl; 
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> MenuTypes()
        {
            var items = await _menuContext.MenuTypes.ToListAsync();
            return Ok(items);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> MenuCategory()
        {
            var items = await _menuContext.MenuCatagories.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _menuContext.MenuItems.SingleOrDefaultAsync(ci => ci.id == id);
            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery]int pageSize = 6, [FromQuery]int pageIndex = 0)

        {
            var totalItems = await _menuContext.MenuItems
                .LongCountAsync();

            var itemsOnPage = await _menuContext.MenuItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

           // itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<MenuItem>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery]int pageSize = 6, [FromQuery]int pageIndex = 0)
        {

            var totalItems = await _menuContext.MenuItems
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _menuContext.MenuItems
                .Where(c => c.Name.StartsWith(name))
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            //itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<MenuItem>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }


        [HttpGet]
        [Route("[action]/type/{MenuTypeId}/category/{MenuCategoryID}")]
        public async Task<IActionResult> Items(int? MenuTypeId, int? menuCategoryId, [FromQuery]int pageSize = 6, [FromQuery]int pageIndex = 0)
        {
            var root = (IQueryable<MenuItem>)_menuContext.MenuItems;

            if (MenuTypeId.HasValue)
            {
                root = root.Where(ci => ci.MenuTypeID == MenuTypeId);
            }

            if (menuCategoryId.HasValue)
            {
                root = root.Where(ci => ci.MenuCategoryId == menuCategoryId);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .OrderBy(c => c.Name)
                .Take(pageSize)
                .ToListAsync();

           // itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<MenuItem>(
                pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody]MenuItem productToUpdate)
        {
            var MenuItem = await _menuContext.MenuItems
                .SingleOrDefaultAsync(i => i.id == productToUpdate.id);

            if (MenuItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.id} not found." });
            }

            //var oldPrice = catalogItem.Price;
            //var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;


            // Update current product
            MenuItem = productToUpdate;
            _menuContext.MenuItems.Update(MenuItem);


            await _menuContext.SaveChangesAsync();


            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.id }, null);
        }

        //POST api/[controller]/items
        [Route("items")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]MenuItem product)
        {
            var item = new MenuItem
            {
                MenuTypeID = product.MenuTypeID,
                MenuCategoryId = product.MenuCategoryId,
                Description = product.Description,
                Name = product.Name,
                //PictureFileName = product.PictureFileName,
                Price = product.Price
            };
            _menuContext.MenuItems.Add(item);

            await _menuContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.id }, null);
        }

        //DELETE api/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _menuContext.MenuItems.SingleOrDefault(x => x.id == id);

            if (product == null)
            {
                return NotFound();
            }

            _menuContext.MenuItems.Remove(product);

            await _menuContext.SaveChangesAsync();

            return NoContent();
        }


    }

}
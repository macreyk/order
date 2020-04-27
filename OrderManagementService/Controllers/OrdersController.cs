using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderManagementService.Data;
using OrderManagementService.Model;

namespace OrderManagementService.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersContext _ordersContext;
        private readonly IOptionsSnapshot<OrderSettings> _settings;


        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrdersContext ordersContext, ILogger<OrdersController> logger)
        {
            //_settings = settings;
            // _ordersContext = ordersContext;
            _ordersContext = ordersContext ?? throw new ArgumentNullException(nameof(ordersContext));

            ((DbContext)ordersContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _logger = logger;
        }

        [Route("new")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var envs = Environment.GetEnvironmentVariables();
            var conString = _settings.Value.ConnectionString;
            _logger.LogInformation($"{conString}");

            order.OrderStatus = OrderStatus.Preparing;
            order.OrderDate = DateTime.UtcNow;

            _logger.LogInformation(" In Create Order");
            _logger.LogInformation(" Order" + order.UserName);


            _ordersContext.Orders.Add(order);
            _ordersContext.OrderItems.AddRange(order.OrderItems);

            _logger.LogInformation(" Order added to context");
            _logger.LogInformation(" Saving........");
            try
            {
                await _ordersContext.SaveChangesAsync();
                return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("An error occored during Order saving .." + ex.Message);
                return BadRequest();
            }


        }

        [HttpGet("{id}", Name = "GetOrder")]
        //  [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrder(int id)
        {

            var item = await _ordersContext.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefaultAsync(ci => ci.OrderId == id);
            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();

        }



        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _ordersContext.Orders.ToListAsync();

            // var orders = await orderTask;

            return Ok(orders);
        }

        [Route("ChangeOrder")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] Order changeorder)
        {
            var orderItem = await _ordersContext.Orders
                .SingleOrDefaultAsync(i => i.OrderId == changeorder.OrderId);

            if (orderItem == null)
            {
                return NotFound(new { Message = $"Order with id {changeorder.OrderId} not found." });
            }

            // Update current product
            orderItem = changeorder;
            _ordersContext.Orders.Update(orderItem);

            try
            {
                await _ordersContext.SaveChangesAsync();
                return CreatedAtRoute("GetOrder", new { id = changeorder.OrderId }, changeorder);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("An error occored during Order Updating .." + ex.Message);
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("ViewOrders")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrdersByUser([FromQuery] string userName)
        {
            var items = await _ordersContext.Orders
                .Where(ci => ci.UserName.Equals(userName))
                .Include(x => x.OrderItems)
                .ToListAsync();
            if (items != null)
            {
                return Ok(items);
            }


            return NotFound();

        }

        [Route("CancelOrder/{id}")]
        [HttpDelete]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var product = _ordersContext.Orders.SingleOrDefault(x => x.OrderId == id);

            if (product == null)
            {
                return NotFound();
            }

            _ordersContext.Orders.Remove(product);

            await _ordersContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
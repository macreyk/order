using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewManagementService.Data;
using ReviewManagementService.Model;

namespace ReviewManagementService.Controllers
{
    [Produces("application/json")]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewContext _reviewContext;
        //private readonly IOptionsSnapshot<OrderSettings> _settings;

        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ReviewContext reviewContext, ILogger<ReviewController> logger)
        {
            //_settings = settings;
            // _ordersContext = ordersContext;
            _reviewContext = reviewContext ?? throw new ArgumentNullException(nameof(reviewContext));

            ((DbContext)reviewContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _logger = logger;
        }
        [HttpGet]
        [Route("GetReview")]
        public async Task<ActionResult> GetReview(int restaurentid)
        {
            var Rid = await _reviewContext.Rating.Where(x => x.restaurentid == restaurentid).ToListAsync();
            return Ok(Rid);
        }
        [HttpGet]
        [Route("[action]{id}")]
        public async Task<ActionResult> GetReviewByID(int id)
        {
            var Rid = await _reviewContext.Rating.Where(x => x.rateid == id).ToListAsync();
            return Ok(Rid);
        }



        [HttpPut]
        [Route("ReviewUpdate")]
        public async Task<ActionResult> ReviewUpdate([FromBody]Rating raterUpdate)
        {
            var review = await _reviewContext.Rating.SingleOrDefaultAsync(x => x.rateid == raterUpdate.rateid);
            if (review == null)
            {
                return NotFound(new { Message = $"Reviewid{ raterUpdate.rateid  }notfound" });
            }
            review = raterUpdate;
            _reviewContext.Rating.Update(review);
            await _reviewContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviewByID), new { id = raterUpdate.rateid }, null);
        }
        [HttpPost]
        [Route("AddReview")]
        public async Task<ActionResult> AddReview([FromBody]Rating ratereview)
        {
            var Ratereview = new Rating
            {
                rateid = ratereview.rateid,
                rating = ratereview.rating,
                review = ratereview.review,
                userid = ratereview.userid,
                ratedate = DateTime.Now
            };
            _reviewContext.Rating.Add(Ratereview);
            await _reviewContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviewByID), new { id = Ratereview.rateid }, null);
        }




        [HttpDelete("{rateid}")]
        public async Task<ActionResult<Rating>> DeleteRate(int rateid)
        {
            var rateReview = await _reviewContext.Rating.FindAsync(rateid);
            if (rateReview == null)
            {
                return NotFound();
            }



            _reviewContext.Rating.Remove(rateReview);
            await _reviewContext.SaveChangesAsync();



            return rateReview;
        }

    }
}
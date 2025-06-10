using BidService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BidService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : Controller
    {
        private readonly BidCreateService _bidCommandService;
        private readonly BidService.Services.BidService _bidService; 

        public BidsController(BidCreateService bidCommandService, BidService.Services.BidService bidService)
        {
            _bidCommandService = bidCommandService;
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetAll()
        {
            var bids = await _bidService.GetAllAsync();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidDto>> GetById(Guid id)
        {
            var bid = await _bidService.GetByIdAsync(id);
            if (bid == null)
                return NotFound();
            return Ok(bid);
        }

        [HttpPost]
        public async Task<ActionResult<BidDto>> CreateBid([FromBody] CreateBidDto dto)
        {
            var bid = await _bidCommandService.CreateBidAsync(dto);
            return CreatedAtAction(nameof(CreateBid), new { id = bid.Id }, bid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BidDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var updated = await _bidService.UpdateAsync(dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _bidService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

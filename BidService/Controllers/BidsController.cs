using BidService.DTO;
using BidService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BidService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BidsController : ControllerBase
    {
        private readonly BidService.Services.BidService _bidService;

        public BidsController(BidService.Services.BidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BidDto>>> GetAllBids()
        {
            var bids = await _bidService.GetAllBidsAsync();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidDto>> GetBidById(Guid id)
        {
            var bid = await _bidService.GetBidByIdAsync(id);
            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }

        [HttpPost]
        public async Task<ActionResult<BidDto>> CreateBid([FromBody] CreateBidDto dto)
        {
            var bid = await _bidService.CreateBidAsync(dto);

            var bidDto = await _bidService.GetBidByIdAsync(bid.Id);

            return CreatedAtAction(nameof(GetBidById), new { id = bid.Id }, bidDto);
        }

        [HttpDelete("clear-all")]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> ClearAllBids()
        {
            await _bidService.ClearAllBidsAsync();
            return Ok(new { message = "Все заявки были успешно удалены." });
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ps_globomantics_signalr.Repositories;

namespace ps_globomantics_signalr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        public IAuctionRepo AuctionRepo { get; }

        public AuctionController(IAuctionRepo auctionRepo)
        {
            AuctionRepo = auctionRepo;
        }

        [HttpGet("Auctions")]
        public IActionResult GetAuctions()
        {
            return Ok(AuctionRepo.GetAll());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SlotLab.Engine.Core; 


namespace SlotLab.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameManager _gameManager;

        public GameController(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        // Endpoint bàsic de prova
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "✅ GameController operational" });
        }

        // Endpoint per fer un spin (equivalent al que teníem abans)
        [HttpGet("spin")]
        public IActionResult Spin()
        {
            var result = _gameManager.Spin();
            return Ok(result);
        }
    }

}
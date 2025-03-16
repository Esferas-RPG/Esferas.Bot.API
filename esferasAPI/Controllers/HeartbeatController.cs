using System.Runtime.CompilerServices;
using apiEsferas.Application.DTO;
using apiEsferas.Application.Sevices;
using apiEsferas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace apiEsferas.Controllers
{
    [ApiController]
    [Route("/[controller]")]

    public class HeartbeatController : ControllerBase
    {
        public HeartbeatController()
        {
        }

        [HttpGet()]
        public IActionResult GetHeartbeat()
        {
            return Ok(new
            {
                Status = "Alive!"
            });
        }


    }
}

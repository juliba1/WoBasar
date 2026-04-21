using Microsoft.AspNetCore.Mvc;
using WoBasar.Shared.Models;

namespace WoBasar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public List<MDLTest> Get()
        {
            return new List<MDLTest>
            {
                new MDLTest { 
                    Id = 1,
                    Title = "Schreibtisch zu verkaufen",
                    Description = "IKEA Tisch, sehr guter Zustand",
                    Price = 50,
                    Category = "Möbel",
                    CreatedAt = DateTime.UtcNow 
                }, new MDLTest { 
                    Id = 2,
                    Title = "Mathebuch",
                    Description = "Analysis 1 für Studenten",
                    Price = 20,
                    Category = "Bücher",
                    CreatedAt = DateTime.UtcNow 
                }
            };
        }
    }
}

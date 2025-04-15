using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Infrastructure;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly TaskManagementDbContext _context;

        public ReportsController(TaskManagementDbContext context)
        {
            _context = context;
        }


    }
}
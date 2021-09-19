using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IPReport.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WuLeiController : ControllerBase
    {
        private static string _ip = "";
        private readonly ILogger<WuLeiController> _logger;

        public WuLeiController(ILogger<WuLeiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetRemoteIP()
        {
            _logger.LogInformation("Someone request and get ip: {0}", _ip);

            return Ok(_ip);
        }

        [HttpPost]
        public IActionResult SetRemoteIP(string ip)
        {
            _logger.LogInformation("Receive reported ip: {0}", _ip);

            _ip = ip;

            return Ok();
        }

    }
}

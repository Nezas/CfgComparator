using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CfgComparator.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace CfgComparator.API.Controllers
{
    [ApiController]
    [Route("configurationFile")]
    public class ConfigurationFileController : ControllerBase
    {
        private readonly ILogger<ConfigurationFileController> _logger;

        public ConfigurationFileController(ILogger<ConfigurationFileController> logger)
        {
            _logger = logger;
        }

        [HttpPost("compare")]
        public ActionResult<ConfigurationsCompareResult> Compare(IFormFile sourceConfigurationFile, IFormFile targetConfigurationFile)
        {
            if(sourceConfigurationFile == null || targetConfigurationFile == null)
            {
                return BadRequest("Both \".cfg\" files should be uploaded!");
            }
            
            if(Path.GetExtension(sourceConfigurationFile.FileName) != ".cfg" || Path.GetExtension(targetConfigurationFile.FileName) != ".cfg")
            {
                return BadRequest("Only \".cfg\" files are supported!");
            }

            var source = ConfigurationFileReader.ReadFromFile(sourceConfigurationFile.FileName, sourceConfigurationFile.OpenReadStream());
            var target = ConfigurationFileReader.ReadFromFile(targetConfigurationFile.FileName, targetConfigurationFile.OpenReadStream());

            ConfigurationsComparator configurationsComparator = new();

            var configurationsCompareResult = configurationsComparator.Compare(source, target);

            return Ok(configurationsCompareResult);
        }
    }
}

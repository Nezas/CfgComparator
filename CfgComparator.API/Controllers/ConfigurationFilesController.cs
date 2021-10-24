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
using Microsoft.Extensions.Caching.Memory;
using CfgComparator.API.Cache;
using CfgComparator.API.Models;
using CfgComparator.API.Services;
using CfgComparator.Enums;

namespace CfgComparator.API.Controllers
{
    [ApiController]
    [Route("configurationFiles")]
    public class ConfigurationFilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ConfigurationFilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public ActionResult Upload(IFormFile sourceFile, IFormFile targetFile)
        {
            if(sourceFile == null || targetFile == null)
            {
                return BadRequest("Both files should be uploaded!");
            }
            if(_fileService.UploadFiles(sourceFile, targetFile) == false)
            {
                return BadRequest("Only \".cfg\" files are supported!");
            }

            return Ok("Files were successfully uploaded!");
        }

        [HttpGet("compare")]
        public ActionResult<ConfigurationFilesResult> Compare()
        {
            var configurationFilesResult = _fileService.CompareFiles();
            return configurationFilesResult == null ? BadRequest("Files were not uploaded!") : Ok(configurationFilesResult);
        }

        [HttpGet("filter/{status}")]
        public ActionResult<List<ParameterDifference>> Filter(ParameterStatus status)
        {
            var parameterDifferences = _fileService.FilterByStatus(status);
            return parameterDifferences == null ? BadRequest("Files were not compared!") : Ok(parameterDifferences);
        }
    }
}

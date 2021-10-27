using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.API.Services;
using CfgComparator.API.Models;

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
            if(_fileService.UploadAndCompareFiles(sourceFile, targetFile) == false)
            {
                return BadRequest("Only \".cfg\" files are supported!");
            }

            return Ok("Files were successfully uploaded!");
        }

        [HttpGet("result")]
        public ActionResult<ConfigurationFilesResult> Result()
        {
            var result = _fileService.GetCompareResult();
            return result == null ? BadRequest("Files were not uploaded yet!") : Ok(result);
        }

        [HttpGet("filterStatus/{status}")]
        public ActionResult<List<ParameterDifference>> FilterByStatus(ParameterStatus status)
        {
            var parameterDifferences = _fileService.FilterByStatus(status);
            return parameterDifferences == null ? BadRequest("Files were not uploaded yet!") : Ok(parameterDifferences);
        }

        [HttpGet("filterId/{id}")]
        public ActionResult<List<ParameterDifference>> FilterById(string id)
        {
            var parameterDifferences = _fileService.FilterById(id);
            return parameterDifferences == null ? BadRequest("Files were not uploaded yet!") : Ok(parameterDifferences);
        }
    }
}

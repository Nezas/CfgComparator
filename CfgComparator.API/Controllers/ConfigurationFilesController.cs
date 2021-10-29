using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
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
            if(Path.GetExtension(sourceFile.FileName) != ".cfg" || Path.GetExtension(targetFile.FileName) != ".cfg")
            {
                return BadRequest("Only \".cfg\" files are supported!");
            }
            _fileService.ReadAndCompareFiles(sourceFile, targetFile);

            var session = HttpContext.Session;
            session.SetString("SourceFileName", sourceFile.FileName);
            session.SetString("TargetFileName", targetFile.FileName);

            return Ok("Files were successfully uploaded!");
        }

        [HttpGet("result")]
        public ActionResult<ConfigurationFilesResult> Result()
        {
            var session = HttpContext.Session;
            var result = _fileService.GetCompareResult(session.GetString("SourceFileName"), session.GetString("TargetFileName"));
            return result == null ? BadRequest("Files were not uploaded yet!") : Ok(result);
        }

        [HttpGet("filterStatus/{status}")]
        public ActionResult<List<ParameterDifference>> FilterByStatus(ParameterStatus status)
        {
            var session = HttpContext.Session;
            var parameterDifferences = _fileService.FilterByStatus(session.GetString("SourceFileName"), session.GetString("TargetFileName"), status);
            return parameterDifferences == null ? BadRequest("Files were not uploaded yet!") : Ok(parameterDifferences);
        }

        [HttpGet("filterId/{id}")]
        public ActionResult<List<ParameterDifference>> FilterById(string id)
        {
            var session = HttpContext.Session;
            var parameterDifferences = _fileService.FilterById(session.GetString("SourceFileName"), session.GetString("TargetFileName"), id);
            return parameterDifferences == null ? BadRequest("Files were not uploaded yet!") : Ok(parameterDifferences);
        }
    }
}

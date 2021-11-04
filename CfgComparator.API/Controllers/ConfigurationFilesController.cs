using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.API.Services;
using CfgComparator.API.Models;
using CfgComparator.Readers;

namespace CfgComparator.API.Controllers
{
    [ApiController]
    [Route("configurationFiles")]
    public class ConfigurationFilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFileReader _fileReader;

        public ConfigurationFilesController(IFileService fileService, IFileReader fileReader)
        {
            _fileService = fileService;
            _fileReader = fileReader;
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
            _fileService.ReadAndCompareFiles(sourceFile, targetFile, _fileReader);

            var session = HttpContext.Session;
            session.SetString("SourceFileName", sourceFile.FileName);
            session.SetString("TargetFileName", targetFile.FileName);

            return Ok("Files were successfully uploaded!");
        }

        [HttpGet("result")]
        public ActionResult<ConfigurationFilesResult> Result()
        {
            var result = GetSessionResult();
            return result is null ? BadRequest("Files were not found!") : Ok(result);
        }

        [HttpGet("filterStatus/{status}")]
        public ActionResult<List<ParameterDifference>> FilterByStatus(ParameterStatus status)
        {
            var result = GetSessionResult();
            return result is null ? BadRequest("Files were not found!") : Ok(_fileService.FilterByStatus(result, status));
        }

        [HttpGet("filterId/{id}")]
        public ActionResult<List<ParameterDifference>> FilterById(string id)
        {
            var result = GetSessionResult();
            return result is null ? BadRequest("Files were not found!") : Ok(_fileService.FilterById(result, id));
        }

        private ConfigurationFilesResult GetSessionResult()
        {
            var session = HttpContext.Session;
            try
            {
                return _fileService.GetCompareResult(session.GetString("SourceFileName"), session.GetString("TargetFileName"));
            }
            catch(FileNotFoundException)
            {
                return null;
            }
        }
    }
}

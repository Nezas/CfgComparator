using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CfgComparator.API.Models;
using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        bool UploadAndCompareFiles(IFormFile sourceFile, IFormFile targetFile);
        void Compare(ConfigurationFile source, ConfigurationFile target);
        ConfigurationFilesResult GetCompareResult();
        List<ParameterDifference> FilterByStatus(ParameterStatus status);
        List<ParameterDifference> FilterById(string id);
    }
}
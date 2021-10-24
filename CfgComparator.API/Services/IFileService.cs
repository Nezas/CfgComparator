using Microsoft.AspNetCore.Http;
using CfgComparator.API.Models;
using System.Collections.Generic;
using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        bool UploadFiles(IFormFile sourceFile, IFormFile targetFile);
        ConfigurationFilesResult CompareFiles();
        List<ParameterDifference> FilterByStatus(ParameterStatus status);
    }
}
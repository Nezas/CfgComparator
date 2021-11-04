using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CfgComparator.API.Models;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.Readers;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        void ReadAndCompareFiles(IFormFile sourceFile, IFormFile targetFile, IFileReader fileReader);
        void Compare(ConfigurationFile source, ConfigurationFile target);
        void SaveCompareResult(string sourceName, string targetName, ConfigurationFilesResult configurationFilesResult);
        ConfigurationFilesResult GetCompareResult(string sourceName, string targetName);
        IEnumerable<ParameterDifference> FilterByStatus(ConfigurationFilesResult result, ParameterStatus status);
        IEnumerable<ParameterDifference> FilterById(ConfigurationFilesResult result, string id);
    }
}
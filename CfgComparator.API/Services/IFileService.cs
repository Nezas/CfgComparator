using Microsoft.AspNetCore.Http;
using CfgComparator.API.Models;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        bool UploadFiles(IFormFile sourceFile, IFormFile targetFile);
        ConfigurationFilesResult CompareFiles();
    }
}
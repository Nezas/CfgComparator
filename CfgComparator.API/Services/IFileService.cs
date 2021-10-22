using Microsoft.AspNetCore.Http;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        bool UploadFiles(IFormFile sourceFile, IFormFile targetFile);
        object ReturnFiles();
    }
}
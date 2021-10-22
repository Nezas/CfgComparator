using CfgComparator.API.Cache;
using CfgComparator.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;

namespace CfgComparator.API.Services
{
    public class ConfigurationFilesService : IFileService
    {
        private readonly IMemoryCache _memoryCache;

        public ConfigurationFilesService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool UploadFiles(IFormFile sourceFile, IFormFile targetFile)
        {
            if(Path.GetExtension(sourceFile.FileName) != ".cfg" || Path.GetExtension(targetFile.FileName) != ".cfg")
            {
                return false;
            }

            _memoryCache.Set(CacheKeys.ConfigurationFileNames, new ConfigurationFiles(sourceFile.FileName, targetFile.FileName), TimeSpan.FromMinutes(30));
            return true;
        }

        public object ReturnFiles()
        {
            return _memoryCache.Get(CacheKeys.ConfigurationFileNames);
        }
    }
}

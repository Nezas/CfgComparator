﻿using CfgComparator.API.Cache;
using CfgComparator.API.Models;
using CfgComparator.Models;
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

            var source = ConfigurationFileReader.ReadFromFile(sourceFile.FileName, sourceFile.OpenReadStream());
            var target = ConfigurationFileReader.ReadFromFile(targetFile.FileName, targetFile.OpenReadStream());
            _memoryCache.Set(CacheKeys.Source, source, TimeSpan.FromMinutes(30));
            _memoryCache.Set(CacheKeys.Target, target, TimeSpan.FromMinutes(30));

            return true;
        }

        public ConfigurationFilesResult CompareFiles()
        {
            var source = _memoryCache.Get(CacheKeys.Source);
            var target = _memoryCache.Get(CacheKeys.Target);

            if(source == null || target == null)
            {
                return null;
            }

            var sourceFile = (ConfigurationFile)source;
            var targetFile = (ConfigurationFile)target;

            var configurationsParameterDifferences = ConfigurationsComparator.Compare(sourceFile.Parameters, targetFile.Parameters);
            var configurationsInfoParameterDifferences = ConfigurationsComparator.Compare(sourceFile.InfoParameters, targetFile.InfoParameters);

            var configurationFilesResult = new ConfigurationFilesResult(sourceFile.Name, targetFile.Name);
            configurationFilesResult.InfoParameters = configurationsInfoParameterDifferences;
            configurationFilesResult.Parameters = configurationsParameterDifferences;

            return configurationFilesResult;
        }
    }
}

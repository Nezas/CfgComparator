﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.API.Models;
using CfgComparator.API.Cache;

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

            var configurationFilesResult = new ConfigurationFilesResult(sourceFile.Name, targetFile.Name);
            configurationFilesResult.Parameters = ConfigurationsComparator.Compare(sourceFile.Parameters, targetFile.Parameters);
            configurationFilesResult.InfoParameters = ConfigurationsComparator.Compare(sourceFile.InfoParameters, targetFile.InfoParameters);

            _memoryCache.Set(CacheKeys.ConfigurationFilesResult, configurationFilesResult, TimeSpan.FromMinutes(30));

            return configurationFilesResult;
        }

        public List<ParameterDifference> FilterByStatus(ParameterStatus status)
        {
            var configurationFilesResult = _memoryCache.Get(CacheKeys.ConfigurationFilesResult);
            if(configurationFilesResult == null)
            {
                return null;
            }
            var result = (ConfigurationFilesResult)configurationFilesResult;

            var parameterDifferences = new List<ParameterDifference>();
            foreach(var parameter in result.Parameters)
            {
                if(parameter.Status == status)
                {
                    parameterDifferences.Add(parameter);
                }
            }
            return parameterDifferences;
        }

        public List<ParameterDifference> FilterById(string id)
        {
            var configurationFilesResult = _memoryCache.Get(CacheKeys.ConfigurationFilesResult);
            if(configurationFilesResult == null)
            {
                return null;
            }
            var result = (ConfigurationFilesResult)configurationFilesResult;

            var parameterDifferences = result.Parameters.FindAll(p => p.Id.StartsWith(id));
            return parameterDifferences;
        }
    }
}
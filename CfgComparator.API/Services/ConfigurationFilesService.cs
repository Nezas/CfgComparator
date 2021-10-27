using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
        
        public bool UploadAndCompareFiles(IFormFile sourceFile, IFormFile targetFile)
        {
            if(Path.GetExtension(sourceFile.FileName) != ".cfg" || Path.GetExtension(targetFile.FileName) != ".cfg")
            {
                return false;
            }

            var source = ConfigurationFileReader.ReadFromFile(sourceFile.FileName, sourceFile.OpenReadStream());
            var target = ConfigurationFileReader.ReadFromFile(targetFile.FileName, targetFile.OpenReadStream());

            Thread thread = new(() => Compare(source, target));
            thread.Start();
            return true;
        }

        public void Compare(ConfigurationFile source, ConfigurationFile target)
        {
            var configurationFilesResult = new ConfigurationFilesResult(source.Name, target.Name);
            configurationFilesResult.Parameters = ConfigurationsComparator.Compare(source.Parameters, target.Parameters);
            configurationFilesResult.InfoParameters = ConfigurationsComparator.Compare(source.InfoParameters, target.InfoParameters);
            _memoryCache.Set(CacheKeys.ConfigurationFilesResult, configurationFilesResult);
        }

        public ConfigurationFilesResult GetCompareResult()
        {
            var result = _memoryCache.Get(CacheKeys.ConfigurationFilesResult);
            return (ConfigurationFilesResult)result;
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

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.API.Models;
using Newtonsoft.Json;

namespace CfgComparator.API.Services
{
    public class ConfigurationFilesService : IFileService
    {
        public void ReadAndCompareFiles(IFormFile sourceFile, IFormFile targetFile)
        {
            var source = ConfigurationFileReader.ReadFromFile(sourceFile.FileName, sourceFile.OpenReadStream());
            var target = ConfigurationFileReader.ReadFromFile(targetFile.FileName, targetFile.OpenReadStream());

            Thread thread = new(() => Compare(source, target));
            thread.Start();
        }

        public void Compare(ConfigurationFile source, ConfigurationFile target)
        {
            var configurationFilesResult = new ConfigurationFilesResult(source.Name, target.Name);
            configurationFilesResult.Parameters = ConfigurationsComparator.Compare(source.Parameters, target.Parameters);
            configurationFilesResult.InfoParameters = ConfigurationsComparator.Compare(source.InfoParameters, target.InfoParameters);
            SaveCompareResult(source.Name, target.Name, configurationFilesResult);
        }

        public void SaveCompareResult(string sourceName, string targetName, ConfigurationFilesResult configurationFilesResult)
        {
            string result = JsonConvert.SerializeObject(configurationFilesResult, Formatting.Indented);
            string writePath = "Files/" + sourceName + "_" + targetName + ".json";
            File.WriteAllText(writePath, result);
        }

        public ConfigurationFilesResult GetCompareResult(string sourceName, string targetName)
        {
            string json = "Files/" + sourceName + "_" + targetName + ".json";
            string jsonValue = File.ReadAllText(json);
            var result = JsonConvert.DeserializeObject<ConfigurationFilesResult>(jsonValue);
            return result;
        }

        public List<ParameterDifference> FilterByStatus(string sourceName, string targetName, ParameterStatus status)
        {
            var configurationFilesResult = GetCompareResult(sourceName, targetName);
            if(configurationFilesResult == null)
            {
                return null;
            }

            var parameterDifferences = new List<ParameterDifference>();
            foreach(var parameter in configurationFilesResult.Parameters)
            {
                if(parameter.Status == status)
                {
                    parameterDifferences.Add(parameter);
                }
            }
            return parameterDifferences;
        }

        public List<ParameterDifference> FilterById(string sourceName, string targetName, string id)
        {
            var configurationFilesResult = GetCompareResult(sourceName, targetName);
            if(configurationFilesResult == null)
            {
                return null;
            }

            var parameterDifferences = configurationFilesResult.Parameters.FindAll(p => p.Id.StartsWith(id));
            return parameterDifferences;
        }
    }
}

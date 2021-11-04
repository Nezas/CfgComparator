using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using CfgComparator.Models;
using CfgComparator.Enums;
using CfgComparator.Readers;
using CfgComparator.API.Models;

namespace CfgComparator.API.Services
{
    public class ConfigurationFilesService : IFileService
    {
        public void ReadAndCompareFiles(IFormFile sourceFile, IFormFile targetFile, IFileReader fileReader)
        {
            var source = fileReader.ReadFromFile(sourceFile.FileName, sourceFile.OpenReadStream());
            var target = fileReader.ReadFromFile(targetFile.FileName, targetFile.OpenReadStream());

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
            string resultPath = "Files/" + sourceName + "_" + targetName + ".json";
            string resultValue = File.ReadAllText(resultPath);
            var result = JsonConvert.DeserializeObject<ConfigurationFilesResult>(resultValue);
            return result;
        }

        public IEnumerable<ParameterDifference> FilterByStatus(ConfigurationFilesResult result, ParameterStatus status)
        {
            return result.Parameters.Where(parameter => parameter.Status == status);
        }

        public IEnumerable<ParameterDifference> FilterById(ConfigurationFilesResult result, string id)
        {
            return result.Parameters.FindAll(p => p.Id.StartsWith(id));
        }
    }
}

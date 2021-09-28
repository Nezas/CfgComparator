using System.IO;
using System.IO.Compression;
using System.Linq;
using CfgComparator.Models;

namespace CfgComparator
{
    /// <summary>
    /// Reads configuration file data.
    /// </summary>
    public static class ConfigurationFileReader
    {
        /// <summary>
        /// Reads data from the given configuration file path.
        /// </summary>
        /// <param name="filePath">Configuration file path.</param>
        /// <returns>Returns <see cref="ConfigurationData"/></returns>
        public static ConfigurationData ReadFromFile(string filePath)
        {
            string fileData = Read(filePath);
            ConfigurationData configurationData = new(Path.GetFileName(filePath));

            foreach(var dataPair in fileData.Split(';'))
            {
                var splitPair = dataPair.Split(':');
                if(splitPair.Length == 2)
                {
                    var id = splitPair[0];
                    var value = splitPair[1];

                    Parameter parameter = new(id, value);
                    if(id.All(char.IsDigit))
                    { 
                        configurationData.Parameters.Add(parameter);
                    }
                    else
                    {
                        configurationData.InfoParameters.Add(parameter);
                    }
                }
                else break;
            }
            return configurationData;
        }

        private static string Read(string filePath)
        {
            using(FileStream file = File.Open(filePath, FileMode.Open))
            using(GZipStream zip = new(file, CompressionMode.Decompress))
            using(StreamReader unzip = new(zip))
            return unzip.ReadToEnd();
        }
    }
}

using System;
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
        /// Reads data from the configuration file with the given stream.
        /// </summary>
        /// <param name="fileName">Configuration file name.</param>
        /// <param name="stream">File reading stream.</param>
        /// <returns>Returns <see cref="ConfigurationFile"/></returns>
        public static ConfigurationFile ReadFromFile(string fileName, Stream stream)
        {
            string fileData = Read(stream);
            ConfigurationFile configurationData = new(fileName);

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

        private static string Read(Stream stream)
        {
            using(GZipStream zip = new(stream, CompressionMode.Decompress))
            using(StreamReader unzip = new(zip))
            return unzip.ReadToEnd();
        }
    }
}

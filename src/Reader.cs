using System.IO;
using System.IO.Compression;
using System.Linq;
using CfgComparator.Models;

namespace CfgComparator
{
    /// <summary>
    /// Reads configuration files data.
    /// </summary>
    public static class Reader
    {
        /// <summary>
        /// Reads data from the given configuration file path.
        /// </summary>
        /// <param name="filePath">Configuration file path.</param>
        /// <returns></returns>
        public static Record Read(string filePath)
        {
            string fileData = ReadFromFile(filePath);
            Record record = new(Path.GetFileName(filePath));

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
                        record.Parameters.Add(parameter);
                    }
                    else
                    {
                        record.InfoParameters.Add(parameter);
                    }
                }
                else break;
            }
            return record;
        }

        private static string ReadFromFile(string filePath)
        {
            using(FileStream file = File.Open(filePath, FileMode.Open))
            using(GZipStream zip = new(file, CompressionMode.Decompress))
            using(StreamReader unzip = new(zip))
            return unzip.ReadToEnd();
        }
    }
}

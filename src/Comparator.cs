using System;
using System.Collections.Generic;
using System.Linq;
using CfgComparator.Models;

namespace CfgComparator
{
    public class Comparator
    {
        public List<string> Unchanged { get; } = new();
        public List<string> Added { get; } = new();
        public List<string> Removed { get; } = new();
        public List<string> Modified { get; } = new();

        public void Compare(Record source, Record target)
        {
            foreach(var sourceDataPair in source.Parameters)
            {
                var sourceId = sourceDataPair.Id;
                var sourceValue = sourceDataPair.Value;

                if(sourceId.All(char.IsDigit))
                {
                    var targetData = target.Parameters.Find(x => x.Id == sourceId);
                    if(targetData == null)
                    {
                        Removed.Add(sourceId + " " + sourceValue + " " + " " + "Removed;");
                    }
                    else
                    {
                        if(sourceValue == targetData.Value)
                        {
                            Unchanged.Add(sourceId + " " + sourceValue + " " + targetData.Value + " " + "Unchanged;");
                        }
                        else
                        {
                            Modified.Add(sourceId + " " + sourceValue + " " + targetData.Value + " " + "Modified;");
                        }
                    }
                }
            }

            foreach(var targetDataPair in target.Parameters)
            {
                var targetId = targetDataPair.Id;
                var targetValue = targetDataPair.Value;

                if(targetId.All(char.IsDigit))
                {
                    var sourceData = source.Parameters.Find(x => x.Id == targetId);
                    if(sourceData == null)
                    {
                        Added.Add(targetId + " " + " " + targetValue + " " + "Added;");
                    }
                }
            }
        }
    }
}

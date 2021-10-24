﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CfgComparator.API.Models;
using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator.API.Services
{
    public interface IFileService
    {
        bool UploadFiles(IFormFile sourceFile, IFormFile targetFile);
        ConfigurationFilesResult CompareFiles();
        List<ParameterDifference> FilterByStatus(ParameterStatus status);
        List<ParameterDifference> FilterById(string id);
    }
}
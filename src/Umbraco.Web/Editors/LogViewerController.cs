﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using Umbraco.Core.Logging.Viewer;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Web.Mvc;

namespace Umbraco.Web.Editors
{
    /// <summary>
    /// Backoffice controller supporting the dashboard for viewing logs with some simple graphs & filtering
    /// </summary>
    [PluginController("UmbracoApi")]
    public class LogViewerController : UmbracoAuthorizedJsonController
    {
        private ILogViewer _logViewer;

        public LogViewerController(ILogViewer logViewer)
        {
            _logViewer = logViewer;
        }

        [HttpGet]
        public int GetNumberOfErrors()
        {
            return _logViewer.GetNumberOfErrors(startDate: DateTime.Now.AddDays(-1), endDate: DateTime.Now);
        }

        [HttpGet]
        public LogLevelCounts GetLogLevelCounts()
        {
            return _logViewer.GetLogLevelCounts(startDate: DateTime.Now.AddDays(-1), endDate: DateTime.Now);
        }

        [HttpGet]
        public IEnumerable<LogTemplate> GetMessageTemplates()
        {
            return _logViewer.GetMessageTemplates(startDate: DateTime.Now.AddDays(-1), endDate: DateTime.Now);
        }

        [HttpGet]
        public PagedResult<LogMessage> GetLogs(string orderDirection = "Descending", int pageNumber = 1, string filterExpression = null, [FromUri]string[] logLevels = null)
        {
            var direction = orderDirection == "Descending" ? Direction.Descending : Direction.Ascending;
            return _logViewer.GetLogs(startDate: DateTime.Now.AddDays(-1), endDate: DateTime.Now, filterExpression: filterExpression, pageNumber: pageNumber, orderDirection: direction, logLevels: logLevels);
        }

        [HttpGet]
        public IEnumerable<SavedLogSearch> GetSavedSearches()
        {
            return _logViewer.GetSavedSearches();
        }

        [HttpPost]
        public IEnumerable<SavedLogSearch> PostSavedSearch(SavedLogSearch item)
        {
            return _logViewer.AddSavedSearch(item.Name, item.Query);
        }

        [HttpPost]
        public IEnumerable<SavedLogSearch> DeleteSavedSearch(SavedLogSearch item)
        {
            return _logViewer.DeleteSavedSearch(item.Name, item.Query);
        }
    }
}
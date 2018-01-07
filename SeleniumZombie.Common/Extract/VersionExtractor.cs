using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NLog;
using SeleniumZombie.Common.Extract.Models;

namespace SeleniumZombie.Common.Extract
{
    public class VersionExtractor
    {
        private readonly string _storageUrl;
        private readonly Regex _filePathRegex = new Regex(@"^(?<version>[\d+\.]+)\/.+$");
        private readonly List<Func<string, string, bool>> _filters = new List<Func<string, string, bool>>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public VersionExtractor(string storageUrl)
        {
            _storageUrl = storageUrl;
        }

        public void AddFilter(Func<string, string, bool> filter)
        {
            _filters.Add(filter);
        }

        public VersionModel[] GetAvailableVersions()
        {
            _logger.Info($"Getting versions from {_storageUrl}...");
            var webRequest = WebRequest.CreateHttp(_storageUrl);
            using (var response = webRequest.GetResponse())
            using (var responseStream = response.GetResponseStream())
            {
                var document = XDocument.Load(responseStream);
                var ns = document.Root.GetDefaultNamespace().NamespaceName;

                return document
                    .Descendants(XName.Get("Contents", ns))
                    .Select(contentNode => contentNode.Element(XName.Get("Key", ns)).Value)
                    .Select(filePath => _filePathRegex.Match(filePath))
                    .Where(filePathMatch => filePathMatch.Success)
                    .Select(filePathMatch => new Tuple<string, string>(filePathMatch.Groups["version"].Value, filePathMatch.Value))
                    .Where(filePathTuple => _filters.All(filter => filter.Invoke(filePathTuple.Item1, filePathTuple.Item2)))
                    .Select(filePathTuple => new VersionModel(
                        Version.Parse(filePathTuple.Item1), 
                        string.Join("/", _storageUrl, filePathTuple.Item2)))
                    .ToArray();
            }
        }
    }
}

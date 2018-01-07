using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SeleniumZombie.Common;

namespace SeleniumZombie.Service.Update
{
    internal class ModuleVersionsFileManager
    {
        private readonly string _fileName;

        public ModuleVersionsFileManager(string fileName)
        {
            _fileName = CurrentDirectory.CreateFullFilePath(fileName);
        }

        public ModuleVersionModel GetVersion(NewestVersionDownloader downloader)
        {
            var moduleVersion = DeserializeModuleVersions()
                .SingleOrDefault(v => v.ModuleName == downloader.GetType().FullName);

            Version version;
            return moduleVersion != null && Version.TryParse(moduleVersion.Version, out version) ? moduleVersion : null;
        }

        public void UpdateVersion(NewestVersionDownloader downloader, Version version)
        {
            var downloaderTypeName = downloader.GetType().FullName;

            var moduleVersions = DeserializeModuleVersions();
            var moduleVersion = moduleVersions
                .SingleOrDefault(v => v.ModuleName == downloaderTypeName);

            if (moduleVersion != null)
                moduleVersions.Remove(moduleVersion);

            moduleVersion = new ModuleVersionModel
            {
                ModuleName = downloaderTypeName,
                Version = version.ToString()
            };
            moduleVersions.Add(moduleVersion);

            SerializeModuleVersions(moduleVersions);
        }

        public List<ModuleVersionModel> DeserializeModuleVersions()
        {
            var deserializer = new XmlSerializer(typeof(List<ModuleVersionModel>));
            if (!File.Exists(_fileName))
                return new List<ModuleVersionModel>();
            using (var fileReader = new FileStream(_fileName, FileMode.Open))
            {
                return (List<ModuleVersionModel>)deserializer.Deserialize(fileReader);
            }
        }

        private void SerializeModuleVersions(List<ModuleVersionModel> moduleVersionModels)
        {
            if(!File.Exists(_fileName))
                File.Create(_fileName).Dispose();

            var serializer = new XmlSerializer(typeof(List<ModuleVersionModel>));
            using (var fileWriter = new FileStream(_fileName, FileMode.Truncate, FileAccess.Write))
            {
                serializer.Serialize(fileWriter, moduleVersionModels);
            }
        }
    }
}

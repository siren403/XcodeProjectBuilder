using System.IO;
using UnityEngine;

namespace XcodeProjectBuilder
{
    internal class PBXBuildPhases : IBuildPhases
    {
        private readonly PBXProjectWrapper _wrapper;

        public PBXBuildPhases(PBXProjectWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public void AddCopyBundleResources(string assetsUnderPath)
        {
            AddCopyBundleResources(assetsUnderPath, assetsUnderPath);
        }

        public void AddCopyBundleResources(string assetsUnderPath, string toPath)
        {
            if (assetsUnderPath.StartsWith("Assets/"))
            {
                Debug.LogError(
                    $"[{nameof(AddCopyBundleResources)}] {assetsUnderPath} -> {assetsUnderPath.Replace("Assets/", "")}");
                return;
            }

            var assetsPath = Path.Combine("Assets", assetsUnderPath);
            var destPath = Path.Combine(_wrapper.BuildPath, toPath);


            if (!File.Exists(destPath))
            {
                var dir = Path.GetDirectoryName(destPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.Copy(assetsPath, destPath);
                _wrapper.AddFileToBuild(destPath, toPath);
            }
        }
    }
}
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace XcodeProjectBuilder
{
    public class PBXProjectWrapper : IXcodeProject
    {
        internal string BuildPath { get; private set; }
        internal string ProjectPath { get; private set; }
        internal PBXProject Project { get; private set; }

        internal string ProjectGuid { get; private set; }

        public IBuildSettings BuildSettings { get; private set; }
        public IBuildPhases BuildPhases { get; private set; }

        private readonly string _unityMainTargetGuid;

        public IPlist Info => _info;
        private readonly InfoPlist _info;

        public PBXProjectWrapper(string buildPath)
        {
            Debug.Log(buildPath);
            BuildPath = buildPath;
            ProjectPath = PBXProject.GetPBXProjectPath(buildPath);
            Project = new PBXProject();
            Project.ReadFromFile(ProjectPath);

            ProjectGuid = Project.ProjectGuid();
            _unityMainTargetGuid = Project.GetUnityMainTargetGuid();

            BuildSettings = new PBXBuildSettings(this);
            BuildPhases = new PBXBuildPhases(this);
            _info = new InfoPlist(this);
        }

        public void AddCapability(PBXCapabilityType capability,
            string entitlementsFilePath = null,
            bool addOptionalFramework = false)
        {
            Project.AddCapability(_unityMainTargetGuid, capability, entitlementsFilePath, addOptionalFramework);
        }

        public void AddBuildProperty(string name, string value)
        {
            Project.AddBuildProperty(ProjectGuid, name, value);
        }

        public void SetBuildProperty(string name, string value)
        {
            Project.SetBuildProperty(ProjectGuid, name, value);
        }

        public void AddFileToBuild(string fromPath, string toPath)
        {
            var fileGuid = Project.AddFile(fromPath, toPath);
            Project.AddFileToBuild(_unityMainTargetGuid, fileGuid);
        }

        public void Dispose()
        {
            Project.WriteToFile(ProjectPath);
            _info.Dispose();
        }
    }
}
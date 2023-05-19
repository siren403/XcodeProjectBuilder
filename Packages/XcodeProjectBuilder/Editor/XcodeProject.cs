using System;
using System.IO;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace XcodeProjectBuilder
{
    public partial class XcodeProject
    {
        public string BuildPath { get; }
        
        /// <summary>
        /// PBXProjectPath
        /// </summary>
        public string ProjectPath { get; }
        
        /// <summary>
        /// PBXProject
        /// </summary>
        internal PBXProject Project { get; }

        /// <summary>
        /// PBXProjectGuid
        /// </summary>
        internal string ProjectGuid { get; }

        public BuildSettings BuildSettings { get; }
        public BuildPhases BuildPhases { get; }

        public string UnityFrameworkTargetGuid { get; }
        public string UnityMainTargetGuid { get; }

        private XcodeProject(string buildPath)
        {
            Debug.Log(buildPath);
            BuildPath = buildPath;
            ProjectPath = PBXProject.GetPBXProjectPath(buildPath);
            Project = new PBXProject();
            Project.ReadFromFile(ProjectPath);

            ProjectGuid = Project.ProjectGuid();
            UnityMainTargetGuid = Project.GetUnityMainTargetGuid();
            UnityFrameworkTargetGuid = Project.GetUnityFrameworkTargetGuid();

            BuildSettings = new BuildSettings(this);
            BuildPhases = new BuildPhases(this);
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
            Project.AddFileToBuild(UnityMainTargetGuid, fileGuid);
        }

        public void WriteToFile()
        {
            Project.WriteToFile(ProjectPath);
        }
    }

    public static class CapabilityExtensions
    {
        public static void AddPushNotifications(this XcodeProject xcodeProject,
            string entitlementsFileName = "Unity-iPhone")
        {
            var entitlements = $"{entitlementsFileName}.entitlements";
            var manager = new ProjectCapabilityManager(xcodeProject.ProjectPath, entitlements,
                targetGuid: xcodeProject.UnityMainTargetGuid);
            manager.AddPushNotifications(false);
            manager.AddInAppPurchase();
            manager.WriteToFile();

            xcodeProject.Project.AddFile(entitlements, entitlements);
            xcodeProject.Project.AddBuildProperty(xcodeProject.UnityMainTargetGuid, "CODE_SIGN_ENTITLEMENTS",
                entitlements);
        }
    }
}
using System.IO;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public class Capability
    {
        private readonly XcodeProject _project;
        private readonly string _entitlementFileDirectory;
        private readonly string _entitlementFileName;
        private string EntitlementFilePath => $"{_entitlementFileDirectory}/{_entitlementFileName}";

        internal Capability(XcodeProject project, string entitlementsFileName = "Unity-iPhone")
        {
            _project = project;
            _entitlementFileName = $"{entitlementsFileName}.entitlements";
            _entitlementFileDirectory = $"Unity-iPhone";
            ReadEntitlement();
        }


        private Plist ReadEntitlement()
        {
            var path = Path.Combine(_project.BuildPath, EntitlementFilePath);
            var entitlements = new Plist(path);
            entitlements.ReadOrCreate();

            _project.Project.AddFile(EntitlementFilePath, _entitlementFileName);
            // _project.AddBuildProperty("CODE_SIGN_ENTITLEMENTS",
            // EntitlementFilePath);
            _project.WriteToFile();
            return entitlements;
        }

        private ProjectCapabilityManager ReadManager()
        {
            return new ProjectCapabilityManager(_project.ProjectPath, EntitlementFilePath,
                targetGuid: _project.UnityMainTargetGuid);
        }

        public void WritePushNotifications(bool development = false)
        {
            var manager = ReadManager();
            manager.AddPushNotifications(development);
            manager.WriteToFile();
        }

        /// <summary>
        /// FCM Manual Configs
        /// https://firebase.google.com/docs/cloud-messaging/unity/client?hl=ko#enable_push_notifications_on_apple_platforms
        /// </summary>
        public void WriteFirebasePushNotifications()
        {
            _project.Project.AddFrameworkToProject(_project.UnityMainTargetGuid, "UserNotifications.framework", true);
            _project.WriteToFile();
            WritePushNotifications();
            WriteBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        }

        public void WriteBackgroundModes(BackgroundModesOptions options)
        {
            var manager = ReadManager();
            manager.AddBackgroundModes(options);
            manager.WriteToFile();
        }

        /// <summary>
        /// 2021.3.23f / Xcode14.2
        /// </summary>
        public void WriteGameCenter()
        {
            // manual entitlement
            var entitlements = ReadEntitlement();
            entitlements.EnableGameCenter();
            entitlements.WriteToFile();

            // add BuildSettings/Signing/CodeSignEntitlements
            _project.Project.AddCapability(_project.UnityMainTargetGuid, PBXCapabilityType.GameCenter,
                EntitlementFilePath);
            _project.WriteToFile();
        }


        public void WriteInAppPurchase()
        {
            var unityTargetGuid = _project.UnityFrameworkTargetGuid;
            var mainTargetGuid = _project.UnityMainTargetGuid;
            _project.Project.AddFrameworkToProject(unityTargetGuid, "StoreKit.framework", false);
            _project.Project.AddFrameworkToProject(mainTargetGuid, "StoreKit.framework", false);
            _project.WriteToFile();

            var manager = ReadManager();
            manager.AddInAppPurchase();
            manager.WriteToFile();
        }
    }
}
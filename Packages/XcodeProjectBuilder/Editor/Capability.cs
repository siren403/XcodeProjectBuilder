using System.IO;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public class Capability
    {
        private readonly string _entitlementFileDirectory;
        private readonly string _entitlementFileName;
        private readonly string _buildPath;
        private string EntitlementFilePath => $"{_entitlementFileDirectory}/{_entitlementFileName}";

        internal Capability(string buildPath, string entitlementsFileName = "Unity-iPhone")
        {
            _buildPath = buildPath;
            _entitlementFileName = $"{entitlementsFileName}.entitlements";
            _entitlementFileDirectory = $"Unity-iPhone";
            ReadEntitlement(ReadProject());
        }

        private XcodeProject ReadProject() => XcodeProject.ReadProject(_buildPath);

        private Plist ReadEntitlement(XcodeProject project)
        {
            var path = Path.Combine(_buildPath, EntitlementFilePath);
            var entitlements = new Plist(path);
            entitlements.ReadOrCreate();

            project.Project.AddFile(EntitlementFilePath, _entitlementFileName);
            // _project.AddBuildProperty("CODE_SIGN_ENTITLEMENTS",
            // EntitlementFilePath);
            project.WriteToFile();
            return entitlements;
        }

        private ProjectCapabilityManager ReadManager(XcodeProject project)
        {
            return new ProjectCapabilityManager(project.ProjectPath, EntitlementFilePath,
                targetGuid: project.UnityMainTargetGuid);
        }

        public void WritePushNotifications(bool development = false)
        {
            var manager = ReadManager(ReadProject());
            manager.AddPushNotifications(development);
            manager.WriteToFile();
        }

        private const string AuthServiceFramework = "AuthenticationServices.framework";

        public void WriteSignIn()
        {
            var project = ReadProject();
            var unityTargetGuid = project.UnityFrameworkTargetGuid;
            project.Project.AddFrameworkToProject(unityTargetGuid, AuthServiceFramework, false);

            var mainTargetGuid = project.UnityMainTargetGuid;
            project.Project.AddFrameworkToProject(mainTargetGuid, AuthServiceFramework, false);

            project.WriteToFile();

            var manager = ReadManager(project);
            manager.AddSignInWithApple();
            manager.WriteToFile();
        }

        /// <summary>
        /// FCM Manual Configs
        /// https://firebase.google.com/docs/cloud-messaging/unity/client?hl=ko#enable_push_notifications_on_apple_platforms
        /// </summary>
        public void WriteFirebasePushNotifications()
        {
            var project = ReadProject();
            project.Project.AddFrameworkToProject(project.UnityMainTargetGuid, "UserNotifications.framework", true);
            project.WriteToFile();
            WritePushNotifications();
            WriteBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        }

        public void WriteBackgroundModes(BackgroundModesOptions options)
        {
            var manager = ReadManager(ReadProject());
            manager.AddBackgroundModes(options);
            manager.WriteToFile();
        }

        /// <summary>
        /// 2021.3.23f / Xcode14.2
        /// </summary>
        public void WriteGameCenter()
        {
            var project = ReadProject();

            // manual entitlement
            var entitlements = ReadEntitlement(project);
            entitlements.EnableGameCenter();
            entitlements.WriteToFile();

            // add BuildSettings/Signing/CodeSignEntitlements
            project.Project.AddCapability(project.UnityMainTargetGuid, PBXCapabilityType.GameCenter,
                EntitlementFilePath);
            project.WriteToFile();
        }


        private const string StoreKitFramework = "StoreKit.framework";

        public void WriteInAppPurchase()
        {
            var project = ReadProject();

            var unityTargetGuid = project.UnityFrameworkTargetGuid;
            var mainTargetGuid = project.UnityMainTargetGuid;
            project.Project.AddFrameworkToProject(unityTargetGuid, StoreKitFramework, false);
            project.Project.AddFrameworkToProject(mainTargetGuid, StoreKitFramework, false);
            project.WriteToFile();

            var manager = ReadManager(project);
            manager.AddInAppPurchase();
            manager.WriteToFile();
        }
    }
}
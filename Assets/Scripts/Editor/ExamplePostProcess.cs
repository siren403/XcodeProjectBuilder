using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XcodeProjectBuilder;

public static class ExamplePostProcess
{
    [PostProcessBuild]
    public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS) return;

        var xcode = XcodeProject.ReadProject(buildPath);
        xcode.BuildSettings.EnableBitcode = false;
        // xcode.BuildPhases.AddCopyBundleResources("dummy/project_structure.json");
        // xcode.BuildPhases.AddCopyBundleResources("dummy/project_structure.json", "copy/project_structure.json");
        xcode.WriteToFile();

        var info = XcodeProject.ReadInfoPlist(buildPath);
        info.DisableAppUsesNonExemptEncryption();
        info.SetBundleName(Application.productName);
        info.WriteToFile();

        var capability = XcodeProject.ReadCapability(buildPath);
        capability.WriteInAppPurchase();
        capability.WriteGameCenter();
        capability.WritePushNotifications();
    }
}
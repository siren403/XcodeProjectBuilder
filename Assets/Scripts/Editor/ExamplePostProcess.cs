using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XcodeProjectBuilder;

public static class ExamplePostProcess
{
    [PostProcessBuild]
    public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
    {
        using var xcode = XcodeProject.FromPostProcess(buildTarget, buildPath);

        xcode.BuildSettings.EnableBitcode = false;
        xcode.Info.DisableAppUsesNonExemptEncryption();
        // xcode.BuildPhases.AddCopyBundleResources("dummy/project_structure.json");
        // xcode.BuildPhases.AddCopyBundleResources("dummy/project_structure.json", "copy/project_structure.json");
    }
}
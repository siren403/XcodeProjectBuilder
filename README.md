# XcodeProjectBuilder

UPM from git

```
https://github.com/qkrsogusl3/XcodeProjectBuilder.git?path=Packages/XcodeProjectBuilder
```

## Project

```csharp
// iOSPostProcess.cs

using UnityEditor;
using UnityEditor.Callbacks;
using XcodeProjectBuilder;

public static class iOSPostProcess
{
    [PostProcessBuild]
    public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS) return;

        var xcode = XcodeProject.ReadProject(buildPath);
        xcode.BuildSettings.EnableBitcode = false;
        xcode.WriteToFile();

        var info = XcodeProject.ReadInfoPlist(buildPath);
        info.DisableAppUsesNonExemptEncryption();
        info.SetBundleName(Application.productName);
        info.WriteToFile();
    }
}
```

## Capability

> **Warning**
> XcodeProject와 Capability(ProjectCapabilityManager)는 내부에서 PBXProject를 개별적으로 읽고 쓰기 때문에
> 둘 다 읽어놓고 수정할 경우 쓰기 순서에 따라 결과가 다를 수 있음.

```csharp
[PostProcessBuild]
public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
{
    if (buildTarget != BuildTarget.iOS) return;
    
    var xcode = XcodeProject.ReadProject(buildPath);
    // project settings...
    xcode.WriteToFile();
    
    var capability = XcodeProject.ReadCapability(buildPath);
    capability.WriteInAppPurchase();
    capability.WriteGameCenter();
    capability.WritePushNotifications();
}
```
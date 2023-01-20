# XcodeProjectBuilder

UPM from git
```
https://github.com/qkrsogusl3/XcodeProjectBuilder.git?path=Packages/XcodeProjectBuilder
```

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
        using var xcode = XcodeProject.FromPostProcess(buildTarget, buildPath);
        xcode.BuildSettings.EnableBitcode = false;
    }
}
```

using System;
using UnityEditor;

namespace XcodeProjectBuilder
{
    public interface IXcodeProject : System.IDisposable
    {
        IBuildSettings BuildSettings { get; }
        IBuildPhases BuildPhases { get; }
        IPlist Info { get; }
    }

    public static class XcodeProject
    {
        public static IXcodeProject FromPostProcess(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return new NotSupportPlatform();
            }

            return new PBXProjectWrapper(buildPath);
        }
    }
}
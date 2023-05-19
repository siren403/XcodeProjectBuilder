using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public class InfoPlist : Plist
    {
        internal InfoPlist(string buildPath) : base(buildPath + "/Info.plist")
        {
        }
    }
}
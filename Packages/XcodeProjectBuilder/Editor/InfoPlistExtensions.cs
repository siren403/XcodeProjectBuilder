namespace XcodeProjectBuilder
{
    public static class InfoPlistExtensions
    {
        public static void DisableAppUsesNonExemptEncryption(this IPlist plist)
        {
            plist.Booleans["ITSAppUsesNonExemptEncryption"] = false;
        }
    }
}
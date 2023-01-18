namespace XcodeProjectBuilder
{
    public interface IBuildPhases
    {
        void AddCopyBundleResources(string assetsUnderPath);
        void AddCopyBundleResources(string assetsUnderPath, string toPath);
    }
}
namespace XcodeProjectBuilder
{
    public class BuildSettings
    {
        private const string OtherLinkerFlagsName = "OTHER_LDFLAGS";
        private readonly XcodeProject _xcodeProject;

        public string OtherLinkerFlags
        {
            set => _xcodeProject.SetBuildProperty(OtherLinkerFlagsName, value);
        }

        public void AddOtherLinkerFlags(string value) => _xcodeProject.AddBuildProperty(OtherLinkerFlagsName, value);

        public bool EnableBitcode
        {
            set => _xcodeProject.SetBuildProperty("ENABLE_BITCODE", value ? "YES" : "NO");
        }

        public BuildSettings(XcodeProject xcodeProject)
        {
            _xcodeProject = xcodeProject;
        }
    }
}
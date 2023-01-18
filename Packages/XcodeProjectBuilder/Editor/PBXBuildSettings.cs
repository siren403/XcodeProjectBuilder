namespace XcodeProjectBuilder
{
    internal class PBXBuildSettings : IBuildSettings
    {
        private const string OtherLinkerFlagsName = "OTHER_LDFLAGS";
        private readonly PBXProjectWrapper _wrapper;

        public string OtherLinkerFlags
        {
            set => _wrapper.SetBuildProperty(OtherLinkerFlagsName, value);
        }

        public void AddOtherLinkerFlags(string value) => _wrapper.AddBuildProperty(OtherLinkerFlagsName, value);

        public bool EnableBitcode
        {
            set => _wrapper.SetBuildProperty("ENABLE_BITCODE", value ? "YES" : "NO");
        }

        public PBXBuildSettings(PBXProjectWrapper wrapper)
        {
            _wrapper = wrapper;
        }
    }
}
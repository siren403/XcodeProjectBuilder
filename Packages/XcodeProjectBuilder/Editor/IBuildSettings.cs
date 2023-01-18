namespace XcodeProjectBuilder
{
    public interface IBuildSettings
    {
        string OtherLinkerFlags { set; }
        void AddOtherLinkerFlags(string value);

        bool EnableBitcode { set; }
    }
}
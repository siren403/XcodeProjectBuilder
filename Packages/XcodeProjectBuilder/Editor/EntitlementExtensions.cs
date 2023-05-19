namespace XcodeProjectBuilder
{
    public static class EntitlementExtensions
    {
        public static void EnableGameCenter(this Plist plist)
        {
            plist.Booleans["com.apple.developer.game-center"] = true;
        }
    }
}
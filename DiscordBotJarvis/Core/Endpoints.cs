namespace DiscordBotJarvis.Core
{
    public static class EndPoints
    {
        public const string CurrentDirectory = ".";
        public const string SeparatorDirectory = @"\";

        public static class Directory
        {
            public const string ResourcePacks = "ResourcePacks";
            public const string ResourcePacksCommands = @"ResourcePacks\{0}\Commands\{1}";
            public const string ResourcePacksResources = @"ResourcePacks\{0}\Resources";
            public const string ResourcePacksServices = @"ResourcePacks\{0}\Services";
        }

        public static class Path
        {
            public const string ResourcePacksConfigFile = @"ResourcePacks\{0}\Config.xml";
            public const string ResourcePacksCommands = @"ResourcePacks\{0}\Commands\{1}\{2}";
            public const string ResourcePacksResources = @"ResourcePacks\{0}\Resources\{1}";
            public const string ResourcePacksServices = @"ResourcePacks\{0}\Services\{1}";
        }
    }
}

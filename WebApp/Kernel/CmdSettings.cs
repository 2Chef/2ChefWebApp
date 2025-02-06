namespace WebApp.Kernel
{
    public static class CmdSettings
    {
        private static string[] CommandArgs { get; }

        static CmdSettings()
        {
            CommandArgs = Environment.GetCommandLineArgs();
        }

        public static bool IsLongPoolingConnection => CommandArgs.Contains("-lp");

        public static bool IsWebHookConnection => !IsLongPoolingConnection;
    }
}

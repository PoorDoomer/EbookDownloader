//File for environment variable like api baseUrl,headers(api_key),upload_directory 



namespace EnvironmentVariable
{
    public static class EnvironmentVariables
    {
        public static string apiKey ;

        public static string apiHost = "annas-archive-api.p.rapidapi.com";
        public static string baseUrl = "https://annas-archive-api.p.rapidapi.com";
        public static string downloadPath = Path.Combine(AppContext.BaseDirectory, "DownloadedBooks");
        public static void EnsureDownloadFolderExists()
        {
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
        }   
    }
}


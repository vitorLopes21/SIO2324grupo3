namespace ExtractDataFromCSV.Helpers
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Find the solution directory info by looking for a .sln file in the current directory or its parent directories.
        /// </summary>
        /// <param name="currentPath">Current directory path to start the search from.</param>
        /// <returns>The DirectoryInfo of the solution directory, or null if not found.</returns>
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            // Start from the current directory
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());

            // Move up the directory tree until a .sln file is found
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            // Return the solution directory, or null if not found
            return directory;
        }

        /// <summary>
        /// Get the values in the appsettings.Development.json file and update the appsettings.json placeholders with those values.
        /// </summary>
        public static void UpdateAppSettingsWithDevelopmentValuesService(string key)
        {
            // Find the solution folder
            string slnFolder = TryGetSolutionDirectoryInfo().FullName;

            // Read the appsettings.json file
            var appSettings = JObject.Parse(File.ReadAllText(slnFolder + "\\appsettings.json"));

            // Read the appsettings.Development.json file
            var developmentSettings = JObject.Parse(File.ReadAllText(slnFolder + "\\appsettings.Development.json"));

            // Update the ConnectionStrings in appsettings.json with the values from appsettings.Development.json
            var keyValue = appSettings[key] as JObject;
            var developmentKeyValue = developmentSettings[key] as JObject;

            foreach (JProperty property in developmentKeyValue.Properties())
            {
                keyValue[property.Name] = property.Value;
            }

            // Write the updated appsettings.json back to disk
            File.WriteAllText("appsettings.json", appSettings.ToString());
        }

        /// <summary>
        /// Get the values in the appsettings.Development.json file and update the appsettings.json placeholders with those values.
        /// </summary>
        public static void UpdateAppSettingsWithDevelopmentValuesService(string[] key)
        {
            // Find the solution folder
            string slnFolder = TryGetSolutionDirectoryInfo().FullName;

            // Read the appsettings.json file
            var appSettings = JObject.Parse(File.ReadAllText(slnFolder + "\\appsettings.json"));

            // Read the appsettings.Development.json file
            var developmentSettings = JObject.Parse(File.ReadAllText(slnFolder + "\\appsettings.Development.json"));

            // Update the ConnectionStrings in appsettings.json with the values from appsettings.Development.json
            foreach (var item in key)
            {
                var keyValue = appSettings[item] as JObject;
                var developmentKeyValue = developmentSettings[item] as JObject;

                foreach (JProperty property in developmentKeyValue.Properties())
                {
                    keyValue[property.Name] = property.Value;
                }
            }

            // Write the updated appsettings.json back to disk
            File.WriteAllText("appsettings.json", appSettings.ToString());
        }
    }
}
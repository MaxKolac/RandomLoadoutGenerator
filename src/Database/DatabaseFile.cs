using System.Reflection;

namespace RandomLoadoutGenerator.Database
{
    /// <summary>
    /// Static class for operations on the database file bundled inside the NuGet package.
    /// </summary>
    internal static class DatabaseFile
    {
        /// <summary>
        /// Name of the database file with extension.
        /// </summary>
        public const string Filename = "tf_weapons.sqlite3";
        /// <summary>
        /// Directory in which the database file is located.
        /// </summary>
        public static string Directory { get => Environment.CurrentDirectory; }
        /// <summary>
        /// Absolute path to the database file.
        /// </summary>
        public static string FullPath { get => Path.Combine(Directory, Filename); }

        /// <summary>
        /// Unpacks the database file from this NuGet package's assembly and creates it in the <see cref="Environment.CurrentDirectory"/>, which usually is the same location where your app's executable is.
        /// </summary>
        public static void Unpack()
        {
            var assembly = typeof(DatabaseFile).GetTypeInfo().Assembly; 
            using Stream resource = assembly.GetManifestResourceStream("RandomLoadoutGenerator.Database." + Filename)!;

            using var reader = new StreamReader(resource);
            using var writer = File.OpenWrite(FullPath);
            resource.CopyTo(writer);            
        }

        public static bool Exists() => File.Exists(FullPath);
    }
}

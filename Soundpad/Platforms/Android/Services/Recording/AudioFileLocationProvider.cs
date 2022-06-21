using Android.Content;
using Soundpad.Data.Constants;

namespace Soundpad.Platforms.Android.Services.Recording
{
    public static class AudioFileLocationProvider
    {
        public static string LatestLocation { get; private set; }
        public static string LatestId { get; private set; }


        public static string GenerateNewLocation(Context context)
        {
            var outputDirectory = Path.Combine(FileSystem.AppDataDirectory, "recordings");

            if (Directory.Exists(outputDirectory) == false)
                Directory.CreateDirectory(outputDirectory);

            LatestId = Guid.NewGuid().ToString();
            LatestLocation = Path.Combine(outputDirectory, $"{RConst.RECORDING_DEFAULT_NAME_PREFIX}{LatestId}.pcm");

            return LatestLocation;
        }
    }
}

namespace Soundpad.Data.Constants
{
    public class RConst
    {
        public static string LOG_TAG = "AudioCaptureService";
        public static int SERVICE_ID = 123;
        public static string NOTIFICATION_CHANNEL_ID = "AudioCapture channel";

        public static string OUTPUT_DIRECTORY = Path.Combine(FileSystem.AppDataDirectory, "recordings");
        public static string RECORDING_DEFAULT_NAME_PREFIX = "Soundpad_recording_";
        public static int NUM_SAMPLES_PER_READ = 1024;
        public static int BYTES_PER_SAMPLE = 2;
        public static int BUFFER_SIZE_IN_BYTES = NUM_SAMPLES_PER_READ * BYTES_PER_SAMPLE;

        public static int DECODE_SAMPLE_RATE = 22000;
        public static int DECODE_BIT_RATE = 128000;
        public static int DECODE_CHANNELS_COUNT = 2;

        public const string ACTION_START = "AudioCaptureService:Start";
        public const string ACTION_STOP = "AudioCaptureService:Stop";
        public static string EXTRA_RESULT_DATA = "AudioCaptureService:Extra:ResultData";

        public static int RECORD_AUDIO_PERMISSION_REQUEST_CODE = 42;
        public static int CAPTURE_MEDIA_PROJECTION_REQUEST_CODE = 13;
        public static int SELECT_OUTPUT_DIRECTORY_REQUEST_CODE = 14;

        public static string PREFERENCES_APP_NAME = "Soundpad";
        public static string PREFERENCES_KEY_OUTPUT_DIRECTORY = "Soundpad";

        public static string FORMAT_DATE_FULL = "hh_mm_ss";
        public static string FORMAT_RECORDING_ENCODED = "%s/%s.pcm";
        public static string FORMAT_RECORDING_DECODED = "%s/%s.mp3";
        public static string FORMAT_OUTPUT_DIRECTORY = "%s/.../%s";

        public static string TITLE_DEFAULT_DIRECTORY = "Music/";
    }
}

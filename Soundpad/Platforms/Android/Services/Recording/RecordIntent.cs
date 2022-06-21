using Android.App;
using Android.Content;
using Android.Media;
using Android.Media.Projection;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Java.Lang;
using Soundpad.Data.Constants;
using Soundpad.Models;
using System.Diagnostics;
using Encoding = Android.Media.Encoding;
using Thread = Java.Lang.Thread;

namespace Soundpad.Platforms.Android.Services.Recording;

/// <summary>
/// Reversed engineered from https://github.com/lincollincol/QRecorder
/// </summary>
[Service(Exported = true, Name = "com.companyname.soundpad.RecordIntent")]
public class RecordIntent : Service
{
    private MediaProjectionManager _mediaProjectionManager;
    private MediaProjection _mediaProjection;

    private Thread _audioCaptureThread;
    private AudioRecord _audioRecord;
    private Stopwatch _stopwatch;



    public override void OnCreate()
    {
        base.OnCreate();

        CreateNotificationChannel();
        StartForeground(
                RConst.SERVICE_ID,
                new NotificationCompat.Builder(this, RConst.NOTIFICATION_CHANNEL_ID).Build()
        );

        // use applicationContext to avoid memory leak on Android 10.
        // see: https://partnerissuetracker.corp.google.com/issues/139732252
        _mediaProjectionManager = ApplicationContext.GetSystemService(MediaProjectionService) as MediaProjectionManager;
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        if (intent == null)
            return StartCommandResult.NotSticky;

        switch (intent.Action)
        {
            case RConst.ACTION_START:
                {
                    _mediaProjection = _mediaProjectionManager.GetMediaProjection(
                            (int)Result.Ok,
                            (Intent)intent.GetParcelableExtra(RConst.EXTRA_RESULT_DATA));

                    StartAudioCapture();

                    _stopwatch = new Stopwatch();
                    _stopwatch.Start();

                    AudioModel recordingmodel = MainActivity.Instance.RecordingState.Value;
                    recordingmodel.Recording = true;
                    MainActivity.Instance.RecordingState.OnNext(recordingmodel);

                    return StartCommandResult.Sticky;
                }
            case RConst.ACTION_STOP:
                {
                    try
                    {
                        StopAudioCapture();

                        _stopwatch.Stop();

                        AudioModel recordingmodel = MainActivity.Instance.RecordingState.Value;
                        recordingmodel.Recording = false;
                        recordingmodel.RecordingTimeInMilliSeconds = _stopwatch.ElapsedMilliseconds;
                        MainActivity.Instance.RecordingState.OnNext(recordingmodel);
                    }
                    catch (InterruptedException e)
                    {
                        e.PrintStackTrace();
                    }
                    return StartCommandResult.NotSticky;
                }
            default:
                {
                    throw new IllegalArgumentException($"Unexpected action received: {intent.Action}");
                }
        }
    }

    private void StartAudioCapture()
    {
        AudioPlaybackCaptureConfiguration config = new AudioPlaybackCaptureConfiguration.Builder(_mediaProjection)
                .AddMatchingUsage(AudioUsageKind.Media)
                .AddMatchingUsage(AudioUsageKind.VoiceCommunication)
                .AddMatchingUsage(AudioUsageKind.VoiceCommunicationSignalling)
                .AddMatchingUsage(AudioUsageKind.Game)
                .Build();


        //Using hardcoded values for the audio format, Mono PCM samples with a sample rate of 44100Hz
        //These can be changed according to your application's needs

        AudioFormat audioFormat = new AudioFormat.Builder()
                .SetEncoding(Encoding.Pcm16bit)
                //            .setSampleRate(8000)
                .SetSampleRate(44100)
                .SetChannelMask(ChannelOut.Mono)
                .Build();

        _audioRecord = new AudioRecord.Builder()
                .SetAudioFormat(audioFormat)
                .SetBufferSizeInBytes(RConst.BUFFER_SIZE_IN_BYTES)
                .SetAudioPlaybackCaptureConfig(config)
                .Build();

        _audioRecord.StartRecording();

        _audioCaptureThread = new Thread(() =>
        {
            string outputFile = AudioFileLocationProvider.GenerateNewLocation(this);
            try
            {
                WriteAudioToFile(outputFile);
            }
            catch (IOException e)
            {
                Log.Error(RConst.LOG_TAG, e.StackTrace);
            }
        });
        _audioCaptureThread.Start();

    }

    private void StopAudioCapture()
    {
        //requireNotNull(mediaProjection) { "Tried to stop audio capture, but there was no ongoing capture in place!" }
        if (_mediaProjection == null)
            return;


        _audioCaptureThread.Interrupt();
        _audioCaptureThread.Join();

        _audioRecord.Stop();
        _audioRecord.Release();
        _audioRecord = null;

        _mediaProjection.Stop();
        StopSelf();
    }

    private void CreateNotificationChannel()
    {
        NotificationChannel serviceChannel = new NotificationChannel(
                RConst.NOTIFICATION_CHANNEL_ID,
                "Audio Capture Service Channel",
                NotificationImportance.Default
        );

        NotificationManager manager = GetSystemService(NotificationService) as NotificationManager;

        manager.CreateNotificationChannel(serviceChannel);
    }

    private void WriteAudioToFile(string outputFilePath)
    {
        short[] capturedAudioSamples = new short[RConst.NUM_SAMPLES_PER_READ];

        using (var writer = File.Create(outputFilePath))
        {
            while (!_audioCaptureThread.IsInterrupted)
            {
                _audioRecord.Read(capturedAudioSamples, 0, RConst.NUM_SAMPLES_PER_READ);

                // This loop should be as fast as possible to avoid artifacts in the captured audio
                // You can uncomment the following line to see the capture samples but
                // that will incur a performance hit due to logging I/O.
                //Log.Info(RConst.LOG_TAG, $"Audio samples captured: {capturedAudioSamples.ToList()}");

                writer.Write(
                        ShortArrayToByteArray(capturedAudioSamples),
                        0,
                        RConst.BUFFER_SIZE_IN_BYTES
                );
            }
        }
    }


    private byte[] ShortArrayToByteArray(short[] array)
    {
        // Samples get translated into bytes following little-endianness:
        // least significant byte first and the most significant byte last
        byte[] bytes = new byte[array.Length * 2];
        for (int i = 0; i < array.Length; i++)
        {
            bytes[i * 2] = (byte)(array[i] & 0x00FF);
            bytes[i * 2 + 1] = (byte)(array[i] >> 8);
            array[i] = 0;
        }
        return bytes;
    }


    public override IBinder OnBind(Intent intent)
    {
        return null;
    }
}

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Media.Projection;
using Android.OS;
using Soundpad.Data.Constants;
using Soundpad.Models;
using Soundpad.Platforms.Android.Services.Recording;
using System.Reactive.Subjects;

namespace Soundpad;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, SupportsPictureInPicture = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
    ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance { get; private set; }
    public BehaviorSubject<bool> PictureInPictureMode { get; private set; } = new BehaviorSubject<bool>(false);
    public BehaviorSubject<AudioModel> PlaybackState { get; set; } = new BehaviorSubject<AudioModel>(new AudioModel());
    public BehaviorSubject<AudioModel> RecordingState { get; set; } = new BehaviorSubject<AudioModel>(new AudioModel());


    public MediaProjectionManager Mpm { get; private set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        Instance = this;
        base.OnCreate(savedInstanceState);

        Platform.Init(this, savedInstanceState);

        Mpm = Application.GetSystemService(MediaProjectionService) as MediaProjectionManager;
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    public override void OnPictureInPictureModeChanged(bool isInPictureInPictureMode, Configuration newConfig)
    {
        PictureInPictureMode.OnNext(isInPictureInPictureMode);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);

        if (requestCode != RConst.CAPTURE_MEDIA_PROJECTION_REQUEST_CODE)
            return;

        var recordingModel = new AudioModel(PlaybackState.Value);
        if (resultCode != Result.Ok)
        {
            recordingModel.IsRecordingAllowed = false;
            RecordingState.OnNext(recordingModel);
            return;
        }

        recordingModel.IsRecordingAllowed = true;
        RecordingState.OnNext(recordingModel);

        Intent audioCaptureIntent = new Intent(this, typeof(RecordIntent));
        audioCaptureIntent.SetAction(RConst.ACTION_START);
        audioCaptureIntent.PutExtra(RConst.EXTRA_RESULT_DATA, data);
        StartForegroundService(audioCaptureIntent);
    }


}

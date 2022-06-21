using Android.Content;
using Soundpad.Data.Constants;
using Soundpad.Models;
using Soundpad.Platforms.Android.Services.Recording;
using Soundpad.Services.Interfaces;
using System.Reactive.Subjects;

namespace Soundpad.Platforms.Services
{
    public class RecordService : IRecordService
    {

        public BehaviorSubject<AudioModel> RecordingState { get; set; } = new BehaviorSubject<AudioModel>(new AudioModel());

        private readonly IStorageService _storageService;
        private MainActivity _mainActivity = MainActivity.Instance;
        private string _categoryId;

        public RecordService(IStorageService storageService)
        {
            _storageService = storageService;

            MainActivity.Instance.RecordingState.Subscribe(async audioModel =>
            {
                RecordingState.OnNext(audioModel);

                if (audioModel.Recording == false && string.IsNullOrEmpty(_categoryId) == false)
                    await decodeCapture(audioModel.RecordingTimeInMilliSeconds);
            });
        }


        public ResultModel<int> StartRecordingSpeaker()
        {
            _mainActivity.StartActivityForResult(
                _mainActivity.Mpm.CreateScreenCaptureIntent(),
                RConst.CAPTURE_MEDIA_PROJECTION_REQUEST_CODE);

            return new ResultModel<int>(true);
        }

        public Task StopRecordingSpeaker(string categoryId)
        {
            Intent serviceIntent = new Intent(MainActivity.Instance.Application, typeof(RecordIntent));
            serviceIntent.SetAction(RConst.ACTION_STOP);
            MainActivity.Instance.StartService(serviceIntent);
            _categoryId = categoryId;

            return Task.CompletedTask;
        }

        private async Task decodeCapture(long recordingTimeInMilliSeconds)
        {
            var storageLocation = AudioFileLocationProvider.LatestLocation;

            TimeSpan duration = TimeSpan.FromMilliseconds(recordingTimeInMilliSeconds);
            duration = new TimeSpan(duration.Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);

            var sound = new SoundModel()
            {
                Id = AudioFileLocationProvider.LatestId,
                Name = AudioFileLocationProvider.LatestId,
                CategoryId = _categoryId,
                storageLocation = storageLocation,
                RecordedAt = DateTime.Now,
                Duration = duration
            };

            await _storageService.CreateSound(sound);

            _categoryId = "";
        }

    }
}

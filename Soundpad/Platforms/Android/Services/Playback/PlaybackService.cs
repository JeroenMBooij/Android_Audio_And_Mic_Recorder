using Android.Media;
using Soundpad.Models;
using Soundpad.Platforms.Android.Services.Playback;
using Soundpad.Services.Interfaces;
using System.Reactive.Subjects;

namespace Soundpad.Platforms.Services
{
    public class PlaybackService : IPlaybackService
    {
        public BehaviorSubject<AudioModel> PlaybackState { get; set; } = new BehaviorSubject<AudioModel>(new AudioModel());

        private readonly IStorageService _storageService;
        private PCMPlayer _pcmPlayer;
        private AudioManager _audioManager;

        public PlaybackService(IStorageService storageService, IPreferenceService preferenceService)
        {
            _storageService = storageService;
            MainActivity.Instance.PlaybackState.Subscribe(audioModel =>
            {
                PlaybackState.OnNext(audioModel);
            });

            preferenceService.Update.Subscribe(ss =>
            {
                if (ss.PlaybackSpeakers && ss.PlaybackMicrophone)
                    throw new NotImplementedException();
                else if (ss.PlaybackSpeakers)
                {
                    _audioManager.Mode = Mode.InCommunication;
                    _audioManager.SpeakerphoneOn = false;
                }
                else if (ss.PlaybackMicrophone)
                {
                    _audioManager.Mode = Mode.InCommunication;
                    _audioManager.SpeakerphoneOn = false;
                }

            });
        }

        public async Task<ResultModel<string>> Start(string soundId)
        {
            ResultModel<SoundModel> result = await _storageService.RetrieveSound(soundId);
            if (result.Success == false)
                return new ResultModel<string>(false) { Message = result.Message };

            SoundModel sound = result.Content;

            _pcmPlayer = new PCMPlayer(sound.storageLocation)
            {
                StopCallback = new Action(() =>
                {
                    AudioModel audiomodel = PlaybackState.Value;
                    audiomodel.SoundId = "";
                    PlaybackState.OnNext(audiomodel);
                })
            };

            _pcmPlayer.Play();

            AudioModel audiomodel = PlaybackState.Value;
            audiomodel.SoundId = soundId;
            PlaybackState.OnNext(audiomodel);

            return new ResultModel<string>(true);
        }

        public void Stop()
        {
            _pcmPlayer.IsPlaying = false;
        }
    }

}

using Soundpad.Models;
using Soundpad.Services.Interfaces;
using System.Reactive.Subjects;

namespace Soundpad.Platforms.Services
{
    public class PlaybackService : IPlaybackService
    {
        private readonly IStorageService _storageService;

        public PlaybackService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public BehaviorSubject<AudioModel> PlaybackState { get; set; }


        public Task<ResultModel<string>> Start(string soundId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}

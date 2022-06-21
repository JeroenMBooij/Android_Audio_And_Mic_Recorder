using Soundpad.Models;
using Soundpad.Services;

namespace Soundpad.Platforms.Services
{
    public class PlaybackService : IPlaybackService
    {
        private readonly IStorageService _storageService;

        public PlaybackService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public Task<ResultModel<string>> StartMicrophone(string soundId)
        {
            return null;
        }

        public Task<ResultModel<string>> StartSpeakers(string soundId)
        {
            throw new NotImplementedException();
        }

    }
}

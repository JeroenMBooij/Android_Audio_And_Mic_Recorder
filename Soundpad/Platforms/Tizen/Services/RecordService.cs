using NAudio.Lame;
using NAudio.Wave;
using Soundpad.Models;
using Soundpad.Services;


namespace Soundpad.Platforms.Services
{
    internal class RecordService : IRecordService
    {
        public BehaviorSubject<AudioModel> AudioState { get; set; }

        private readonly IStorageService _storageService;

        public RecordService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public ResultModel<int> StartRecordingSpeaker()
        {
            throw new NotImplementedException();
        }

        public Task StopRecordingSpeaker(string categoryId)
        {
            throw new NotImplementedException();
        }

       

    }
}

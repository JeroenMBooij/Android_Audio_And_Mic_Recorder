using NAudio.Lame;
using NAudio.Wave;
using Soundpad.Models;
using Soundpad.Services.Interfaces;
using System.Reactive.Subjects;

namespace Soundpad.Platforms.Services
{
    internal class RecordService : IRecordService
    {
        private readonly IStorageService _storageService;

        public RecordService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public BehaviorSubject<AudioModel> RecordingState { get; set; }

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

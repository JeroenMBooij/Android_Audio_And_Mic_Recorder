using Soundpad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Services.Interfaces
{
    public interface IPlaybackService
    {
        BehaviorSubject<AudioModel> PlaybackState { get; set; }
        public Task<ResultModel<string>> Start(string SoundId);
        public void Stop();

    }
}

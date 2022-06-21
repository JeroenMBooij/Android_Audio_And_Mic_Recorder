using Soundpad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Services.Interfaces
{
    public interface IRecordService
    {
        BehaviorSubject<AudioModel> RecordingState { get; set; }
        ResultModel<int> StartRecordingSpeaker();
        Task StopRecordingSpeaker(string categoryId);
    }
}

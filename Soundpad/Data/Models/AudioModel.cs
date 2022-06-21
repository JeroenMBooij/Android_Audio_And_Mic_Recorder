using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Models
{
    public class AudioModel
    {
        public AudioModel(AudioModel model)
        {
            SoundId = model.SoundId;
            IsRecordingAllowed = model.IsRecordingAllowed;
            Recording = model.Recording;
            RecordingTimeInMilliSeconds = model.RecordingTimeInMilliSeconds;
        }

        public AudioModel()
        {

        }
        public string SoundId { get; set; }
        public bool IsRecordingAllowed { get; set; } = false;
        public bool Recording { get; set; } = false;
        public long RecordingTimeInMilliSeconds { get; set; }

    }
}

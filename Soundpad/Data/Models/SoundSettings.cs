using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Data.Models
{
    public class SoundSettings
    {
        public bool RecordSpeakers { get; set; }
        public bool RecordMicrophone { get; set; }
        public bool PlaybackSpeakers { get; set; }
        public bool PlaybackMicrophone { get; set; }
    }
}

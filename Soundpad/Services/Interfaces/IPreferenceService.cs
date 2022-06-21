using Soundpad.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Services.Interfaces
{
    public interface IPreferenceService
    {
        BehaviorSubject<SoundSettings> Update { get; set; }
        void SaveSoundSettings(SoundSettings soundsettings);
        SoundSettings RetrieveSoundSettings();
    }
}

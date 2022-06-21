using Soundpad.Data.Models;
using Soundpad.Services.Interfaces;
using System.Reactive.Subjects;

namespace Soundpad.Services
{
    public class PreferenceService : IPreferenceService
    {
        public BehaviorSubject<SoundSettings> Update { get; set; }

        public PreferenceService()
        {
            var ss = RetrieveSoundSettings();
            Update = new BehaviorSubject<SoundSettings>(ss);
        }
        public void SaveSoundSettings(SoundSettings soundSettings)
        {
            ValidateSoundSettings(soundSettings);

            Preferences.Set(nameof(SoundSettings.PlaybackSpeakers), soundSettings.PlaybackSpeakers);
            Preferences.Set(nameof(SoundSettings.PlaybackMicrophone), soundSettings.PlaybackMicrophone);

            Preferences.Set(nameof(SoundSettings.RecordSpeakers), soundSettings.RecordSpeakers);
            Preferences.Set(nameof(SoundSettings.RecordMicrophone), soundSettings.RecordMicrophone);

            Update.OnNext(soundSettings);
        }
        public SoundSettings RetrieveSoundSettings()
        {
            var soundsettings = new SoundSettings();

            soundsettings.PlaybackSpeakers = Preferences.Get(nameof(SoundSettings.PlaybackSpeakers), true);
            soundsettings.PlaybackMicrophone = Preferences.Get(nameof(SoundSettings.PlaybackMicrophone), false);

            soundsettings.RecordSpeakers = Preferences.Get(nameof(SoundSettings.RecordSpeakers), true);
            soundsettings.RecordMicrophone = Preferences.Get(nameof(SoundSettings.RecordMicrophone), false);

            return soundsettings;
        }

        private void ValidateSoundSettings(SoundSettings ss)
        {
            SoundSettings previousSS = RetrieveSoundSettings();

            if (ss.PlaybackSpeakers == false && ss.PlaybackMicrophone == false)
            {
                ss.PlaybackSpeakers = !previousSS.PlaybackSpeakers;
                ss.PlaybackMicrophone = !previousSS.PlaybackMicrophone;
            }

            if (ss.RecordSpeakers == false && ss.RecordMicrophone == false)
            {
                ss.RecordSpeakers = !previousSS.RecordSpeakers;
                ss.RecordMicrophone = !previousSS.RecordMicrophone;
            }
        }

    }
}

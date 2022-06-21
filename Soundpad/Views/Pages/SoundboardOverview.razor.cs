using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Soundpad.Data.Models;
using Soundpad.Models;
using Soundpad.Services.Interfaces;

namespace Soundpad.Views.Pages
{
    public partial class SoundboardOverview
    {
        [Parameter]
        public string CategoryId { get; set; }

        public CategoryModel Category { get; set; } = new CategoryModel();
        public AudioModel Audio { get; set; } = new AudioModel();

        public SoundSettings SS { get; set; } = new SoundSettings();
        private EditContext EditContext;
        public bool PanelOpenState { get; set; }
        public bool IsRecording { get; set; }


        [Inject] private IStorageService _storageService { get; set; }
        [Inject] private IPreferenceService _preferenceService { get; set; }
        [Inject] private IRecordService _recordService { get; set; }
        [Inject] private IPlaybackService _playbackService { get; set; }
        [Inject] private NavigationManager _navigation { get; set; }

        protected override void OnInitialized()
        {
            CategoryId = CategoryId ?? "";

            if (string.IsNullOrEmpty(CategoryId))
            {
                _navigation.NavigateTo("/");
                return;
            }

            _playbackService.PlaybackState.Subscribe(async audioModel =>
            {
                Audio = audioModel;
                await RetrieveCategoryData();
                await InvokeAsync(() => StateHasChanged());
            });

            SS = _preferenceService.RetrieveSoundSettings();
            EditContext = new EditContext(SS);
            EditContext.OnFieldChanged += UpdateSoundSettings;

        }

        public void StartPlayback(string soundId)
        {
            _playbackService.Start(soundId);
        }

        public void StopPlayback()
        {
            _playbackService.Stop();
        }

        public void StartRecording()
        {
            ResultModel<int> result = _recordService.StartRecordingSpeaker();

            //TODO display errors to user
            if (result.Success)
                IsRecording = true;
        }


        public async Task StopRecording()
        {
            await _recordService.StopRecordingSpeaker(CategoryId);

            IsRecording = false;

            await RetrieveCategoryData();
        }

        private void UpdateSoundSettings(object sender, FieldChangedEventArgs e)
        {
            _preferenceService.SaveSoundSettings(SS);
        }


        private async Task RetrieveCategoryData()
        {
            var result = await _storageService.RetrieveCategory(CategoryId);
            //TODO display errors to user
            if (result.Success)
                Category = result.Content;
        }
    }
}

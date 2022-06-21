using Microsoft.AspNetCore.Components;
using Soundpad.Models;
using Soundpad.Services.Interfaces;

namespace Soundpad.Views.Pages
{
    public partial class CategoriesOverview
    {
        [Inject] private IStorageService _storageService { get; set; }
        [Inject] private NavigationManager _navigation { get; set; }

        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();

        protected override async Task OnInitializedAsync()
        {
            //_storageService.DeleteCategories();
            //_storageService.DeleteSounds();
            //_storageService.DeleteRecordings();

            ResultModel<List<CategoryModel>> result = await _storageService.RetrieveAllCategories();

            // TODO notify user if there is an error
            if (result.Success)
                Categories = result.Content;

            await GetPermissions();
        }


        private async Task GetPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            while (status is not PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            while (status is not PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Microphone>();
            }

        }

    }
}

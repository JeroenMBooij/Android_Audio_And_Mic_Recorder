using Microsoft.AspNetCore.Components;
using Soundpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Views.Shared
{
    public partial class NavMenu: ComponentBase
    {
        [Inject] private IPiPService _pipService { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        public bool IsInPictureInPictureMode { get; set; } = false;


        protected override void OnInitialized()
        {
            _pipService.Interaction.Subscribe(isInPictureInPictureMode =>
            {
                IsInPictureInPictureMode = isInPictureInPictureMode;

                StateHasChanged();

                /*if(IsInPictureInPictureMode)
                    Navigation.NavigateTo("/pip");
                else
                    Navigation.NavigateTo("/");*/

            });
        }

        public void EnterPictureInPictureMode()
        {
            _pipService.Activate();
        }
    }
}

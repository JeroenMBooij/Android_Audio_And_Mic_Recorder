using Android.App;
using Android.Util;
using Android.Views;
using Microsoft.AspNetCore.Components;
using Soundpad.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Platforms.Services
{
    public class PiPService : IPiPService
    {
        public BehaviorSubject<bool> Interaction { get; set; } = new BehaviorSubject<bool>(false);

        public PiPService()
        {
            MainActivity.Instance.PictureInPictureMode.Subscribe(isInPictureInPictureMode =>
            {
                Interaction.OnNext(isInPictureInPictureMode);
            });
        }

        public void Activate()
        {
            var windowBounds = MainActivity.Instance.WindowManager.CurrentWindowMetrics.Bounds;

            Rational aspectRatio = new Rational(windowBounds.Width() * 4, windowBounds.Height());
            PictureInPictureParams.Builder pipParams = new PictureInPictureParams.Builder();
            pipParams.SetAspectRatio(aspectRatio).Build();

            MainActivity.Instance.EnterPictureInPictureMode(pipParams.Build());
        }
    }
}

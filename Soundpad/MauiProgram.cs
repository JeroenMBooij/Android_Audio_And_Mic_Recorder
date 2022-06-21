using MatBlazor;
using Soundpad.Services.Interfaces;
using Soundpad.Services;
using Soundpad.Platforms.Services;

namespace Soundpad;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMatBlazor();
		#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
		#endif
        builder.Services.AddSingleton<IPiPService, PiPService>();
        builder.Services.AddSingleton<IStorageService, StorageService>();
        builder.Services.AddSingleton<IRecordService, RecordService>();
        builder.Services.AddSingleton<IPlaybackService, PlaybackService>();
        builder.Services.AddSingleton<IPreferenceService, PreferenceService>();

        return builder.Build();
	}
}

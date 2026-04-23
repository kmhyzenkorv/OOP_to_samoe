using Microsoft.Extensions.Logging;

namespace RomashkaMaui;

/// <summary>
/// Точка сборки MAUI-приложения: конфигурирует <see cref="MauiApp"/>,
/// регистрирует шрифты и подключает логирование в Debug-режиме.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Создаёт и настраивает экземпляр <see cref="MauiApp"/>, используемый рантаймом MAUI как корень приложения.
    /// </summary>
    /// <returns>Сконфигурированный <see cref="MauiApp"/>.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

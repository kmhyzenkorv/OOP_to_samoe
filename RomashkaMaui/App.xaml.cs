using Microsoft.Extensions.DependencyInjection;

namespace RomashkaMaui;

/// <summary>
/// Корневой класс MAUI-приложения. Отвечает за инициализацию ресурсов из App.xaml
/// и создание главного окна.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Создаёт экземпляр приложения и загружает XAML-ресурсы.
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Создаёт главное окно приложения, оболочкой которого выступает <see cref="AppShell"/>.
    /// </summary>
    /// <param name="activationState">Состояние активации, передаваемое платформой (может быть null).</param>
    /// <returns>Новое окно с <see cref="AppShell"/> в качестве корневой страницы.</returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}

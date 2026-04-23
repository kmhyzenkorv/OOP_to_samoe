namespace RomashkaMaui;

/// <summary>
/// Оболочка навигации приложения (<see cref="Shell"/>), содержащая единственную страницу <see cref="MainPage"/>.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Создаёт экземпляр оболочки и инициализирует XAML-разметку.
    /// </summary>
    public AppShell()
    {
        InitializeComponent();
    }
}

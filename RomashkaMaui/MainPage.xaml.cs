namespace RomashkaMaui;

/// <summary>
/// Главная страница GUI: отображает список документов, форму создания, детали выбранного документа
/// и историю операций. Делегирует вызовы во <see cref="DocumentsViewModel"/>.
/// </summary>
public partial class MainPage : ContentPage
{
    private readonly DocumentsViewModel _vm;

    /// <summary>
    /// Создаёт страницу, инициализирует XAML-разметку и привязывает <see cref="DocumentsViewModel"/>
    /// как <see cref="BindableObject.BindingContext"/>.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        _vm = new DocumentsViewModel();
        BindingContext = _vm;
    }

    /// <summary>Обработчик кнопки «Создать»: создаёт новый документ выбранного типа.</summary>
    private void OnCreateClicked(object? sender, EventArgs e) => _vm.CreateDocument();

    /// <summary>Обработчик кнопки «Сгенерировать»: применяет шаблон к выбранному документу.</summary>
    private void OnGenerateClicked(object? sender, EventArgs e) => _vm.GenerateSelected();

    /// <summary>Обработчик кнопки «Удалить»: удаляет выбранный документ из коллекции.</summary>
    private void OnDeleteClicked(object? sender, EventArgs e) => _vm.DeleteSelected();

    /// <summary>Обработчик кнопки «Сохранить всё»: сериализует все документы в JSON.</summary>
    private void OnSaveClicked(object? sender, EventArgs e) => _vm.Save();

    /// <summary>Обработчик кнопки «История»: отображает журнал операций из <see cref="roma_application.Logger"/>.</summary>
    private void OnHistoryClicked(object? sender, EventArgs e) => _vm.ShowHistory();
}

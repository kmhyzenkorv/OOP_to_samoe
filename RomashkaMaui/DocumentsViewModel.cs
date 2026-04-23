using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using oma_structure;
using roma_application;
using romashka_core;

namespace RomashkaMaui;

/// <summary>
/// ViewModel главной страницы: обёртка над <see cref="DocumentManager"/>, <see cref="TemplateManager"/>,
/// <see cref="PersistenceService"/> и <see cref="Logger"/>. Предоставляет свойства для data binding
/// и методы-действия, пригодные для вызова из UI без обращения к консоли.
/// </summary>
public class DocumentsViewModel : INotifyPropertyChanged
{
    private readonly DocumentManager _docManager = new();
    private readonly TemplateManager _templateManager = new();
    private readonly PersistenceService _persistence = new();
    private readonly Logger _logger = new();

    /// <summary>Наблюдаемая коллекция документов, связанная со списком в UI.</summary>
    public ObservableCollection<Document> Documents { get; } = new();

    /// <summary>Допустимые типы документов для выбора в <c>Picker</c>.</summary>
    public IReadOnlyList<string> DocumentTypes { get; } = new[] { "contract", "application", "memo" };

    private string _selectedType = "contract";
    /// <summary>Тип документа, выбранный пользователем в форме создания.</summary>
    public string SelectedType
    {
        get => _selectedType;
        set { _selectedType = value; OnPropertyChanged(); }
    }

    private string _newContent = string.Empty;
    /// <summary>Текст содержимого, введённый пользователем для нового документа.</summary>
    public string NewContent
    {
        get => _newContent;
        set { _newContent = value; OnPropertyChanged(); }
    }

    private Document? _selectedDocument;
    /// <summary>
    /// Документ, выбранный в списке. При смене значения обновляет зависимые свойства
    /// <see cref="SelectedContent"/> и <see cref="HasSelection"/>.
    /// </summary>
    public Document? SelectedDocument
    {
        get => _selectedDocument;
        set
        {
            _selectedDocument = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedContent));
            OnPropertyChanged(nameof(HasSelection));
        }
    }

    /// <summary>Признак наличия выбранного документа (используется для видимости деталей в UI).</summary>
    public bool HasSelection => _selectedDocument is not null;

    /// <summary>
    /// Содержимое выбранного документа для двусторонней привязки к редактору.
    /// При записи обновляет <see cref="Document.Content"/> и <see cref="Document.LastModified"/>.
    /// </summary>
    public string SelectedContent
    {
        get => _selectedDocument?.Content ?? string.Empty;
        set
        {
            if (_selectedDocument is null) return;
            _selectedDocument.Content = value;
            _selectedDocument.LastModified = DateTime.Now;
            OnPropertyChanged();
        }
    }

    private string _statusMessage = "Готов к работе";
    /// <summary>Сообщение в строке состояния внизу страницы.</summary>
    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    private string _historyText = string.Empty;
    /// <summary>Текст истории операций, показываемый в правой нижней области.</summary>
    public string HistoryText
    {
        get => _historyText;
        set { _historyText = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Создаёт ViewModel, загружает документы из файла через <see cref="PersistenceService"/>
    /// и синхронизирует счётчик идентификаторов.
    /// </summary>
    public DocumentsViewModel()
    {
        _docManager.setLogger(_logger);
        _docManager.setDocuments(_persistence.loadDocuments());
        _docManager.syncIds();
        RefreshDocuments();
    }

    /// <summary>
    /// Создаёт новый документ выбранного типа (<see cref="SelectedType"/>), заполняет его
    /// содержимым из <see cref="NewContent"/> и добавляет в менеджер. Обходит консольный
    /// ввод исходного класса <see cref="Document.fillData"/>.
    /// </summary>
    public void CreateDocument()
    {
        DocumentCreator creator = SelectedType switch
        {
            "contract" => new ContractDocumentCreator(),
            "application" => new ApplicationDocumentCreator(),
            "memo" => new MemoDocumentCreator(),
            _ => throw new InvalidOperationException("Неизвестный тип")
        };

        var doc = creator.createDocument();
        doc.Type = doc.getDocumentType();
        doc.Id = _docManager.generateId();
        doc.Content = NewContent ?? string.Empty;
        doc.LastModified = DateTime.Now;

        _docManager.addDocument(doc);
        StatusMessage = $"Создан документ #{doc.Id} ({doc.Type})";
        NewContent = string.Empty;
        RefreshDocuments();
    }

    /// <summary>
    /// Применяет шаблон, соответствующий типу выбранного документа, через
    /// <see cref="Document.generate"/>. Если шаблон не найден или документ не выбран,
    /// обновляет <see cref="StatusMessage"/>.
    /// </summary>
    public void GenerateSelected()
    {
        if (_selectedDocument is null)
        {
            StatusMessage = "Сначала выберите документ";
            return;
        }

        var template = _templateManager.getTemplateByType(_selectedDocument.Type);
        if (template is null)
        {
            StatusMessage = "Шаблон не найден";
            return;
        }

        _selectedDocument.generate(template);
        StatusMessage = $"Документ #{_selectedDocument.Id} сгенерирован по шаблону \"{template.Name}\"";
        OnPropertyChanged(nameof(SelectedContent));
        RefreshDocuments();
    }

    /// <summary>
    /// Удаляет выбранный документ через <see cref="DocumentManager.deleteDocument"/> и
    /// сбрасывает выбор. При отсутствии выбора сообщает об этом в строке состояния.
    /// </summary>
    public void DeleteSelected()
    {
        if (_selectedDocument is null)
        {
            StatusMessage = "Сначала выберите документ";
            return;
        }

        int id = _selectedDocument.Id;
        _docManager.deleteDocument(_selectedDocument);
        SelectedDocument = null;
        StatusMessage = $"Документ #{id} удалён";
        RefreshDocuments();
    }

    /// <summary>
    /// Сохраняет все документы в JSON-файл через <see cref="PersistenceService.saveDocuments"/>.
    /// </summary>
    public void Save()
    {
        _persistence.saveDocuments(_docManager.getAllDocuments());
        StatusMessage = $"Сохранено документов: {_docManager.getAllDocuments().Count}";
    }

    /// <summary>
    /// Получает строку истории из <see cref="Logger.show"/> путём временного перенаправления
    /// <see cref="Console.Out"/> в <see cref="StringWriter"/> — так как список логов в
    /// <see cref="Logger"/> приватный и извлекается только через консольный вывод.
    /// </summary>
    public void ShowHistory()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            _logger.show();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        HistoryText = writer.ToString().TrimEnd();
        if (string.IsNullOrWhiteSpace(HistoryText))
            HistoryText = "История пуста";
    }

    /// <summary>
    /// Перезаполняет <see cref="Documents"/> текущим содержимым <see cref="DocumentManager"/>.
    /// Вызывается после любой операции, меняющей состав коллекции.
    /// </summary>
    private void RefreshDocuments()
    {
        Documents.Clear();
        foreach (var d in _docManager.getAllDocuments())
            Documents.Add(d);
    }

    /// <summary>Событие изменения свойства для поддержки data binding.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Генерирует <see cref="PropertyChanged"/> для указанного (или вызывающего) свойства.
    /// </summary>
    /// <param name="name">Имя свойства; по умолчанию подставляется вызывающим членом.</param>
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

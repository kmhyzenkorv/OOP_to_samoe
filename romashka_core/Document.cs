using romashka_core;

/// <summary>
/// Базовый абстрактный класс документа: содержит общие поля и операции ввода/обновления содержимого.
/// </summary>
public abstract class Document
{
    public int Id { get; set; }
    public string Type { get; set; }

    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;
    public string Status { get; set; }
    public string Content { get; set; }

    public Template Template { get; set; }

    /// <summary>
    /// Возвращает строковый идентификатор типа документа. Переопределяется в наследниках.
    /// </summary>
    public virtual string getDocumentType()
    {
        return "";
    }

    /// <summary>
    /// Запрашивает у пользователя исходное содержимое документа и обновляет дату изменения.
    /// </summary>
    public virtual void fillData()
    {
        Console.Write("Введите текст: ");
        Content = Console.ReadLine();
        LastModified = DateTime.Now;
    }

    /// <summary>
    /// Привязывает шаблон к документу и помечает содержимое префиксом имени шаблона.
    /// </summary>
    /// <param name="template">Шаблон, используемый для генерации.</param>
    public virtual void generate(Template template)
    {
        Template = template;
        Content = $"[Template: {template.Name}] {Content}";
        LastModified = DateTime.Now;
    }

    /// <summary>
    /// Запрашивает у пользователя новое содержимое документа.
    /// </summary>
    public void updateContent()
    {
        Console.Write("Новый текст: ");
        Content = Console.ReadLine();
        LastModified = DateTime.Now;
    }

    /// <summary>
    /// Запрашивает у пользователя новый статус документа.
    /// </summary>
    public void changeStatus()
    {
        Console.Write("Новый статус: ");
        Status = Console.ReadLine();
    }
}

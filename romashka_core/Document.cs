using romashka_core;

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

    public virtual string getDocumentType()
    {
        return "";
    }

    public virtual void fillData()
    {
        Console.Write("Введите текст: ");
        Content = Console.ReadLine();
        LastModified = DateTime.Now;
    }

    public virtual void generate(Template template)
    {
        Template = template;
        Content = $"[Template: {template.Name}] {Content}";
        LastModified = DateTime.Now;
    }

    public void updateContent()
    {
        Console.Write("Новый текст: ");
        Content = Console.ReadLine();
        LastModified = DateTime.Now;
    }

    public void changeStatus()
    {
        Console.Write("Новый статус: ");
        Status = Console.ReadLine();
    }
}

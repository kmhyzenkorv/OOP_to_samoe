using oma_structure;
using roma_application;

/// <summary>
/// Точка входа консольного приложения для управления документами.
/// Инициализирует менеджеры, загружает данные и запускает меню работы с документами.
/// </summary>
class Program
{
    /// <summary>
    /// Главный метод. Отображает меню и обрабатывает команды пользователя в цикле.
    /// </summary>
    static void Main()
    {
        var docManager = new DocumentManager();
        var templateManager = new TemplateManager();
        var persistence = new PersistenceService();
        var invoker = new Invoker();
        var logger = new Logger();

        docManager.setLogger(logger);
        docManager.setDocuments(persistence.loadDocuments());
        docManager.syncIds();

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Создать документ");
            Console.WriteLine("2 - Сгенерировать документ");
            Console.WriteLine("3 - Показать документы");
            Console.WriteLine("4 - Удалить документ");
            Console.WriteLine("5 - Сохранить");
            Console.WriteLine("6 - Показать историю");
            Console.WriteLine("0 - Выход");

            try
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("contract / application / memo");
                        string type = Console.ReadLine();

                        DocumentCreator creator = type switch
                        {
                            "contract" => new ContractDocumentCreator(),
                            "application" => new ApplicationDocumentCreator(),
                            "memo" => new MemoDocumentCreator(),
                            _ => throw new Exception("Неверный тип")
                        };

                        invoker.setCommand(new CreateDocumentCommand(creator, docManager));
                        break;

                    case "2":
                        invoker.setCommand(new GenerateDocumentCommand(docManager, templateManager));
                        break;

                    case "3":
                        invoker.setCommand(new OpenDocumentCommand(docManager));
                        break;

                    case "4":
                        invoker.setCommand(new DeleteDocumentCommand(docManager));
                        break;

                    case "5":
                        invoker.setCommand(new SaveDocumentCommand(docManager, persistence));
                        break;

                    case "6":
    invoker.setCommand(new ShowLogCommand(logger));
    break;

                    case "0":
                        return;

                    default:
                        throw new Exception("Ошибка ввода");
                }

                invoker.execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

using romashka_core;

namespace roma_application
{
    /// <summary>
    /// Абстрактный создатель документов (паттерн "Фабричный метод").
    /// </summary>
    public abstract class DocumentCreator
    {
        /// <summary>
        /// Создаёт экземпляр конкретного типа документа.
        /// </summary>
        public abstract Document createDocument();
    }

    /// <summary>
    /// Создатель документов типа "contract".
    /// </summary>
    public class ContractDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт документ-контракт.</summary>
        public override Document createDocument() => new ContractDocument();
    }

    /// <summary>
    /// Создатель документов типа "application".
    /// </summary>
    public class ApplicationDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт документ-заявление.</summary>
        public override Document createDocument() => new ApplicationDocument();
    }

    /// <summary>
    /// Создатель документов типа "memo".
    /// </summary>
    public class MemoDocumentCreator : DocumentCreator
    {
        /// <summary>Создаёт документ-служебную записку.</summary>
        public override Document createDocument() => new MemoDocument();
    }
}
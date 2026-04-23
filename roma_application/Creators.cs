using romashka_core;

namespace roma_application
{
    public abstract class DocumentCreator
    {
        public abstract Document createDocument();
    }

    public class ContractDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new ContractDocument();
    }

    public class ApplicationDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new ApplicationDocument();
    }

    public class MemoDocumentCreator : DocumentCreator
    {
        public override Document createDocument() => new MemoDocument();
    }
}
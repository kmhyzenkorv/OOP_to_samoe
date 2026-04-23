namespace romashka_core
{
    public class ContractDocument : Document
    {
        public override string getDocumentType() => "contract";
    }

    public class ApplicationDocument : Document
    {
        public override string getDocumentType() => "application";
    }

    public class MemoDocument : Document
    {
        public override string getDocumentType() => "memo";
    }
}
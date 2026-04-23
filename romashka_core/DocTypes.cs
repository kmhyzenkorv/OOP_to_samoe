namespace romashka_core
{
    /// <summary>
    /// Документ типа "contract" (договор).
    /// </summary>
    public class ContractDocument : Document
    {
        /// <summary>Возвращает идентификатор типа документа.</summary>
        public override string getDocumentType() => "contract";
    }

    /// <summary>
    /// Документ типа "application" (заявление).
    /// </summary>
    public class ApplicationDocument : Document
    {
        /// <summary>Возвращает идентификатор типа документа.</summary>
        public override string getDocumentType() => "application";
    }

    /// <summary>
    /// Документ типа "memo" (служебная записка).
    /// </summary>
    public class MemoDocument : Document
    {
        /// <summary>Возвращает идентификатор типа документа.</summary>
        public override string getDocumentType() => "memo";
    }
}
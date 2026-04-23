using romashka_core;
using System.Collections.Generic;
using System.Linq;

namespace roma_application
{
    /// <summary>
    /// Менеджер шаблонов документов: содержит встроенные шаблоны и позволяет искать их по типу.
    /// </summary>
    public class TemplateManager
    {
        private List<Template> templates = new()
    {
        new Template { TemplateId = 1, Name = "Contract Template", DocumentType = "contract" },
        new Template { TemplateId = 2, Name = "Application Template", DocumentType = "application" },
        new Template { TemplateId = 3, Name = "Memo Template", DocumentType = "memo" }
    };

        /// <summary>
        /// Возвращает шаблон, соответствующий указанному типу документа, или null при отсутствии.
        /// </summary>
        /// <param name="type">Тип документа (например, "contract").</param>
        public Template getTemplateByType(string type)
            => templates.FirstOrDefault(t => t.DocumentType == type);
    }
}
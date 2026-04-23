using romashka_core;
using System.Collections.Generic;
using System.Linq;

namespace roma_application
{
    public class TemplateManager
    {
        private List<Template> templates = new()
    {
        new Template { TemplateId = 1, Name = "Contract Template", DocumentType = "contract" },
        new Template { TemplateId = 2, Name = "Application Template", DocumentType = "application" },
        new Template { TemplateId = 3, Name = "Memo Template", DocumentType = "memo" }
    };

        public Template getTemplateByType(string type)
            => templates.FirstOrDefault(t => t.DocumentType == type);
    }
}
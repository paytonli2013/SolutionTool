using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StyleCop;
using StyleCop.CSharp;

namespace Orc.StyleCop.CSharp.Rules
{
    /// <summary>
    /// This StyleCop Rule makes sure that instance variables are prefixed with an underscore.
    /// </summary>
    [SourceAnalyzer(typeof(CsParser))]
    public class InstanceVariablesUnderscorePrefix : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            CsDocument csdocument = (CsDocument)document;
            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
                csdocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.VisitElement), null, null);
        }

        private bool VisitElement(CsElement element, CsElement parentElement, object context)
        {
            // Flag a violation if the instance variables are not prefixed with an underscore.
            if (!element.Generated && element.ElementType == ElementType.Field && element.ActualAccess != AccessModifierType.Public &&
                element.ActualAccess != AccessModifierType.Internal && element.Declaration.Name.ToCharArray()[0] != '_')
            {
                AddViolation(element, "InstanceVariablesUnderscorePrefix");
            }
            return true;
        }
    }
}
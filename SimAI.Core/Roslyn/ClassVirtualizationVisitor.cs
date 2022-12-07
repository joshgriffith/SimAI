using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SimAI.Core.Roslyn {
    public class ClassVirtualizationVisitor : CSharpSyntaxRewriter {
        List<string> _classes = new();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
            node = (ClassDeclarationSyntax) base.VisitClassDeclaration(node);

            string className = node.Identifier.ValueText;
            _classes.Add(className);

            return node;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Roslyn;

namespace Runtime.Ext.Compiler.Preprocess
{
    public class Internalization : ICompileModule
    {
        public Internalization(IServiceProvider services)
        {
        }

        public void BeforeCompile(IBeforeCompileContext context)
        {
            var replacementDict = new Dictionary<SyntaxTree, SyntaxTree>();
            var removeList = new List<SyntaxTree>();

            foreach (var tree in context.CSharpCompilation.SyntaxTrees)
            {
                if (tree.FilePath.IndexOf("Newtonsoft") == -1)
                {
                    continue;
                }

                if (string.Equals("AssemblyInfo.cs", Path.GetFileName(tree.FilePath),
                    StringComparison.OrdinalIgnoreCase))
                {
                    removeList.Add(tree);
                    continue;
                }

                //var root = tree.GetRoot();

                //var targetSyntaxKinds = new[] {
                //    SyntaxKind.ClassDeclaration,
                //    SyntaxKind.InterfaceDeclaration,
                //    SyntaxKind.StructDeclaration,
                //    SyntaxKind.EnumDeclaration
                //};

                //var typeDeclarations = root.DescendantNodes()
                //    .Where(x => targetSyntaxKinds.Contains(x.Kind()))
                //    .OfType<BaseTypeDeclarationSyntax>();
                //var publicKeywordTokens = new List<SyntaxToken>();

                //foreach (var declaration in typeDeclarations)
                //{
                //    var publicKeywordToken = declaration.Modifiers
                //        .SingleOrDefault(x => x.Kind() == SyntaxKind.PublicKeyword);
                //    if (publicKeywordToken != default(SyntaxToken))
                //    {
                //        publicKeywordTokens.Add(publicKeywordToken);
                //    }
                //}

                //if (publicKeywordTokens.Any())
                //{
                //    root = root.ReplaceTokens(publicKeywordTokens,
                //        (_, oldToken) => SyntaxFactory.ParseToken("internal").WithTriviaFrom(oldToken));
                //}

                //replacementDict.Add(tree,
                //    SyntaxFactory.SyntaxTree(root, tree.Options, tree.FilePath, tree.GetText().Encoding));
            }

            context.CSharpCompilation = context.CSharpCompilation.RemoveSyntaxTrees(removeList);
            foreach (var pair in replacementDict)
            {
                context.CSharpCompilation = context.CSharpCompilation.ReplaceSyntaxTree(pair.Key, pair.Value);
            }
        }

        public void AfterCompile(IAfterCompileContext context)
        {

        }

        private static bool IsChildOfDirectory(string dir, string candidate)
        {
            dir = Path.GetFullPath(dir);
            dir = dir.EndsWith(Path.DirectorySeparatorChar.ToString()) ? dir : dir + Path.DirectorySeparatorChar;
            candidate = Path.GetFullPath(candidate);
            return candidate.StartsWith(dir, StringComparison.OrdinalIgnoreCase);
        }
    }
}

namespace Microsoft.Framework.Runtime
{
    [AssemblyNeutral]
    public class AssemblyNeutralAttribute : Attribute
    {
    }

    /// <summary>
    /// Summary description for ICompileModule
    /// </summary>
    [AssemblyNeutral]
    public interface ICompileModule
    {
        void BeforeCompile(IBeforeCompileContext context);

        void AfterCompile(IAfterCompileContext context);
    }

    [AssemblyNeutral]
    public interface IBeforeCompileContext
    {
        CSharpCompilation CSharpCompilation { get; set; }

        IList<ResourceDescription> Resources { get; }

        IList<Diagnostic> Diagnostics { get; }
    }

    [AssemblyNeutral]
    public interface IAfterCompileContext
    {
        CSharpCompilation CSharpCompilation { get; set; }

        IList<Diagnostic> Diagnostics { get; }
    }
}

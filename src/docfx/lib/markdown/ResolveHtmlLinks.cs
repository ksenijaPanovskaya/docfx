// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Markdig;
using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;

namespace Microsoft.Docs.Build
{
    internal static class ResolveHtmlLinks
    {
        public static MarkdownPipelineBuilder UseResolveHtmlLinks(this MarkdownPipelineBuilder builder, MarkdownContext context, StrongBox<bool> hasHtml)
        {
            return builder.Use(document =>
            {
                document.Visit(node =>
                {
                    if (node is HtmlBlock block)
                    {
                        block.Lines = new StringLineGroup(ResolveLinks(block.Lines.ToString()));
                        hasHtml.Value = true;
                    }
                    else if (node is HtmlInline inline)
                    {
                        inline.Tag = ResolveLinks(inline.Tag);
                        hasHtml.Value = true;
                    }
                    return false;
                });
            });

            string ResolveLinks(string html)
            {
                return HtmlUtility.TransformLinks(html, href => context.GetLink(href, InclusionContext.File));
            }
        }
    }
}

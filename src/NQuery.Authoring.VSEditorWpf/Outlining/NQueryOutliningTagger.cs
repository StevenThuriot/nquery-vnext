using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

using NQuery.Authoring.Composition.Outlining;
using NQuery.Authoring.Outlining;

namespace NQuery.Authoring.VSEditorWpf.Outlining
{
    internal sealed class NQueryOutliningTagger : AsyncTagger<IOutliningRegionTag, OutliningRegionSpan>
    {
        private readonly Workspace _workspace;
        private readonly IOutliningService _outliningService;

        public NQueryOutliningTagger(Workspace workspace, IOutliningService outliningService)
        {
            _workspace = workspace;
            _outliningService = outliningService;
            _workspace.CurrentDocumentChanged += WorkspaceOnCurrentDocumentChanged;
            InvalidateTags();
        }

        private void WorkspaceOnCurrentDocumentChanged(object sender, EventArgs e)
        {
            InvalidateTags();
        }

        protected override async Task<Tuple<ITextSnapshot, IEnumerable<OutliningRegionSpan>>> GetRawTagsAsync()
        {
            var document = _workspace.CurrentDocument;
            var syntaxTree = await document.GetSyntaxTreeAsync();
            var snapshot = document.GetTextSnapshot();
            var result = syntaxTree.Root.FindRegions(_outliningService.Outliners);
            return Tuple.Create(snapshot, result.AsEnumerable());
        }

        protected override ITagSpan<IOutliningRegionTag> CreateTagSpan(ITextSnapshot snapshot, OutliningRegionSpan rawTag)
        {
            var textSpan = rawTag.Span;
            var span = new Span(textSpan.Start, textSpan.Length);
            var snapshotSpan = new SnapshotSpan(snapshot, span);
            var hint = snapshot.GetText(span);
            var tag = new OutliningRegionTag(false, false, rawTag.Text, hint);
            var tagSpan = new TagSpan<IOutliningRegionTag>(snapshotSpan, tag);
            return tagSpan;
        }
    }
}
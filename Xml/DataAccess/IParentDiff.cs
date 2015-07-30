using System;
using System.Collections.Generic;

namespace DotNEET.Xml.DataAccess
{
    public interface IParentDiff
    {
        void AddDiffs(IEnumerable<ModifyEntry> diffs);

        bool IsTaggedAsRemoved(Guid id);

        void TagAsRemoved(Guid id);

        bool TryGetDiff(Guid id, out ModifyEntry diff);
    }
}
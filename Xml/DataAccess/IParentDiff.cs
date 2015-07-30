using System;
using System.Collections.Generic;

namespace DotNEET.Xml.DataAccess
{
    public interface IParentDiff
    {
        bool IsTaggedAsRemoved(Guid id);
        void TagAsRemoved(Guid id);
        void AddDiffs(IEnumerable<ModifyEntry> diffs);
        bool TryGetDiff(Guid id, out ModifyEntry diff);


    }
}
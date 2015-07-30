using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DotNEET.Xml.DataAccess
{
    [Serializable]
    [XmlRoot("ParentDiff")]
    [XmlInclude(typeof(Guid))]
    [XmlInclude(typeof(ModifyEntry))]
    public class ParentDiff : IParentDiff
    {
        [XmlArray("Modify"), XmlArrayItem(typeof(ModifyEntry), ElementName = "ModifyEntry")]
        public List<ModifyEntry> Modify;

        [XmlArray("Remove"), XmlArrayItem(typeof(Guid), ElementName = "RemoveEntry")]
        public HashSet<Guid> RemoveList;

        [XmlIgnore]
        [NonSerialized]
        private readonly LazyCache<Dictionary<Guid, ModifyEntry>> modifyById;

        public ParentDiff()
        {
            this.RemoveList = new HashSet<Guid>();
            this.Modify = new List<ModifyEntry>();
            this.modifyById = new LazyCache<Dictionary<Guid, ModifyEntry>>(() => this.Modify.ToDictionary(x => x.IdEntry));
        }

        [XmlIgnore]
        public IReadOnlyDictionary<Guid, ModifyEntry> ModifyById
        {
            get
            {
                return this.modifyById.Value;
            }
        }

        public void AddDiffs(IEnumerable<ModifyEntry> diffs)
        {
            diffs = diffs.ToList();
            var removeGuids = diffs.Select(x => x.IdEntry).ToHashSet();
            this.Modify.RemoveAll(x => removeGuids.Contains(x.IdEntry)); // Avoid a diff to apply multiple time
            this.Modify.AddRange(diffs);
            this.modifyById.Reset();
        }

        public bool IsTaggedAsRemoved(Guid id)
        {
            return this.RemoveList.Contains(id);
        }

        public void TagAsRemoved(Guid id)
        {
            this.RemoveList.Add(id);
        }

        public bool TryGetDiff(Guid id, out ModifyEntry diff)
        {
            return this.ModifyById.TryGetValue(id, out diff);
        }
    }
}
using DotNEET;
using DotNEET.Extensions;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNEET.Xml.DataAccess
{
    public abstract class DiffContextAccess<TDataContext> : IDisposable
        where TDataContext : IDataContext// The constraints may be trying too hard, but better safe than sorry
    {
        private IReadOnlyDictionary<Guid, ModifyEntry> modifyById;
        private readonly TDataContext context;
        
        // null if no parent
        private readonly DiffContextAccess<TDataContext> parent;
        // null if not parent
        private readonly IParentDiff parentDiff;

        private readonly Executer queryExecuter;

        public DiffContextAccess(TDataContext context, DiffContextAccess<TDataContext> parent = null)
        {
            this.queryExecuter = new Executer();
            this.context = context.ThrowIfNull("Context cannot be null");
            this.parentDiff = this.context.ParentDiff; // Can be null, and it's perfectly fine
            this.parent = parent;
            
        }

        public void Delete<T>(Func<TDataContext, ICollection<T>> selector, IEnumerable<T> items) where T : XmlEntity
        {
            items = items.ToList(); //Just to be sure
            this.queryExecuter.AddAction(() =>
                {
                    var collection = selector(this.context);
                    items.ForEach(x =>
                        {
                            if (x.Owner == this)
                            {
                                collection.Remove(x); //directly remove those items on save
                            }
                            else
                            {
                                this.parentDiff.TagAsRemoved(x.Id); //Mark in parent diff if it's parent related
                            }
                        });
                });
        }

        public Task DeleteAsync<T>(Func<TDataContext, ICollection<T>> selector, IEnumerable<T> items) where T : XmlEntity
        {
            //items = items.ToList(); //Just to be sure
            return this.queryExecuter.AddActionAsync(() =>
            {
                var collection = selector(this.context);
                items.ForEach(x =>
                {
                    if (x.Owner == this)
                    {
                        collection.Remove(x); //directly remove those items on save
                    }
                    else
                    {
                        this.parentDiff.TagAsRemoved(x.Id); //Mark in parent diff if it's parent related
                    }
                });
            });
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Insert<T>(Func<IDataContext, ICollection<T>> selector, IEnumerable<T> items) where T : XmlEntity
        {
            items = items.ToList(); //Just to be sure
            this.queryExecuter.AddAction(() =>
                {
                    //add to context
                    var collection = selector(this.context);
                    items.Pipe(x => x.Owner = this).ForEach(x => collection.Add(x));
                });
        }

        public void SaveChanges()
        {
            this.queryExecuter.AddAction(() =>
                {
                    this.context.Sync();
                });
        }

        public Task SavesChangesAsync()
        {
            return this.queryExecuter.AddActionAsync(() =>
                {
                    this.context.Sync();
                });
        }

        public IReadOnlyCollection<T> Select<T>(Func<IDataContext, IEnumerable<T>> selector) where T : XmlEntity
        {
            return this.queryExecuter.AddFunc(() => this.ApplyDiff(selector(this.context).Pipe(x => x.Owner = this).Concat(this.parent == null ? Enumerable.Empty<T>() : parent.Select(selector))));
        }

        public Task<IReadOnlyCollection<T>> SelectAsync<T>(Func<IDataContext, IEnumerable<T>> selector) where T : XmlEntity
        {
            return this.queryExecuter.AddFuncAsync(() => this.ApplyDiff(selector(this.context).Pipe(x => x.Owner = this).Concat(this.parent == null ? Enumerable.Empty<T>() : parent.Select(selector))));
        }

        public void Update<T>(IEnumerable<T> items) where T : XmlEntity
        {
            //items = items.ToList(); //Just to be sure
            this.queryExecuter.AddAction(() =>
                {
                    if (this.parentDiff != null)
                    {
                        this.parentDiff.AddDiffs(items.Where(x => x.Owner != this).Select(x => x.GetDiff()));
                    }
                    //other entity will be saved directly with Save() call
                });
        }

        public async Task UpdateAsync<T>(IEnumerable<T> items) where T : XmlEntity
        {
            //items = items.ToList(); //Just to be sure
            await this.queryExecuter.AddActionAsync(() =>
            {
                if (this.parentDiff != null)
                {
                    this.parentDiff.AddDiffs(items.Where(x => x.Owner != this).Select(x => x.GetDiff()));
                }
                //other entity will be saved directly with Save() call
            });
        }

        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.queryExecuter.Dispose();
                if (this.parent != null)
                {
                    this.parent.Dispose();
                }
            }
        }

        private IReadOnlyCollection<T> ApplyDiff<T>(IEnumerable<T> items) where T : XmlEntity
        {
            if (this.parent == null)
            {
                return items.ToList(); // shortcut
            }
            return this.ApplyDiffInternal(items).ToList(); //avoid direct modification of list by users
        }

        private IEnumerable<T> ApplyDiffInternal<T>(IEnumerable<T> items) where T : XmlEntity
        {
            ModifyEntry modifyEntry;
            foreach (var item in items) //this include current context and parent context items
            {
                if (!this.parentDiff.IsTaggedAsRemoved(item.Id))
                {
                    if (this.parentDiff.TryGetDiff(item.Id, out modifyEntry))
                    {
                        item.ApplyDiff(modifyEntry);
                    }
                    yield return item;
                }
            }
        }
    }
}
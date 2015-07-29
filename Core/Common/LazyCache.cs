using System;

namespace DotNEET
{
    public class LazyCache<TModel>
    {
        private readonly Func<TModel> _valueFactory;
        private Lazy<TModel> _lazyObj;

        public LazyCache(Func<TModel> valueFactory)
        {
            this._lazyObj = new Lazy<TModel>(valueFactory);
            _valueFactory = valueFactory;
        }

        public TModel Value
        {
            get { return _lazyObj.Value; }
        }

        public void Reset()
        {
            _lazyObj = new Lazy<TModel>(_valueFactory);
        }
    }
}
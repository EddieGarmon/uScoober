using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public class WeakCache
    {
        private readonly Hashtable _cache = new Hashtable();

        public void Add(object key, object target) {
            _cache.Add(key, new WeakReference(target));
        }

        public void Clear() {
            _cache.Clear();
        }

        public void DisposeItemsAndClear() {
            foreach (WeakReference reference in _cache.Values) {
                if (!reference.IsAlive) {
                    continue;
                }
                var disposable = reference.Target as IDisposable;
                if (disposable != null) {
                    disposable.Dispose();
                }
            }
            _cache.Clear();
        }

        public object GetIfActive(object key) {
            if (_cache.Contains(key)) {
                WeakReference reference = (WeakReference)_cache[key];
                if (reference.IsAlive) {
                    return reference.Target;
                }
                _cache.Remove(key);
            }
            return null;
        }
    }
}
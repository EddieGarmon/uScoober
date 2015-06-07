using System;
using System.Collections;

namespace uScoober.DataStructures
{
    public partial class Ring
    {
        public class Enumerator : DisposableBase,
                                  IEnumerator
        {
            private readonly int _editVersion;
            private readonly bool _isReversed;
            private int _currentIndex;
            private Link _currentLink;
            private bool _isDone;
            private Ring _ring;

            protected internal Enumerator(Ring ring, bool isReversed = false) {
                _ring = ring;
                _editVersion = _ring.EditVersion;
                _isReversed = isReversed;
                Reset();
            }

            public virtual object Current {
                get {
                    if (_currentLink == null) {
                        throw new InvalidOperationException("Invalid enumerator state.");
                    }
                    return _currentLink.Value;
                }
            }

            public int CurrentIndex {
                get {
                    if (_currentLink == null) {
                        throw new InvalidOperationException("Invalid enumerator state.");
                    }
                    return _currentIndex;
                }
            }

            public Link CurrentLink {
                get { return _currentLink; }
            }

            object IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                GuardListNotEdited();
                if (_isDone) {
                    return false;
                }
                if (_currentLink == null) {
                    if (_ring.Count == 0) {
                        _isDone = true;
                        return false;
                    }
                    // begin at the appropriate end
                    if (!_isReversed) {
                        _currentLink = _ring.Head;
                        _currentIndex = 0;
                    }
                    else {
                        _currentLink = _ring.Tail;
                        _currentIndex = _ring.Count - 1;
                    }
                    return _currentLink != null;
                }
                if (_isReversed) {
                    _currentLink = _currentLink.Previous;
                    _currentIndex--;
                    if (_currentLink != _ring.Tail) {
                        return true;
                    }
                    _isDone = true;
                    _currentLink = null;
                    return false;
                }
                _currentLink = _currentLink.Next;
                _currentIndex++;
                if (_currentLink != _ring.Head) {
                    return true;
                }
                _isDone = true;
                _currentLink = null;
                return false;
            }

            public void Reset() {
                GuardListNotEdited();
                _isDone = false;
                _currentLink = null;
            }

            protected override void DisposeManagedResources() {
                _currentLink = null;
                _ring = null;
            }

            private void GuardListNotEdited() {
                if (_editVersion != _ring.EditVersion) {
                    throw new InvalidOperationException("List changed during enumeration.");
                }
            }
        }
    }
}
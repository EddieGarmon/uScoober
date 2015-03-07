using System;
using uScoober.TestFramework.Assert;

namespace uScoober.Threading
{
    public class CancellationSourceTests
    {
        public void CancelationSourceLifecycle_Fact() {
            CancellationSource source;
            using (source = new CancellationSource()) {
                source.IsCancelationRequested.ShouldBeFalse();
                source.IsDisposed.ShouldBeFalse();
                source.Cancel();
                source.IsCancelationRequested.ShouldBeTrue();
                source.IsDisposed.ShouldBeFalse();
                source.Cancel();
                source.IsCancelationRequested.ShouldBeTrue();
                source.IsDisposed.ShouldBeFalse();
            }
            source.IsDisposed.ShouldBeTrue();
        }

        public void CreateCancelableTokens_Fact() {
            new CancellationSource().Token.CanBeCanceled.ShouldBeTrue();
            ((CancellationToken)new CancellationSource()).CanBeCanceled.ShouldBeTrue();
        }

        public void CreateUncancellableTokens_Fact() {
            CancellationToken.None.CanBeCanceled.ShouldBeFalse();
        }

        public void DisposedThrows_Fact() {
            CancellationSource source;
            using (source = new CancellationSource()) {
                source.IsCancelationRequested.ShouldBeFalse();
            }
            try {
                source.Cancel();
            }
            catch (ObjectDisposedException) {
                return;
            }
            throw new Exception("CancellationSource not disposed.");
        }

        public void MergeAndCancelFirst_Fact() {
            var source1 = new CancellationSource();
            var source2 = new CancellationSource();
            var merged = CancellationSource.Merge(source1, source2);
            source1.Cancel();
            source1.IsCancelationRequested.ShouldBeTrue();
            source2.IsCancelationRequested.ShouldBeFalse();
            merged.IsCancelationRequested.ShouldBeTrue();
            //ensure safety in calling other cancels
            source2.Cancel();
            merged.Cancel();
        }

        public void MergeAndCancelMerged_Fact() {
            var source1 = new CancellationSource();
            var source2 = new CancellationSource();
            var merged = CancellationSource.Merge(source1, source2);
            merged.Cancel();
            source1.IsCancelationRequested.ShouldBeFalse();
            source2.IsCancelationRequested.ShouldBeFalse();
            merged.IsCancelationRequested.ShouldBeTrue();
            //ensure safety in calling other cancels
            source1.Cancel();
            source2.Cancel();
        }

        public void MergeAndCancelSecond_Fact() {
            var source1 = new CancellationSource();
            var source2 = new CancellationSource();
            var merged = CancellationSource.Merge(source1, source2);
            source2.Cancel();
            source1.IsCancelationRequested.ShouldBeFalse();
            source2.IsCancelationRequested.ShouldBeTrue();
            merged.IsCancelationRequested.ShouldBeTrue();
            //ensure safety in calling other cancels
            source1.Cancel();
            merged.Cancel();
        }

        public void MergeNullSources_Fact() {
            CancellationSource.Merge(null, null)
                              .ShouldBeNull();
            CancellationSource.Merge(CancellationSource.Canceled, null)
                              .ShouldEqual(CancellationSource.Canceled);
            CancellationSource.Merge(null, CancellationSource.Canceled)
                              .ShouldEqual(CancellationSource.Canceled);
        }

        public void RegisterAfterCancel_Fact() {
            object callbackCreated = null;
            var source = new CancellationSource();
            source.Cancel();
            var token = source.Token;
            token.Register(() => callbackCreated = new object());
            callbackCreated.ShouldNotBeNull();
        }

        public void RegisterBeforeCancel_Fact() {
            object callbackCreated = null;
            var source = new CancellationSource();
            source.Token.Register(() => callbackCreated = new object());
            callbackCreated.ShouldBeNull();
            source.Cancel();
            callbackCreated.ShouldNotBeNull();
        }
    }
}
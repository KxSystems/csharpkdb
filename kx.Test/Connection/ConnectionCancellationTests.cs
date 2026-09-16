using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace kx.Test.Connection
{
    [TestFixture]
    public class ConnectionCancellationTests
    {
        [Test]
        public async Task CancellingAsyncWriteClosesConnection()
        {
            using (var stream = new CancellableStream())
            using (var connection = new c(stream))
            using (var cancellation = new CancellationTokenSource())
            {
                Task write = connection.ksAsync("test", cancellation.Token);
                await stream.WriteStarted.Task;

                cancellation.Cancel();

                Assert.CatchAsync<OperationCanceledException>(async () => await write);
                Assert.AreEqual(cancellation.Token, stream.WriteCancellationToken);
                Assert.IsTrue(stream.IsDisposed);
            }
        }

        [Test]
        public async Task CancellingAsyncReadClosesConnection()
        {
            using (var stream = new CancellableStream())
            using (var connection = new c(stream))
            using (var cancellation = new CancellationTokenSource())
            {
                Task read = connection.k0Async(cancellation.Token);
                await stream.ReadStarted.Task;

                cancellation.Cancel();

                Assert.CatchAsync<OperationCanceledException>(async () => await read);
                Assert.AreEqual(cancellation.Token, stream.ReadCancellationToken);
                Assert.IsTrue(stream.IsDisposed);
            }
        }

        private sealed class CancellableStream : Stream
        {
            internal TaskCompletionSource<bool> ReadStarted { get; } =
                new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            internal TaskCompletionSource<bool> WriteStarted { get; } =
                new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            internal CancellationToken ReadCancellationToken { get; private set; }

            internal CancellationToken WriteCancellationToken { get; private set; }

            internal bool IsDisposed { get; private set; }

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override Task<int> ReadAsync(
                byte[] buffer,
                int offset,
                int count,
                CancellationToken cancellationToken)
            {
                ReadCancellationToken = cancellationToken;
                ReadStarted.TrySetResult(true);
                return WaitForReadCancellation(cancellationToken);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override Task WriteAsync(
                byte[] buffer,
                int offset,
                int count,
                CancellationToken cancellationToken)
            {
                WriteCancellationToken = cancellationToken;
                WriteStarted.TrySetResult(true);
                return Task.Delay(Timeout.Infinite, cancellationToken);
            }

#if NETCOREAPP3_1_OR_GREATER
            public override ValueTask<int> ReadAsync(
                Memory<byte> buffer,
                CancellationToken cancellationToken = default)
            {
                ReadCancellationToken = cancellationToken;
                ReadStarted.TrySetResult(true);
                return new ValueTask<int>(WaitForReadCancellation(cancellationToken));
            }

            public override ValueTask WriteAsync(
                ReadOnlyMemory<byte> buffer,
                CancellationToken cancellationToken = default)
            {
                WriteCancellationToken = cancellationToken;
                WriteStarted.TrySetResult(true);
                return new ValueTask(Task.Delay(Timeout.Infinite, cancellationToken));
            }
#endif

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }

            private static async Task<int> WaitForReadCancellation(
                CancellationToken cancellationToken)
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
                return 0;
            }
        }
    }
}

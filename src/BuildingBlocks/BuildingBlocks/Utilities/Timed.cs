using System.Diagnostics;

namespace BuildingBlocks.Utilities;

public class Timed(Action<TimeSpan> afterMeasuredAction) : IDisposable
{
    private readonly Action<TimeSpan> _afterMeasuredAction = afterMeasuredAction ?? (_ => { });
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private bool isDisposed;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!isDisposed && disposing)
        {
            _stopwatch.Stop();
            _afterMeasuredAction.Invoke(_stopwatch.Elapsed);

            isDisposed = true;
        }
    }
}

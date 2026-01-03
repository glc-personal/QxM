using Qx.Domain.Devices.Models;

namespace Qx.Domain.Devices;

public interface ILinearAxis
{
    Task HomeAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    Task SetSpeedAsync(AxisSpeedMmPerSec speed, CancellationToken cancellationToken = default);
    Task MoveAbsoluteAsync(AxisPositionMm position, CancellationToken cancellationToken = default);
    Task MoveRelativeAsync(AxisDeltaMm delta, CancellationToken cancellationToken = default);
    Task<AxisStatus> GetStatusAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<AxisStatus> StreamStatusesAsync(AxisStatusStreamOptions options, CancellationToken cancellationToken = default);
}
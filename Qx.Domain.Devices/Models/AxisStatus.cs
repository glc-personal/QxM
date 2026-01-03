using Qx.Core.Measurement;
using Qx.Domain.Devices.States;

namespace Qx.Domain.Devices.Models;

public readonly record struct AxisStatus(AxisPositionMm AxisPositionMm, AxisSpeedMmPerSec AxisSpeedMmPerSec, MotorState State);
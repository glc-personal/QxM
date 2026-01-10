# README

# Overview
ICS requires a single connecting interface to the ICB’s CAN bus; 
access to this CAN bus is therefore provided to other 
services within ICS via this gateway service. The Hardware 
Gateway implements this behavior via a gRPC service, 
exposing macro level actions/commands. The Hardware Gateway 
is responsible for hiding primitive and atomic 
actions/commands that depend on the hardware devices on 
the CAN bus, therefore if hardware changes the exposed 
macro layer will remain fixed. Therefore we will use 3 
layers:
- Primitive Layer: low-level actions/commands, e.g. open/close valve, raw motor moves, etc. (internal only).
- Atomic Layer: composition of primitives, e.g. MoveToPosition (Gateway API, limited exposure)
- Macro Layer: composition of atomics and primitives, e.g. TransferSample, BeadCapture, etc. (Orchestrator uses this)

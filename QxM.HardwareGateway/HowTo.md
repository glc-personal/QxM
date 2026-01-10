# HowTo

## Prerequisites
- `grpcurl` is needed which can be from
  - `brew install grpcurl` (macOS)
- gRPC service is running (localhost:8080 in this case)

## Check all services available
```shell
grpcurl -plaintext \
  -import-path /Users/glc/RiderProjects/QxM/QxM.HardwareGateway \
  -proto Protos/hardware_gateway.proto \
  localhost:8080 \
  list
```
Output:
```shell
qxm.ics.HardwareGateway
```

## Check the descriptions of the API
```shell
grpcurl -plaintext \
  -import-path /Users/glc/RiderProjects/QxM/QxM.HardwareGateway \
  -proto Protos/hardware_gateway.proto \
  localhost:8080 \
  describe
```
Output:
```shell
qxm.ics.HardwareGateway is a service:
// Hardware Gateway Service Definition:
service HardwareGateway {
  // Check the state of a hardware command request
  rpc CheckCommand ( .qxm.ics.CheckCommandRequest ) returns ( .qxm.ics.CheckCommandResponse );
  // Send a hardware command request
  rpc SendCommand ( .qxm.ics.SendCommandRequest ) returns ( .qxm.ics.SendCommandResponse );
  // Subscribe to a hardware command
  rpc SubscribeToCommand ( .qxm.ics.SubscribeToCommandRequest ) returns ( stream .qxm.ics.CommandEvent );
}
```

## Test an RPC call
```shell
grpcurl -plaintext \
  -import-path /Users/glc/RiderProjects/QxM/QxM.HardwareGateway \
  -proto Protos/hardware_gateway.proto \
  -d '{
    "runId": "1234",
    "idempotencyKey": "1234",
    "spec": {
      "dispense": {
        "volumeUl": 50.0,
        "pressure": 1.2,
        "timeoutSeconds": 30
      }
    }
  }' \
  localhost:8080 \
  qxm.ics.HardwareGateway/SendCommand
```
Output:
```shell
{
  "commandId": "Test"
}
```
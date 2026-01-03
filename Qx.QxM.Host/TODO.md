# TODO

## Tasks:
- replace Qx.Application.Services Dtos with Commands/Queries where commands mutate data whereas queries get data and don't mutate it.
- replace Qx.QxM.Host Dtos with Request/Responses
- introduce entity ids rather than just Guid for everyone to prevent issues in the future with compile time protection (e.g. ConsumableId)
- let the API controllers map Requests to app service commands or queries, the application returns a result model (could be a domain object or application model), then the controller maps this back to a Response
- If you feel you are building “repos for everything,” pause—EF already is a repository/unit-of-work. A common compromise is: Application defines repository interfaces for the few aggregates you actually load/save (Inventory, Run, etc.)
- utilize factories for In domain-driven design, an object's creation is often separated from the object itself.
- Domain: entities/value objects, invariants, domain services, factory (if no IO)
- Application: use-cases, orchestration, transactions, interfaces/ports (IInventoryRepository, IDeviceController)
- Infrastructure: EF repositories, device drivers, implementations of ports
- Definition has “models”; Instance has “state”
- Use OPTIMISTIC CONCURRENCY when doing the EF database work

# README

## Overview
`Qx.Infrastructure.Persistence` is responsible for implementing all 
repository interfaces. We go from `Entity Framework (EF)` `Entities`
to using a `DbContext` (`QxMDbContext`) in our case to set up our
`EF` migrations.

`dotnet ef migrations InitialCreate -p Qx.Infrastructure.Persistence -s Qx.QxM.Host`

for an initial migration but `InitialCreate` can be changed for later
migrations.

Once the database has been updated our `DbContext` (`QxMDbContext`) can be
used in our `Repositories` (e.g. `InventoryRepository`) for ways to 
interact with our database and application services.

An example path is: `Entity` > `DbContext` > `Repository` > `Application Service` > `Host API Controller`
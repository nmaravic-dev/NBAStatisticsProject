REST API for tracking and analyzing NBA statistics — players, teams, games, per-game stats, injuries, and derived analytics.

Status: Work in progress.

## Technologies

* ASP.NET Core Web API (.NET 9)
* Entity Framework Core (code-first) + SQL Server (LocalDB)
* ASP.NET Core Identity + JWT authentication
* Scalar (OpenAPI UI)

## Architecture

Layered: Controllers (HTTP) → Services (business logic) → EF Core (data access). Controllers depend on service interfaces, not on the DbContext — no data access leaks into the presentation layer.

## Current

* Relational model: Team, Player, Game, PlayerGameStat, Injury
* Full CRUD for all entities, plus bulk create
* Service layer per entity; controllers only translate results to HTTP
* DTOs on every endpoint — entities are never exposed
* Input validation: data annotations for format, service-level checks for business rules (FK existence, date sanity, no team playing itself)
* Soft delete for teams: a team that has played games stays in the record
* Per-player stats aggregation (games, totals, averages)
* Injury Susceptibility Score — a 1–10 availability rating derived from a player's injuries, weighting missed games by injury severity and overlapping injury periods with the team's schedule
* JWT authentication with Identity: register, login, [Authorize]-protected endpoints
* Personal watchlist: authenticated users follow players; ownership scoped to the token, never the request

## Planned

* Player comparison (head-to-head stats)
* Data ingestion from external NBA API
* Deployment (live instance)
* Unit tests for the injury score logic

## Notes on decisions

* Mapping extensions over AutoMapper — keeps projections explicit and visible in code, and lets EF translate them to SQL.
* Service layer over a repository — EF Core's DbSet already acts as a repository, so Controller → Service → DbContext stays clean without an extra abstraction.
* No JsonPatch — it needed a separate mutable DTO and double mapping without adding anything over PUT.
* Derived values (averages, injury score) are computed on read, not stored — the API keeps facts and derives the rest.
* JWT key currently in appsettings for local dev — must move to user secrets / environment variables before deployment.

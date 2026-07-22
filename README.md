# NBAStatisticsProject

REST API for tracking and analyzing NBA statistics — players, teams, games, per-game stats, injuries, and derived analytics.

**Live demo:** https://nbastatsmaravic.fly.dev/scalar/v1

The database is seeded with sample teams, players, games, and injuries. To try the `[Authorize]`-protected endpoints (watchlist), register your own account via `POST /api/Auth/register`, or use the demo account:

* Email: `demo@nbastats.com`
* Password: `Demo123!`

Copy the token from the login response into the `Authorization: Bearer <token>` header to access protected endpoints.

## Technologies

* ASP.NET Core Web API (.NET 9)
* Entity Framework Core (code-first) + PostgreSQL
* ASP.NET Core Identity + JWT authentication
* Docker (multi-stage build)
* Deployed on Fly.io with a Neon PostgreSQL database
* Scalar (OpenAPI UI)

## Architecture

Layered: Controllers (HTTP) → Services (business logic) → EF Core (data access). Controllers depend on service interfaces, not on the DbContext — no data access leaks into the presentation layer.

## Features

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

## Deployment

Containerized with a multi-stage Dockerfile and deployed to Fly.io. The database is PostgreSQL hosted on Neon. Secrets (connection string, JWT key) are provided via environment variables, never committed. Migrations are applied automatically on startup. The app runs behind Fly's HTTPS proxy, with forwarded-header handling so OpenAPI reports the correct HTTPS server.

## Planned

* Player comparison (head-to-head stats)
* Data ingestion from external NBA API
* Unit tests for the injury score logic

## Notes on decisions

* Mapping extensions over AutoMapper — keeps projections explicit and visible in code, and lets EF translate them to SQL.
* Service layer over a repository — EF Core's DbSet already acts as a repository, so Controller → Service → DbContext stays clean without an extra abstraction.
* No JsonPatch — it needed a separate mutable DTO and double mapping without adding anything over PUT.
* Derived values (averages, injury score) are computed on read, not stored — the API keeps facts and derives the rest.

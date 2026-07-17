# NBA Statistics API

REST API for tracking and analyzing NBA statistics — players, teams, games, and per-game stats.

> **Status:** Work in progress.

## Technologies

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core (code-first) + SQL Server (LocalDB)
- Scalar (OpenAPI UI)

## Current

- Relational model: Team, Player, Game, PlayerGameStat
- Full CRUD for all four entities, plus bulk create
- DTOs on every endpoint — entities are never exposed
- Soft delete for teams: a team that has played games stays in the record
- Query projections extracted into mapping extensions

## Planned

- Per-game stats aggregation and player comparison
- Injury Susceptibility Score (rule-based)
- Player watchlist with user authentication
- Data ingestion from external NBA API

## Notes on decisions

- **Mapping extensions over AutoMapper** — keeps projections explicit and visible in code, and lets EF translate them to SQL.
- **No JsonPatch** — it needed a separate mutable DTO and double mapping without adding anything over PUT.

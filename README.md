# NBA Statistics API

REST API for tracking and analyzing NBA statistics — players, teams, games, and per-game stats.

> **Status:** Work in progress.

## Technologies

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core (code-first) + SQL Server (LocalDB)
- Scalar (OpenAPI UI)

## Current

- Player CRUD (GET / POST / PUT / PATCH / DELETE) via EF Core
- Relational model: Team, Player, Game, PlayerGameStat

## Planned

- Per-game stats and player comparison
- Injury Susceptibility Score (rule-based)
- Player watchlist with user authentication
- Data ingestion from external NBA API

# Ideas

## Data model
* Split Player.Name into FirstName/LastName (needs migration)
* PlayerTeamHistory entity — track transfers, so stats belong to the team the player was on at the time
* Soft delete for Player (like teams already have)

## Validation & error handling
* Update endpoints: distinguish 404 (entity not found) from 400 (invalid FK like TeamId) — currently both return the same response since the service returns null for both cases. Needs a richer result type (enum/Result) or exception-based handling.
* Refactor validation to FluentValidation — currently split between data annotations (format) on DTOs and manual checks in services (business rules like FK existence, date-not-in-future, home≠away). FluentValidation would consolidate both into one validator per DTO, improving readability and keeping all rules in one discoverable place. Async rules (MustAsync) can also hit the DB for existence checks.

## Architecture experiments
* Try MediatR/CQRS on one module (e.g. injury score or auth) — currently using a service-per-entity layer, which is simpler for CRUD. MediatR would show command/query separation and pipeline behaviors (logging, validation as cross-cutting). Keep services elsewhere to contrast both approaches.
* Upgrade .NET 9 → 10 as a separate exercise: bump target framework, update all packages to matching major versions, resolve breaking changes. Do this in isolation, not during another big change.

## Features
* Player comparison endpoint — compare two players head-to-head on their stats (points, rebounds, assists averages). Belongs in AnalyticsController, separate from watchlist. Was in the original MVP scope.
* Data ingestion from an external NBA API — HttpClient-based sync service, an ExternalId on entities to map source records, rate limiting, and error handling for when the source is down. Largest and riskiest piece; would replace manual data entry and the bulk endpoints.
* NBA betting predictions → multi-sport (long-term direction)

## Performance
* InjuryScore GetAll: N+1 — loops per player, each call hits the DB separately. Optimize by loading all injuries/games/stats once and computing in memory.

## Testing
* Unit tests for the injury score logic (xUnit) — feed known injuries/games, assert the computed score. It's pure rule-based logic, so it's the easiest and most valuable thing to test first.

## Deployment & security
* Fix CORS to a specific origin instead of AllowAnyOrigin once a frontend exists — currently open because it's a public demo API.
* The dev JWT key was committed in early history — treat it as compromised. Production uses a separate key via Fly secrets. Consider scrubbing history (BFG) if it ever matters, though low priority for a portfolio.
* Add an appsettings.Development.example.json (placeholder values, committed) so the required config keys are documented without leaking secrets.
* Bulk (CreateMany) endpoints are intentionally unvalidated — revisit once external API ingestion is built, since that will determine whether they stay or get replaced by a sync service.

## Frontend
* Blazor frontend consuming the API — would make the project demoable beyond the Scalar UI.

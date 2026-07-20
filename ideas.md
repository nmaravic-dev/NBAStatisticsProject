# Ideas

- Split Player.Name into FirstName/LastName (needs migration)
- PlayerTeamHistory entity — track transfers, so stats belong to the team the player was on at the time
- Soft delete for Player?
- NBA betting predictions → multi-sport
- Update endpoints: distinguish 404 (entity not found) from 400 (invalid FK like TeamId) — currently both return the same response since the service returns null for both cases. Needs a richer result type (enum/Result) or exception-based handling.
- Refactor validation to FluentValidation — currently split between data annotations (format) on DTOs and manual checks in services (business rules like FK existence, date-not-in-future, home≠away). FluentValidation would consolidate both into one validator per DTO, improving readability and keeping all rules in one discoverable place. Async rules (MustAsync) can also hit the DB for existence checks.
- - Try MediatR/CQRS on one module (e.g. injury score or auth) — currently using a service-per-entity layer, which is simpler for CRUD. MediatR would show command/query separation and pipeline behaviors (logging, validation as cross-cutting). Keep services elsewhere to contrast both approaches.

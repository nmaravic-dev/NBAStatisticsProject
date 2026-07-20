# Ideas

- Split Player.Name into FirstName/LastName (needs migration)
- PlayerTeamHistory entity — track transfers, so stats belong to the team the player was on at the time
- Soft delete for Player?
- NBA betting predictions → multi-sport
- Update endpoints: distinguish 404 (entity not found) from 400 (invalid FK like TeamId) — currently both return the same response since the service returns null for both cases. Needs a richer result type (enum/Result) or exception-based handling.

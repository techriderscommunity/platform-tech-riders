# Local Run with Podman

This project runs locally with Podman only (no Docker required).

## Prerequisites

1. Podman installed.
2. Podman machine started.
3. A `.env` file in repository root.

## 1) Prepare environment variables

Create `.env` and set strong values.

Required keys:

```dotenv
SQL_SA_PASSWORD=ChangeMe_Strong_123!
JWT_KEY=ChangeMe_Jwt_Key_AtLeast32Chars
Database__UseInMemory=true
```

`Database__UseInMemory=true` lets backend run even when Azure SQL access is not available yet.
If `Database__UseInMemory=true`, `podman-up.ps1` skips the SQL container and runs the API against the in-memory fallback.
If `Database__UseInMemory=false`, `SQL_SA_PASSWORD` is required and the SQL container is started.

## 2) Start stack

Run from repository root:

```powershell
./scripts/setup/podman-up.ps1
```

Optional dry run:

```powershell
./scripts/setup/podman-up.ps1 -DryRun
```

## 3) Endpoints

- Frontend: http://localhost:4200
- API Swagger: http://localhost:8080/swagger
- SQL Server: localhost,14333 when `Database__UseInMemory=false`

## 4) Stop stack

```powershell
./scripts/setup/podman-down.ps1
```

## Notes

- SQL data persists in Podman volume `techriders_sql_data`.
- Container images are built from:
  - `backend/TechRiders.API/Containerfile`
  - `techito/Containerfile`

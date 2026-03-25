# GameStore.API Progress Journal

This file tracks what was implemented, what problems were faced, and how they were fixed.
The goal is to make future continuation easy.

## Current status (quick view)

- API is running with JWT authentication through Keycloak.
- OpenAPI JSON endpoint is enabled.
- Swagger UI is enabled and supports Bearer token authorization.
- Controller endpoints require authorization by default.
- Root endpoint and OpenAPI endpoint are public.

## Day-by-day progress

### Day 1 (1 hour)

- Started .NET project in VS Code.
- Configured development environment.
- Configured REST Client extension for API testing.
- Created DTOs (`GameDTO`, `CreateGameDTO`) using C# records.
- Implemented initial endpoints and tested them.
- Pushed code to GitHub and validated clone/run flow.

### Day 2 (2 hours)

- Started authentication design.
- Evaluated IAM options and selected Keycloak.
- Installed Keycloak locally (also handled JDK setup issues).
- Learned Realm/Client basics and OAuth2 authorization-code flow.
- Configured authentication middleware in `Program.cs`.

### Day 3 (2 hours)

- Migrated authentication from cookie-based to JWT-based.
- Updated JWT configuration in `Program.cs`.
- Enabled required service account roles in Keycloak.

### Day 4 (2 hours)

- Learned Docker basics (image/container/Dockerfile).
- Containerized API.
- Ran Keycloak in Docker.
- Configured authority URL for container networking.
- Used `host.docker.internal` so API container can reach host services.
- Validated token generation and API authorization in Postman.

### Day 5 (2 hours)

- Ran PostgreSQL in Docker on port `5432`.
- Connected API to PostgreSQL using container-network host/port.
- Added DB-backed CRUD behavior.
- Configured Entity Framework Core integration.

### Day 6 (1 hour)

- Refactored project structure.
- Organized layers: `Controllers`, `Services`, `Interfaces`, `DTOs`, `Persistence`, `Entities`, `Migrations`.

### Day 7 (recent)

#### Goal
Enable OpenAPI/Swagger in the API.

#### Problem faced
- `AddOpenApi()` gave compile error even after adding package.

#### Root cause
- Target framework is `net9.0`, but `Microsoft.AspNetCore.OpenApi` was on major version `10.x` (mismatch).

#### Fix
- Aligned OpenAPI package to `9.x`.

#### Result
- Build succeeded and OpenAPI services became available.

### Day 8 (recent)

#### Goal
Expose OpenAPI JSON and Swagger UI endpoints.

#### Problems faced
- `http://localhost:5000/openapi/v1.json` returned `401 Unauthorized`.
- Port `5000` conflicts happened (`address already in use`).
- Route testing confusion (`/api/game` vs actual route).

#### Root causes
- Global fallback authorization policy protected docs endpoints too.
- Older running API processes were still bound to port `5000`.
- Controller route template is `[Route("api/allgames")]`.

#### Fixes
- Switched to `DefaultPolicy` and applied authorization at controller mapping:
  - `app.MapControllers().RequireAuthorization();`
- Kept docs public:
  - `app.MapOpenApi().AllowAnonymous();`
- Verified with clean run on alternate port and stopped stale processes.

#### Result
- Swagger UI and OpenAPI JSON reachable.
- API controllers remain protected.

### Day 9 (recent)

#### Goal
Make Swagger accept Authorization header.

#### Work done
- Added Bearer security scheme to generated OpenAPI document.
- Added global OpenAPI security requirement.
- Enabled Swagger auth persistence.

#### Result
- Swagger shows Authorize option.
- Bearer token can be supplied from UI and sent in requests.

## Important commands used during troubleshooting

- Check process listening on port 5000:
  - `Get-NetTCPConnection -LocalPort 5000 -State Listen`
- Check process details by PID:
  - `Get-Process -Id <pid>`
- Stop stale API process:
  - `Stop-Process -Id <pid> -Force`
- Run API on custom port:
  - `dotnet run --urls http://localhost:5056`

## Known route notes

- Controller base route currently: `/api/allgames`
- Public docs route: `/openapi/v1.json`
- Swagger UI route: `/swagger`

## Pending / next improvements

- Add production-grade structured logging (Serilog + sink).
- Add request correlation ID and tracing.
- Standardize route naming (`/api/games` style) if needed.
- Add automated tests for auth + docs endpoints.

## CI/CD implementation notes

### What is implemented now

- GitHub Actions workflow file exists at `.github/workflows/ci.yml`.
- Workflow is currently configured to run on push to `main`.
- Workflow name is `CD`.
- It builds Docker image and pushes image to Docker Hub.
- Image tags used:
  - `saisumanthkedarisetti/sumanth:latest`
  - `saisumanthkedarisetti/sumanth:<git-sha>`

### Pipeline flow (current)

1. Developer pushes code to `main`.
2. GitHub Action starts on `ubuntu-latest` runner.
3. Source code checkout happens.
4. Docker Hub login happens using GitHub secrets.
5. Docker image is built from `Dockerfile`.
6. Built image is pushed to Docker Hub with latest and commit SHA tag.

### Required GitHub secrets

- `DOCKERHUB_USERNAME`
- `DOCKERHUB_TOKEN`

If these are missing or incorrect, image push fails.

### Manual CD currently used

Current deployment is manual using Docker Compose with image pull/run behavior.

Typical manual deployment flow:

1. Pull latest image:
   - `docker pull saisumanthkedarisetti/sumanth:latest`
2. Start or refresh containers:
   - `docker compose down`
   - `docker compose up -d`
3. Verify running containers:
   - `docker ps`
4. Verify API health endpoint:
   - `http://localhost:5005/`

### Runtime stack from compose

- PostgreSQL container on `5432`
- Keycloak container on `8080`
- API container on `5005`

All are connected through `gamestore-net` Docker network.

### Problems faced in CI/CD and how they were handled

- Problem: deployment confusion between CI and CD responsibilities.
  - Clarification: workflow currently does image build and push (CD artifact stage), while runtime deployment is still manual.

- Problem: old containers/processes causing stale behavior.
  - Fix: stop old runtime and restart containers before retesting.

- Problem: environment differences between local and container.
  - Fix: use container-aware configuration and shared Docker network.

### Current maturity level

- CI/CD is partially automated.
- Automated part: build and push image on main branch.
- Manual part: deploy/redeploy using docker compose commands.

### Next CI/CD improvements planned

- Add a real CI job before CD:
  - restore, build, test, and fail pipeline if tests fail.
- Split workflow into clear stages (`ci.yml` and `cd.yml`) or separate jobs.
- Add environment-based deployment strategy (dev/stage/prod).
- Add rollout verification step after deploy.
- Add rollback instructions and rollback command section.



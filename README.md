# JobFollower

A full-stack job application tracker with a drag-and-drop Kanban board. Built to actually use during my own job search. Track applications from "Not Applied" through "Interviewed" to "Accepted" or "Rejected" without juggling spreadsheets.

**Live demo:** https://job-follower.vercel.app

***If u don't wish to use your own email feel free to use the sample account:***

**Email:** *user@user.com*

**Password:** *user*

> **Note on cross-browser auth:** the token cookies currently relies on cross-origin cookies between the frontend (Vercel) and backend (Render) domains.
>  Chrome allows this by default; Safari and Firefox block third-party cookies by default and will block you from doing all functions and
>  will log you out on page refresh. This is a known limitation of the split-domain deployment (not a bug) — the fix is putting both services under
>  a shared parent domain, which is on the list of future improvements below. Currently use Chrome for full experience.

## Features

- **Authentication** — registration, login, and logout with hashed passwords (`PasswordHasher`), short-lived JWT access tokens, and rotating refresh tokens stored server-side (hashed, single-use, revoked on each refresh)
- **Kanban board** — drag and drop job applications between status columns (Not Applied, Applied, Interviewed, Rejected, Ghosted, Accepted), backed by [`@dnd-kit`](https://dndkit.com/)
- **Full CRUD** — create, edit, and delete job applications through a modal form, with server-side validation errors surfaced in the UI
- **Ownership-scoped data** — every job application is tied to the authenticated user; API endpoints verify ownership before returning or mutating data (protects against IDOR)
- **Resilient sessions** — access tokens live in memory only (not `localStorage`, to limit XSS exposure); an `httpOnly` refresh-token cookie silently restores the session on page reload
- **Cold-start awareness** — since the backend runs on Render's free tier, the UI detects slow responses and shows a "server is waking up" message rather than a frozen screen

## Tech stack

**Backend**
- ASP.NET Core (Minimal APIs) on .NET
- Entity Framework Core + Npgsql (PostgreSQL with a database hosted on Neon)
- JWT Bearer authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Repository → Service → Endpoint layering with DTOs at the API boundary

**Frontend**
- React + TypeScript (Vite)
- React Router for client-side routing and protected routes
- Axios, with interceptors that attach the access token and silently refresh it on `401`
- `@dnd-kit` for drag-and-drop
- Plain CSS Modules with a shared design-token stylesheet (dark theme)

**Infrastructure**
- [Neon](https://neon.tech) — serverless PostgreSQL
- [Render](https://render.com) — backend hosting (Docker)
- [Vercel](https://vercel.com) — frontend hosting

## Architecture notes

- **Auth flow:** login issues a short-lived JWT (in-memory on the client) and a long-lived refresh token (hashed, stored server-side, delivered as an `httpOnly`/`Secure`/`SameSite=None` cookie). Every refresh **rotates** the token — the old one is revoked and a new one issued — limiting the blast radius if a token is ever leaked.
- **Ownership checks:** every job-related endpoint reads the caller's user ID from the validated JWT claims, never from client-supplied input, and filters every query by that ID before returning or mutating a row.
- **DTOs are never entities:** request and response shapes are explicit, separate types from the EF Core models — in particular, password hashes are never included in any API response, and create/update DTOs never expose server-assigned fields like primary keys.

## Running locally

### Backend
1. `cd JobFollower/Backend`
2. Set up a PostgreSQL connection string (e.g. a free [Neon](https://neon.tech) project) and add it to your user secrets or `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=...;Database=...;Username=...;Password=...;SSL Mode=VerifyFull;Channel Binding=Require;",
       "DefaultFrontEnd": "http://localhost:5173"
     },
     "Jwt": {
       "Key": "a-long-random-secret-at-least-32-characters",
       "Issuer": "JobFollower",
       "Audience": "JobFollowerClient"
     }
   }
   ```
3. Apply migrations: `dotnet ef database update`
4. Run: `dotnet run` (defaults to `https://localhost:7096`)

### Frontend
1. `cd JobFollower/Frontend/jobfollower-frontend`
2. `npm install`
3. Create a `.env` file:
   ```
   VITE_API_URL=https://localhost:7096
   ```
4. `npm run dev`

## Possible future improvements

- Move frontend and backend under a shared custom domain to resolve the cross-browser cookie limitation
- Refresh-token theft detection (revoke the full session family if a rotated-out token is reused)
- Optimistic UI rollback indicators (currently silent on failure)
- Automated tests (xUnit for the API, integration tests for the auth flow)

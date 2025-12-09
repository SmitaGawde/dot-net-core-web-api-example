# MVC Web App consuming API with JWT + Refresh (ASP.NET Core 8)

This zip contains:
- **ApiServer**: Demo API with `/api/auth/login`, `/api/auth/refresh`, and `/api/products` (protected).
- **MvcClient**: MVC app that logs in to the API, stores tokens in cookie claims, and uses a DelegatingHandler to auto-refresh on 401.

## Quick Start

1. Install .NET 8 SDK and Visual Studio 2022+.
2. Extract the zip. Open `MvcApiJwtSample.sln`.
3. Set **multiple startup projects**: ApiServer and MvcClient (both `Start`).
   - ApiServer will run on `https://localhost:5001` (Kestrel default dev cert).
   - MvcClient will run on `https://localhost:5003` (typical next free port).
4. In **MvcClient/appsettings.json**, ensure `Api:BaseUrl` matches the ApiServer URL (default already set).
5. Run.
6. In MVC app, go to **/Account/Login** and sign in with `admin/admin` or `user/user`.
7. Visit **/Home/Products**. The access token lifetime is set to 2 minutes for easy testing:
   - After it expires, the `TokenHandler` will automatically call `/api/auth/refresh` and retry the request.

## Notes

- For demo, refresh tokens are stored in-memory in the API and rotated on use.
- For production:
  - Store refresh tokens hashed in DB with expiry/IP/user-agent binding and revocation.
  - Use HTTPS everywhere and set strict cookie settings.
  - Consider server-side token storage instead of cookie claims.
  - Add CSRF protection to POST forms.

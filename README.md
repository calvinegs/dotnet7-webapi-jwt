本文將記錄如何一步步從無到有，使用 Dotnet Core 7.0 建立 ASP.NET Core Web API，其中將會使用到下列技術:

- Dotnet CLI
- Entity Framework 7.0
- Json Web Token
- PostgreSQL DB (Docker Version)
- ASP.NET Core Generator

## 專案完成後的檔案結構

```
./專案目錄
├── .config/
│   └── dotnet-tools.json
├── .vscode/
│   ├── launch.js
│   └── tasks.json
├── Controller/
│   ├── AuthenticateController.cs
│   ├── TodoController.cs
│   └── WeatherForecast.cs
├── Data/
│   └── ApiDbContext.cs
├── Migrations/
├── Models/
│   ├── AuthenticateData.cs
│   └── TodoList.cs
├── obj/
├── Properties/
│   └── launchSettings.json
├── .gitignore
├── appsettings.Development.json
├── appsettings.json
├── dotnet7-webapi-jwt.csproj
├── global.json
├── Program.cs
├── README.md
└── WeatherForecast.cs
```

## 專案完成後所提供的 API 端點

|Methods|Urls|Actions|
|-------|-----------------------------|-------------------------------------------------|
|POST| /api/Authenticate/login|註冊新使用者帳號|
|POST| /api/Authenticate/register|使用者帳號登入|
|POST| /api/Authenticate/register-admin|管理者帳號登入|
|GET| /api/Todos|get all Todos|
|POST| /api/Todos|add New Todo|
|GET| /api/Todos/:id|get Todo by id|
|PUT| /api/Todos/:id|update Todo by id|
|DELETE| /api/Todos/:id|remove Todo by id|
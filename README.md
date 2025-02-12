<div align="center">
  <img src="https://www.getadigital.com/EPiServer/CMS/Content/globalassets/images/geta-logo.png,,2648_4790?epieditmode=true" alt="Geta" width="400"/>
  <h1>Geta Packages Foundation Sandbox</h1>
  <p>Made specially to use for open source packages</p>

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Docker](https://img.shields.io/badge/Docker-✓-2496ED)](https://www.docker.com)
[![Aspire Supported](https://img.shields.io/badge/.NET_Aspire-✓-512BD4)](https://learn.microsoft.com/en-us/dotnet/aspire/)

</div>

---

## 🚀 Overview

This sandbox is a fork of [Optimizely Foundation](https://github.com/episerver/Foundation) designed to serve as either:
- A standalone development environment
- A submodule for Geta's open-source packages

Featuring integrated [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/) support for streamlined orchestration and enhanced developer productivity.

---

## 🌟 Key Features/Concept

- **Dual Operation Modes**
    - 🧩 Submodule integration
    - 🖥️ Standalone project
- **Foundation Project**
    - 📦 Modular architecture covering CMS, Commerce, Personalization, Search, and more
- **Aspire Foundation.AppHost**
    - 🐳 Docker-based environment
    - 📊 Centralized dashboard monitoring

### Concept

Instead of referencing all existing open-source packages into the Optimizely Foundation project, 
it is possible to use this project’s codebase as a submodule for an open-source package and run a web project with a specific configuration.

This approach allows us to:
- Follow the single-responsibility principle.
- Reuse foundation code in open-source repositories by using a submodule.
- Keep package-specific functionality within its respective project.

---

## 🛠️ Prerequisites
| Aspire project                                                         | .Net project (Windows)                                                              | .Net project (Linux)                                                    | .Net project (MacOs) |
|------------------------------------------------------------------------|-------------------------------------------------------------------------------------|-------------------------------------------------------------------------|----------------------|
| [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) | [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)       | [Docker Desktop](https://docs.docker.com/desktop/setup/install/linux/)  | [Docker Desktop](https://docs.docker.com/desktop/setup/install/linux/)                     |
| [Docker Desktop](https://www.docker.com/products/docker-desktop)       | [Docker Desktop](https://www.docker.com/products/docker-desktop)                    | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)                      |
| [Node.js](https://nodejs.org/en/download/)                             | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)              | [Node.js](https://nodejs.org/en/download/)                              | [Node.js](https://nodejs.org/en/download/)                     |
|                                                                        | [Node.js](https://nodejs.org/en/download/)                                          |                                                                         |                      |

---

## 🏁 Getting Started
### 🧪 Quick Start with Aspire (Recommended)
```bash
    git clone https://github.com/Geta/geta-packages-foundation-sandbox.git
    cd geta-packages-foundation-sandbox/src/Foundation.AppHost
    dotnet run 
```
### 🖥️ Standalone Setup
```bash
   git clone https://github.com/Geta/geta-packages-foundation-sandbox.git
   cd geta-packages-foundation-sandbox/src/Foundation
   
   # Windows
   ./setup.cmd

   # Linux/macOS
   chmod +x setup.sh
   ./setup.sh
   
   dotnet run
```

## 🔑 Default Credentials
### Admin Access
<code>admin@example.com</code> / <code>Episerver123!</code>


## ❓ FAQ
### How to Use as a Submodule
1. **Add submodule to your project:**
   ```bash
   cd yourProjectDirectory
   mkdir sandbox
   cd sandbox
   git submodule add https://github.com/Geta/geta-packages-foundation-sandbox.git
   ```
2. **Create web project (.net >= 8):**
   ```bash
   cd yourProjectDirectory
   mkdir src
   dotnet new web --name yourProjectName.Web --output src/yourProjectName.Web
   ```
3. **Add Foundation reference and modules importer**
   ```xml
    <!-- yourProjectName.Web.csproj -->
    <ItemGroup>
      <ProjectReference Include="..\..\sandbox\src\Foundation\Foundation.csproj" />
    </ItemGroup>
   
    <Import Project="..\..\sandbox\geta-packages-foundation-sandbox\src\Foundation\modules\ModulesInclude.proj"/>
   ```
4. **Create startup.cs file**
    ```cs
    public class Startup : Foundation.Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration config): base(env, config) { }
    
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            // Custom services here
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            ... // Custom services here
        }
    }
    ```
5. **Adjust program.cs file**
    ```cs
    using yourProjectNameWebNamespace;
    
    Host.CreateDefaultBuilder(args)
    .ConfigureCmsDefaults()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.UseContentRoot(Path.GetFullPath("../../sandbox/geta-packages-foundation-sandbox/src/Foundation"));
    })
    .Build()
    .Run();

    ```
   
6. **Add AppHostConfiguration to appsettings.json**
    ```json
    "AppHost": {
      "SqlServerName": "yourSqlServerName",
      "SqlServerPort": 1433,
      "CmsDatabaseName": "yourProjectName-cms",
      "CommerceDatabaseName": "yourProjectName-commerce",
      "WebName": "yourProjectName",
      "WebPort": 5001
    }

    ```
7. **Run Foundation.AppHost**   


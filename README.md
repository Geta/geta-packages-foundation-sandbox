<a href="https://github.com/episerver/Foundation"><img src="https://www.optimizely.com/globalassets/02.-global-images/navigation/optimizely_logo_navigation.svg" title="Foundation" alt="Foundation"></a>

# Latest version
  This version is built for net6.0.  For the legacy full framework version please use the [full-framework/master](https://github.com/episerver/Foundation/tree/full-framework/master)

## Foundation 

Foundation offers a starting point that is intuitive, well-structured and modular allowing developers to explore CMS, Commerce, Personalization, Search amd Navigaion, Data Platform and Experimentation.

## Prerequisites
| Aspire project                                                       | .Net project (Windows)                                                        | .Net project (Linux)                                                    | .Net project (MacOs) |
|----------------------------------------------------------------------|-------------------------------------------------------------------------------|-------------------------------------------------------------------------|----------------------|
| [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) | [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | [Docker Desktop](https://docs.docker.com/desktop/setup/install/linux/)  | [Docker Desktop](https://docs.docker.com/desktop/setup/install/linux/)                     |
| [Docker Desktop](https://www.docker.com/products/docker-desktop)     | [Docker Desktop](https://www.docker.com/products/docker-desktop)              | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)                      |
|                                                                      | [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)        | [Node.js](https://nodejs.org/en/download/)                              | [Node.js](https://nodejs.org/en/download/)                     |
|                                                                      | [Node.js](https://nodejs.org/en/download/)                                    |                                                                         |                      |

## Local Development
### Option 1: Aspire Project (Recommended)
1. **Clone Repository**
   ```bash
   git clone https://github.com/Geta/geta-packages-foundation-sandbox.git
   cd geta-packages-foundation-sandbox/src/Foundation.AppHost
2. **Run application**
   ```bash
   dotnet run --project Foundation.AppHost
3. **Access Dashboard**
   ```
   Open the Aspire dashboard and navigate from the dashboard to https://localhost:5001/

### Option 2: Regular project
1. **Clone Repository**
   ```bash
   git clone https://github.com/Geta/geta-packages-foundation-sandbox.git
   cd geta-packages-foundation-sandbox/src/Foundation
2. **Setup Environment**
   ```bash
    # Windows
    setup.cmd
    
    # macOS/Linux
    chmod +x setup.sh
    ./setup.sh
3. **Run application**
   ```bash
   dotnet run --project ./src/Foundation/Foundation.csproj
---

## Access Credentials
### Default Admin Account
🔑 <code>admin@example.com</code> / <code>Episerver123!</code>



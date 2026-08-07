param(
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

function Invoke-PodmanCommand {
    param(
        [string[]]$CommandArgs
    )

    if ($DryRun) {
        Write-Host "[DRY-RUN] podman $($CommandArgs -join ' ')"
        return
    }

    Write-Host "[EXEC] podman $($CommandArgs -join ' ')"
    & podman @CommandArgs
}

function Ensure-ContainerRemoved {
    param([string]$Name)

    $exists = podman ps -a --format "{{.Names}}" | Where-Object { $_ -eq $Name }
    if ($exists) {
        Invoke-PodmanCommand -CommandArgs @("rm", "-f", $Name)
    }
}

Write-Host "Starting Tech Riders stack with Podman..."

if (-not (Test-Path ".env")) {
    throw "Missing .env file. Set local values before starting the stack."
}

$envMap = @{}
$envLines = Get-Content .env | Where-Object { $_ -and -not $_.StartsWith('#') }
foreach ($line in $envLines) {
    $parts = $line -split '=', 2
    if ($parts.Count -eq 2) {
        $envMap[$parts[0].Trim()] = $parts[1].Trim()
    }
}

$useInMemoryDatabase = $false
if ($envMap.ContainsKey('Database__UseInMemory')) {
    $useInMemoryDatabase = [System.Convert]::ToBoolean($envMap['Database__UseInMemory'])
}

Invoke-PodmanCommand -CommandArgs @("network", "exists", "techriders-net")
if ($LASTEXITCODE -ne 0) {
    Invoke-PodmanCommand -CommandArgs @("network", "create", "techriders-net")
}

if (-not $useInMemoryDatabase) {
    Invoke-PodmanCommand -CommandArgs @("volume", "exists", "techriders_sql_data")
    if ($LASTEXITCODE -ne 0) {
        Invoke-PodmanCommand -CommandArgs @("volume", "create", "techriders_sql_data")
    }
}

Ensure-ContainerRemoved -Name "techriders-api"
Ensure-ContainerRemoved -Name "techriders-web"
Ensure-ContainerRemoved -Name "techriders-sql"

Invoke-PodmanCommand -CommandArgs @("build", "-f", "backend/TechRiders.API/Containerfile", "-t", "techriders-api:local", ".")
Invoke-PodmanCommand -CommandArgs @("build", "-f", "techito/Containerfile", "-t", "techriders-web:local", ".")

if ((-not $useInMemoryDatabase) -and (-not $envMap.ContainsKey('SQL_SA_PASSWORD'))) {
    throw "SQL_SA_PASSWORD is required in .env"
}
if (-not $envMap.ContainsKey('JWT_KEY')) {
    throw "JWT_KEY is required in .env"
}

$sqlPassword = if ($envMap.ContainsKey('SQL_SA_PASSWORD')) { $envMap['SQL_SA_PASSWORD'] } else { "<SQL_SA_PASSWORD>" }
$jwtKey = $envMap['JWT_KEY']

if (-not $useInMemoryDatabase) {
    Invoke-PodmanCommand -CommandArgs @(
        "run", "-d",
        "--name", "techriders-sql",
        "--network", "techriders-net",
        "-p", "14333:1433",
        "-e", "ACCEPT_EULA=Y",
        "-e", "MSSQL_SA_PASSWORD=$sqlPassword",
        "-v", "techriders_sql_data:/var/opt/mssql",
        "mcr.microsoft.com/mssql/server:2022-latest"
    )
}

$apiRunArgs = @(
    "run", "-d",
    "--name", "techriders-api",
    "--network", "techriders-net",
    "-p", "8080:8080",
    "-e", "ASPNETCORE_ENVIRONMENT=Development",
    "-e", "Database__UseInMemory=$($useInMemoryDatabase.ToString().ToLowerInvariant())",
    "-e", "Jwt__Key=$jwtKey",
    "-e", "Jwt__Issuer=TechRiders",
    "-e", "Jwt__Audience=TechRiders"
)

if (-not $useInMemoryDatabase) {
    $apiRunArgs += @(
        "-e", "ConnectionStrings__DefaultConnection=Server=techriders-sql,1433;Database=TechRidersDev;User Id=sa;Password=$sqlPassword;TrustServerCertificate=true"
    )
}

$apiRunArgs += "techriders-api:local"

Invoke-PodmanCommand -CommandArgs $apiRunArgs

Invoke-PodmanCommand -CommandArgs @(
    "run", "-d",
    "--name", "techriders-web",
    "--network", "techriders-net",
    "-p", "4200:80",
    "techriders-web:local"
)

Write-Host "Tech Riders Podman stack started."
Write-Host "Frontend: http://localhost:4200"
Write-Host "API: http://localhost:8080/swagger"
if ($useInMemoryDatabase) {
    Write-Host "Database mode: InMemory"
}
else {
    Write-Host "SQL: localhost,14333"
}

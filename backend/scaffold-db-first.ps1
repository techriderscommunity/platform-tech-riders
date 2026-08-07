param(
    [string]$ConnectionString,
    [string]$Server = '(localdb)\MSSQLLocalDB',
    [string]$Database = 'TechRidersDev',
    [string]$Project = '.\TechRiders.Infrastructure\TechRiders.Infrastructure.csproj',
    [string]$StartupProject = '.\TechRiders.Api\TechRiders.Api.csproj',
    [string]$Provider = 'Microsoft.EntityFrameworkCore.SqlServer',
    [string]$ContextName = 'TechRidersDatabaseFirstContext',
    [string]$ContextDir = 'Data\DatabaseFirstSnapshot',
    [string]$OutputDir = 'Data\DatabaseFirstSnapshot\Entities',
    [string[]]$Schemas = @('dbo'),
    [switch]$Force,
    [switch]$NoBuild,
    [switch]$DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-ResolvedConnectionString {
    param(
        [string]$ExplicitConnectionString,
        [string]$ResolvedServer,
        [string]$ResolvedDatabase
    )

    if (-not [string]::IsNullOrWhiteSpace($ExplicitConnectionString)) {
        return $ExplicitConnectionString
    }

    if (-not [string]::IsNullOrWhiteSpace($env:TECHRIDERS_DB_CONNECTIONSTRING)) {
        return $env:TECHRIDERS_DB_CONNECTIONSTRING
    }

    return "Server=$ResolvedServer;Database=$ResolvedDatabase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;Encrypt=False;"
}

$repoRoot = $PSScriptRoot
$connectionStringValue = Get-ResolvedConnectionString -ExplicitConnectionString $ConnectionString -ResolvedServer $Server -ResolvedDatabase $Database

$commandParts = @(
    'ef',
    'dbcontext',
    'scaffold',
    $connectionStringValue,
    $Provider,
    '--project', $Project,
    '--startup-project', $StartupProject,
    '--context', $ContextName,
    '--context-dir', $ContextDir,
    '--output-dir', $OutputDir,
    '--no-onconfiguring',
    '--use-database-names',
    '--no-pluralize'
)

foreach ($schema in $Schemas) {
    $commandParts += @('--schema', $schema)
}

if ($Force) {
    $commandParts += '--force'
}

if ($NoBuild) {
    $commandParts += '--no-build'
}

Write-Host "[database-first] startup project : $StartupProject"
Write-Host "[database-first] target project  : $Project"
Write-Host "[database-first] database        : $Database"
Write-Host "[database-first] context         : $ContextName"
Write-Host "[database-first] output          : $OutputDir"

if ($DryRun) {
    Write-Host '[database-first] dry-run command:'
    Write-Host ("dotnet " + ($commandParts -join ' '))
    exit 0
}

Push-Location $repoRoot
try {
    & dotnet @commandParts
}
finally {
    Pop-Location
}
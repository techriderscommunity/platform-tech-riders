param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName,
    [string]$Project = '.\TechRiders.Infrastructure\TechRiders.Infrastructure.csproj',
    [string]$StartupProject = '.\TechRiders.Api\TechRiders.Api.csproj',
    [switch]$NoBuild,
    [switch]$UpdateDatabase,
    [switch]$DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot

$commandParts = @(
    'ef',
    'migrations',
    'add',
    $MigrationName,
    '--project', $Project,
    '--startup-project', $StartupProject
)

if ($NoBuild) {
    $commandParts += '--no-build'
}

Write-Host "[code-first] startup project : $StartupProject"
Write-Host "[code-first] target project  : $Project"
Write-Host "[code-first] migration       : $MigrationName"

if ($DryRun) {
    Write-Host '[code-first] dry-run command:'
    Write-Host ("dotnet " + ($commandParts -join ' '))
    exit 0
}

Push-Location $repoRoot
try {
    & dotnet @commandParts

    if ($UpdateDatabase) {
        & dotnet ef database update --project $Project --startup-project $StartupProject
    }
}
finally {
    Pop-Location
}
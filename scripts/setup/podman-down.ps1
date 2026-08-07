$ErrorActionPreference = 'Continue'

Write-Host "Stopping Tech Riders Podman stack..."

$containers = @("techriders-web", "techriders-api", "techriders-sql")
foreach ($name in $containers) {
    $exists = podman ps -a --format "{{.Names}}" | Where-Object { $_ -eq $name }
    if ($exists) {
        Write-Host "[EXEC] podman rm -f $name"
        podman rm -f $name | Out-Null
    }
}

Write-Host "Tech Riders Podman containers removed."
Write-Host "Data volume remains: techriders_sql_data"

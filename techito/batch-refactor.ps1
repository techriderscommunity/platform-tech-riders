$files = @(
  'src\app\features\capacitacion\solicita.scss',
  'src\app\features\empleo\embajador.scss',
  'src\app\features\onboarding\perfil-usuario.scss'
)

foreach ($file in $files) {
  if (Test-Path $file) {
    Write-Host "Processing: $(Split-Path $file -Leaf)"
    $content = Get-Content $file -Raw
    
    # Standard replacements
    $content = $content -replace 'padding:\s*2rem\b', 'padding: var(--space-8)'
    $content = $content -replace 'margin:\s*2rem\b', 'margin: var(--space-8)'
    $content = $content -replace 'gap:\s*1rem\b', 'gap: var(--space-4)'
    $content = $content -replace 'padding:\s*1rem\b', 'padding: var(--space-4)'
    $content = $content -replace 'margin-bottom:\s*1rem\b', 'margin-bottom: var(--space-4)'
    $content = $content -replace 'padding:\s*1\.5rem\b', 'padding: var(--space-6)'
    $content = $content -replace 'gap:\s*1\.5rem\b', 'gap: var(--space-6)'
    $content = $content -replace 'margin-bottom:\s*1\.5rem\b', 'margin-bottom: var(--space-6)'
    $content = $content -replace '\bwhite\b(?!-)', 'var(--text-inverse)'
    $content = $content -replace 'var\(--primary-color\)', 'var(--tr-blue)'
    $content = $content -replace 'var\(--text-dark\)', 'var(--text-primary)'
    $content = $content -replace 'var\(--text-gray\)', 'var(--text-secondary)'
    
    Set-Content $file -Value $content
    Write-Host "  ✓ Updated"
  }
}

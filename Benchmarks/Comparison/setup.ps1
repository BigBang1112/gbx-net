param([string]$Python = "python", [string]$Npm = "npm")

$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot
try {
    if (-not (Test-Path .venv)) {
        & $Python -m venv .venv
        if ($LASTEXITCODE -ne 0) { throw "Creating the Python environment failed." }
    }
    $venvPython = if ($IsLinux -or $IsMacOS) { ".venv/bin/python" } else { ".venv/Scripts/python.exe" }
    & $venvPython -m pip install -r requirements.txt
    if ($LASTEXITCODE -ne 0) { throw "Installing Python dependencies failed." }
    & $Npm ci --ignore-scripts --no-audit --no-fund
    if ($LASTEXITCODE -ne 0) { throw "Installing Node dependencies failed." }

    $dependencies = Get-Content dependencies.json -Raw | ConvertFrom-Json
    $sources = @(
        @{ Name = "gbx-py"; Repository = $dependencies.gbxPyRepository; Revision = $dependencies.gbxPyRevision },
        @{ Name = "ManiaPlanetSharp"; Repository = $dependencies.maniaPlanetSharpRepository; Revision = $dependencies.maniaPlanetSharpRevision }
    )
    foreach ($source in $sources) {
        $checkout = Join-Path .deps $source.Name
        if (-not (Test-Path -LiteralPath $checkout)) {
            git clone --no-checkout $source.Repository $checkout
            if ($LASTEXITCODE -ne 0) { throw "Cloning $($source.Name) failed." }
            git -C $checkout checkout --detach $source.Revision
            if ($LASTEXITCODE -ne 0) { throw "Checking out $($source.Name) failed." }
        }
        $revision = git -C $checkout rev-parse HEAD
        if ($LASTEXITCODE -ne 0 -or $revision -ne $source.Revision) {
            throw "The $($source.Name) checkout does not match dependencies.json. Use a separate checkout or restore it manually."
        }
        $changes = git -C $checkout status --porcelain --untracked-files=no
        if ($LASTEXITCODE -ne 0 -or $changes) { throw "The $($source.Name) checkout has tracked changes." }
    }
}
finally {
    Pop-Location
}

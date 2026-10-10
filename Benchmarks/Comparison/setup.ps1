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
    if (-not (Test-Path .deps/gbx-py)) {
        git clone --no-checkout $dependencies.gbxPyRepository .deps/gbx-py
        if ($LASTEXITCODE -ne 0) { throw "Cloning gbx-py failed." }
        git -C .deps/gbx-py checkout --detach $dependencies.gbxPyRevision
        if ($LASTEXITCODE -ne 0) { throw "Checking out gbx-py failed." }
    }
    $revision = git -C .deps/gbx-py rev-parse HEAD
    if ($LASTEXITCODE -ne 0 -or $revision -ne $dependencies.gbxPyRevision) {
        throw "The gbx-py checkout does not match dependencies.json. Use a separate checkout or restore it manually."
    }
}
finally {
    Pop-Location
}

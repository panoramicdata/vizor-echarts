# Run Playwright E2E Tests
# This script starts the demo app and runs the tests

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  PanoramicData.ECharts E2E Test Runner" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Check if we're in the correct directory
$testProjectExists = Test-Path "PanoramicData.ECharts.Test\PanoramicData.ECharts.Test.csproj"
$demoProjectExists = Test-Path "PanoramicData.ECharts.Demo\PanoramicData.ECharts.Demo.csproj"

if (-not $testProjectExists -or -not $demoProjectExists) {
    Write-Host "ERROR: Please run this script from the src directory" -ForegroundColor Red
    Write-Host "Expected to find:" -ForegroundColor Yellow
    Write-Host "  - PanoramicData.ECharts.Test\" -ForegroundColor Gray
    Write-Host "  - PanoramicData.ECharts.Demo\" -ForegroundColor Gray
    exit 1
}

# Check if browsers are installed
$playwrightScript = "PanoramicData.ECharts.Test\bin\Debug\net10.0\playwright.ps1"
if (-not (Test-Path $playwrightScript)) {
    Write-Host "WARNING: Playwright browsers may not be installed yet." -ForegroundColor Yellow
    Write-Host "Run setup-playwright.ps1 in the test directory first." -ForegroundColor Yellow
    Write-Host ""
}

# Start demo application in background
Write-Host "Step 1: Starting demo application..." -ForegroundColor Yellow
$demoJob = Start-Job -ScriptBlock {
    param($demoPath)
    Set-Location $demoPath
    dotnet run
} -ArgumentList (Resolve-Path "PanoramicData.ECharts.Demo")

Write-Host "? Demo application starting (Job ID: $($demoJob.Id))" -ForegroundColor Green
Write-Host ""

# Wait for application to start
Write-Host "Step 2: Waiting for demo application to be ready..." -ForegroundColor Yellow
$timeout = 60 # seconds
$elapsed = 0
$ready = $false

while ($elapsed -lt $timeout) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5185" -TimeoutSec 2 -UseBasicParsing -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            $ready = $true
            break
        }
    }
    catch {
        # Not ready yet
    }
    
    Start-Sleep -Seconds 2
    $elapsed += 2
    Write-Host "." -NoNewline -ForegroundColor Gray
}

Write-Host ""

if (-not $ready) {
    Write-Host "ERROR: Demo application did not start within $timeout seconds" -ForegroundColor Red
    Write-Host "Stopping background job..." -ForegroundColor Yellow
    Stop-Job -Job $demoJob
    Remove-Job -Job $demoJob
    exit 1
}

Write-Host "? Demo application is ready at http://localhost:5185" -ForegroundColor Green
Write-Host ""

# Run tests
Write-Host "Step 3: Running Playwright tests..." -ForegroundColor Yellow
Write-Host ""

Push-Location "PanoramicData.ECharts.Test"

try {
    dotnet test --logger "console;verbosity=normal"
    $testExitCode = $LASTEXITCODE
}
finally {
    Pop-Location
}

Write-Host ""

# Cleanup
Write-Host "Step 4: Stopping demo application..." -ForegroundColor Yellow
Stop-Job -Job $demoJob
Remove-Job -Job $demoJob
Write-Host "? Demo application stopped" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "==================================================" -ForegroundColor Cyan
if ($testExitCode -eq 0) {
    Write-Host "  Tests Passed! ?" -ForegroundColor Green
} else {
    Write-Host "  Tests Failed ?" -ForegroundColor Red
}
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

if ($testExitCode -eq 0) {
    Write-Host "Screenshots saved to:" -ForegroundColor Cyan
    Write-Host "  PanoramicData.ECharts.Test\bin\Debug\net10.0\screenshots\" -ForegroundColor Gray
    Write-Host ""
}

exit $testExitCode

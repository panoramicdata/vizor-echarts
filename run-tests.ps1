# Run Playwright E2E Tests
# This script starts the demo app and runs the tests

$InformationPreference = 'Continue'

Write-Information "==================================================" -InformationAction Continue
Write-Information "  PanoramicData.ECharts E2E Test Runner" -InformationAction Continue
Write-Information "==================================================" -InformationAction Continue
Write-Information ""

# Check if we're in the correct directory
$testProjectExists = Test-Path "PanoramicData.ECharts.Test\PanoramicData.ECharts.Test.csproj"
$demoProjectExists = Test-Path "PanoramicData.ECharts.Demo\PanoramicData.ECharts.Demo.csproj"

if (-not $testProjectExists -or -not $demoProjectExists) {
    Write-Information "ERROR: Please run this script from the src directory" -InformationAction Continue
    Write-Information "Expected to find:" -InformationAction Continue
    Write-Information "  - PanoramicData.ECharts.Test\" -InformationAction Continue
    Write-Information "  - PanoramicData.ECharts.Demo\" -InformationAction Continue
    exit 1
}

# Check if browsers are installed
$playwrightScript = "PanoramicData.ECharts.Test\bin\Debug\net10.0\playwright.ps1"
if (-not (Test-Path $playwrightScript)) {
    Write-Information "WARNING: Playwright browsers may not be installed yet." -InformationAction Continue
    Write-Information "Run setup-playwright.ps1 in the test directory first." -InformationAction Continue
    Write-Information ""
}

# Start demo application in background
Write-Information "Step 1: Starting demo application..." -InformationAction Continue
$demoJob = Start-Job -ScriptBlock {
    param($demoPath)
    Set-Location $demoPath
    dotnet run
} -ArgumentList (Resolve-Path "PanoramicData.ECharts.Demo")

Write-Information "✓ Demo application starting (Job ID: $($demoJob.Id))" -InformationAction Continue
Write-Information ""

# Wait for application to start
Write-Information "Step 2: Waiting for demo application to be ready..." -InformationAction Continue
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
    Write-Information "." -InformationAction Continue
}

Write-Information ""

if (-not $ready) {
    Write-Information "ERROR: Demo application did not start within $timeout seconds" -InformationAction Continue
    Write-Information "Stopping background job..." -InformationAction Continue
    Stop-Job -Job $demoJob
    Remove-Job -Job $demoJob
    exit 1
}

Write-Information "✓ Demo application is ready at http://localhost:5185" -InformationAction Continue
Write-Information ""

# Run tests
Write-Information "Step 3: Running Playwright tests..." -InformationAction Continue
Write-Information ""

Push-Location "PanoramicData.ECharts.Test"

try {
    dotnet test --logger "console;verbosity=normal"
    $testExitCode = $LASTEXITCODE
}
finally {
    Pop-Location
}

Write-Information ""

# Cleanup
Write-Information "Step 4: Stopping demo application..." -InformationAction Continue
Stop-Job -Job $demoJob
Remove-Job -Job $demoJob
Write-Information "✓ Demo application stopped" -InformationAction Continue
Write-Information ""

# Summary
Write-Information "==================================================" -InformationAction Continue
if ($testExitCode -eq 0) {
    Write-Information "  Tests Passed! ✓" -InformationAction Continue
} else {
    Write-Information "  Tests Failed ✗" -InformationAction Continue
}
Write-Information "==================================================" -InformationAction Continue
Write-Information ""

if ($testExitCode -eq 0) {
    Write-Information "Screenshots saved to:" -InformationAction Continue
    Write-Information "  PanoramicData.ECharts.Test\bin\Debug\net10.0\screenshots\" -InformationAction Continue
    Write-Information ""
}

exit $testExitCode
